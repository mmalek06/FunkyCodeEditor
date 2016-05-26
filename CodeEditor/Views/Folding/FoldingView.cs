using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Enums;
using CodeEditor.Core.Extensions;
using CodeEditor.Core.Messaging;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Folding {
    internal class FoldingView : HelperViewBase {

        #region fields

        private IFoldingAlgorithm foldingAlgorithm;

        private Dictionary<FoldingPositionInfo, FoldingPositionInfo> foldingPositions;

        #endregion

        #region properties

        private IFoldingAlgorithm FoldingAlgorithm =>
            foldingAlgorithm == null ? FoldingAlgorithmFactory.CreateAlgorithm(ConfigManager.GetConfig(EditorCode).Language) : foldingAlgorithm;

        #endregion

        #region constructor

        public FoldingView() : base() {
            bgBrush = SharedEditorConfiguration.GetFoldingColumnBrush();
            foldingPositions = new Dictionary<FoldingPositionInfo, FoldingPositionInfo>();
            Margin = new Thickness(SharedEditorConfiguration.GetLinesColumnWidth(), 0, 0, 0);
        }

        #endregion

        #region event handlers

        public override void HandleTextInput(TextAddedMessage message) {
            if (message.Text == TextProperties.Properties.NEWLINE) {
                if (IsCaretInbetweenTags(message.PrevCaretPosition)) {
                    IncreaseFoldHeight(message.PrevCaretPosition);
                } else {
                    MoveFoldsDown(message.PrevCaretPosition);
                }
            } else {
                if (!FoldingAlgorithm.CanRun(message.Text)) {
                    return;
                }

                RunFolding(message.Text, message.NewCaretPosition);
            }

            RedrawFolds();
        }

        public override void HandleTextRemove(TextRemovedMessage message) {
            if (message.RemovedText == string.Empty) {
                if (IsCaretInbetweenTags(message.OldCaretPosition)) {
                    DecreaseFoldHeight(message.OldCaretPosition);
                } else {
                    MoveFoldsUp(message.NewCaretPosition);
                }
            } else {
                var positions = GetClosedFoldingInfos().ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);
                var removedKey = FoldingAlgorithm.DeleteFolds(message.RemovedText, message.NewCaretPosition, positions);

                if (removedKey == null) {
                    return;
                }

                DeleteFolds(removedKey, message.RemovedText);
            }

            RedrawFolds();
        }

        public override void HandleLinesRemove(LinesRemovedMessage message) {
            
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var position = e.GetPosition(this).GetDocumentPosition(TextConfiguration.GetCharSize());

            RunFoldingOnClick(position);
        }
        
        #endregion

        #region methods

        protected override double GetWidth() => SharedEditorConfiguration.GetFoldingColumnWidth();

        private bool IsCaretInbetweenTags(TextPosition position) =>
            foldingPositions.Any(pair => position > pair.Key.Position && position <= pair.Value.Position);

        private void SendMessage(FoldingStates state, FoldingPositionInfo folding) {
            var areaBeforeFolding = new TextRange();
            var areaAfterFolding = new TextRange();

            if (state == FoldingStates.FOLDED) {
                areaBeforeFolding.StartPosition = folding.Position;
                areaBeforeFolding.EndPosition = foldingPositions.First(pair => pair.Key == folding).Value.Position;
                areaAfterFolding.StartPosition = folding.Position;
                areaAfterFolding.EndPosition = new TextPosition(column: folding.Position.Column + FoldingAlgorithm.GetCollapsibleRepresentation().Length, line: folding.Position.Line);
            } else {
                areaAfterFolding.StartPosition = folding.Position;
                areaAfterFolding.EndPosition = foldingPositions.First(pair => pair.Key == folding).Value.Position;
                areaBeforeFolding.StartPosition = folding.Position;
                areaBeforeFolding.EndPosition = new TextPosition(column: folding.Position.Column + FoldingAlgorithm.GetCollapsibleRepresentation().Length, line: folding.Position.Line);
            }

            Postbox.Put(new FoldClickedMessage {
                AreaBeforeFolding = areaBeforeFolding,
                AreaAfterFolding = areaAfterFolding,
                ClosingTag = FoldingAlgorithm.GetClosingTag(),
                OpeningTag = FoldingAlgorithm.GetOpeningTag(),
                State = state
            });
        }

        private void RunFoldingOnClick(TextPosition position) {
            var folding = foldingPositions.Keys.FirstOrDefault(foldingInfo => foldingInfo.Position.Line == position.Line);

            if (folding != null) {
                var state = folding.State == FoldingStates.EXPANDED ? FoldingStates.FOLDED : FoldingStates.EXPANDED;

                folding.State = state;

                SendMessage(state, folding);
                RedrawFolds();
            }
        }

        private void RunFolding(string text, TextPosition caretPosition) {
            var folds = FoldingAlgorithm.CreateFolds(text, new TextPosition(column: caretPosition.Column - 1, line: caretPosition.Line), GetPotentialFoldingPositions());

            if (folds == null || !folds.Any()) {
                return;
            }

            CreateFolds(folds);
            MakeFoldsUnique();
            RedrawFolds();
        }

        private void IncreaseFoldHeight(TextPosition position, int amount = 1) {
            var key = foldingPositions.First(pair => position >= pair.Key.Position && position <= pair.Value.Position).Key;
            var tmpPosition = foldingPositions[key].Position;

            foldingPositions[key].Position = new TextPosition(column: tmpPosition.Column, line: tmpPosition.Line + amount);
        }

        private void DecreaseFoldHeight(TextPosition position, int amount = 1) {
            var key = foldingPositions.First(pair => position >= pair.Key.Position && position <= pair.Value.Position).Key;
            var tmpPosition = foldingPositions[key].Position;

            foldingPositions[key].Position = new TextPosition(column: tmpPosition.Column, line: tmpPosition.Line - amount);
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
            var repeatingFolds = FoldingAlgorithm.GetRepeatingFolds(foldingPositions.ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position));

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

            if (FoldingAlgorithm.IsOpeningTag(removedText)) {
                if (foldingPositions[info] == null || foldingPositions[info].Deleted || foldingPositions[info].Position == null) {
                    foldingPositions.Remove(info);
                } else {
                    info.Deleted = true;
                }
            } else if (FoldingAlgorithm.IsCollapseRepresentation(removedText)) {
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
