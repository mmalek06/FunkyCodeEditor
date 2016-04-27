using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Folding {
    internal class FoldingView : HelperViewBase {

        #region fields

        private IFoldingAlgorithm foldingAlgorithm;

        private Dictionary<FoldingPositionInfo, FoldingPositionInfo> foldingPositions;

        #endregion

        #region constructor

        public FoldingView() : base() {
            bgBrush = EditorConfiguration.GetFoldingColumnBrush();
            foldingAlgorithm = EditorConfiguration.GetFoldingAlgorithm();
            foldingPositions = new Dictionary<FoldingPositionInfo, FoldingPositionInfo>();
            Margin = new Thickness(EditorConfiguration.GetLinesColumnWidth(), 0, 0, 0);
        }

        #endregion

        #region event handlers

        public override void HandleTextInput(TextAddedMessage message) {
            if (message.Text == TextProperties.Properties.NEWLINE) {
                MoveFoldsDown(message.PrevCaretPosition);
            } else {
                if (!foldingAlgorithm.CanRun(message.Text)) {
                    return;
                }

                RunFolding(message.Text, message.NewCaretPosition);
            }

            RedrawFolds();
        }

        public override void HandleTextRemove(TextRemovedMessage message) {
            if (message.RemovedText == string.Empty) {
                MoveFoldsUp(message.NewCaretPosition);
            } else {
                var positions = GetClosedFoldingInfos().ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);
                var removedKey = foldingAlgorithm.DeleteFolds(message.RemovedText, message.NewCaretPosition, positions);

                if (removedKey == null) {
                    return;
                }

                DeleteFolds(removedKey, message.RemovedText);
            }

            RedrawFolds();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var position = e.GetPosition(this).GetDocumentPosition(TextConfiguration.GetCharSize());
            var folding = foldingPositions.Keys.FirstOrDefault(foldingInfo => foldingInfo.Position.Line == position.Line);

            if (folding != null) {
                var state = folding.State == FoldingStates.EXPANDED ? FoldingStates.FOLDED : FoldingStates.EXPANDED;

                Postbox.Instance.Send(new FoldClickedMessage {
                    Area = new TextRange {
                        StartPosition = folding.Position,
                        // don't know why this throws KeyNotFoundException in a case when folding was moved: EndPosition = foldingPositions[folding].Position
                        // the key is there...
                        EndPosition = foldingPositions.First(pair => pair.Key == folding).Value.Position
                    },
                    ClosingTag = foldingAlgorithm.GetClosingTag(),
                    OpeningTag = foldingAlgorithm.GetOpeningTag(),
                    State = state
                });
                folding.State = state;

                RedrawFolds();
            }
        }

        #endregion

        #region methods

        protected override double GetWidth() => EditorConfiguration.GetFoldingColumnWidth();

        private void RunFolding(string text, TextPosition caretPosition) {
            var folds = foldingAlgorithm.CreateFolds(text, new TextPosition(column: caretPosition.Column - 1, line: caretPosition.Line), GetPotentialFoldingPositions());

            if (folds == null || !folds.Any()) {
                return;
            }

            CreateFolds(folds);
            MakeFoldsUnique();
            RedrawFolds();
        }

        private void MoveFoldsDown(TextPosition prevCaretPosition) {
            var foldsAfterCaret = foldingPositions.Where(pair => pair.Key.Position >= prevCaretPosition);
            
            foreach (var fold in foldsAfterCaret) {
                fold.Key.Position = new TextPosition(column: fold.Key.Position.Column, line: fold.Key.Position.Line + 1);

                if (fold.Value.Position != null) {
                    fold.Value.Position = new TextPosition(column: fold.Value.Position.Column, line: fold.Value.Position.Line + 1);
                }
            }
        }

        private void MoveFoldsUp(TextPosition newCaretPosition) {
            var foldsAfterCaret = foldingPositions.Where(pair => pair.Key.Position > newCaretPosition);
            var foldsWithClosingTagAfterPosition = foldingPositions.Except(foldsAfterCaret)
                                                                   .Where(pair => pair.Value.Position != null && pair.Value.Position.Line > newCaretPosition.Line);

            foreach (var fold in foldsAfterCaret) {
                fold.Key.Position = new TextPosition(column: fold.Key.Position.Column, line: fold.Key.Position.Line - 1);

                if (fold.Value.Position != null) {
                    fold.Value.Position = new TextPosition(column: fold.Value.Position.Column, line: fold.Value.Position.Line - 1);
                }
            }
            foreach (var fold in foldsWithClosingTagAfterPosition) {
                fold.Value.Position = new TextPosition(column: fold.Value.Position.Column, line: fold.Value.Position.Line - 1);
            }
        }

        private void MakeFoldsUnique() {
            var repeatingFolds = foldingAlgorithm.GetRepeatingFolds(foldingPositions.ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position));

            foreach (var repeatingKey in repeatingFolds) {
                var position = foldingPositions.First(pair => pair.Key.Position == repeatingKey);

                position.Key.Deleted = true;
            }
        }
        
        private IDictionary<TextPosition, TextPosition> GetPotentialFoldingPositions() =>
            foldingPositions.Where(pair => pair.Key.Deleted || pair.Value == null || pair.Value.Deleted || pair.Value.Position == null)
                            .Distinct()
                            .ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);

        private IDictionary<FoldingPositionInfo, FoldingPositionInfo> GetClosedFoldingInfos() =>
            foldingPositions.Where(pair => !pair.Key.Deleted && pair.Value != null && !pair.Value.Deleted && pair.Value.Position != null)
                            .Distinct()
                            .ToDictionary(pair => pair.Key, pair => pair.Value);

        private void CreateFolds(IDictionary<TextPosition, TextPosition> folds) {
            foreach (var kvp in folds) {
                var tmpKey = new FoldingPositionInfo { Deleted = false, Position = kvp.Key };
                var existingKey = foldingPositions.Keys.FirstOrDefault(k => k.Equals(tmpKey));

                if (existingKey != null) {
                    existingKey.Deleted = false;
                } else {
                    existingKey = tmpKey;
                }

                foldingPositions[existingKey] = new FoldingPositionInfo { Deleted = false, Position = kvp.Value };
            }
        }

        private void DeleteFolds(TextPosition removedKey, string removedText) {
            var info = foldingPositions.Keys.FirstOrDefault(k => k.Position == removedKey);

            if (info == null) {
                return;
            }

            if (foldingAlgorithm.IsOpeningTag(removedText)) {
                if (foldingPositions[info] == null || foldingPositions[info].Deleted || foldingPositions[info].Position == null) {
                    foldingPositions.Remove(info);
                } else {
                    info.Deleted = true;
                }
            } else if (foldingAlgorithm.IsCollapseRepresentation(removedText)) {
                foldingPositions.Remove(info);
            } else {
                foldingPositions[info].Deleted = true;
            }
        }

        private void RedrawFolds() {
            visuals.Clear();

            foreach (var kvp in GetClosedFoldingInfos()) {
                var symbol = new VisualElementSymbol();
                int top = (int)kvp.Key.Position.GetPositionRelativeToParent(TextConfiguration.GetCharSize()).Y;

                symbol.DrawFolding(kvp.Key.State, top);

                visuals.Add(symbol);
            }
        }

        #endregion

    }
}
