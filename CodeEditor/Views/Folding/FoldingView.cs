﻿using System.Collections.Generic;
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

        private readonly List<VisualElementSymbol> symbols;
        private IFoldingAlgorithm foldingAlgorithm;
        private Dictionary<FoldingPositionInfo, FoldingPositionInfo> foldingPositions;

        #endregion

        #region constructor

        public FoldingView() : base() {
            bgBrush = EditorConfiguration.GetFoldingColumnBrush();
            symbols = new List<VisualElementSymbol>();
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
                        EndPosition = foldingPositions[folding].Position
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

        private void RunFolding(string text, TextPosition activePosition) {
            var folds = foldingAlgorithm.CreateFolds(text, new TextPosition(column: activePosition.Column - 1, line: activePosition.Line), GetPotentialFoldingPositions());

            if (folds == null || !folds.Any()) {
                return;
            }

            CreateFolds(folds);
            BalanceFolds();
            RedrawFolds();
        }

        private void MoveFoldsDown(TextPosition prevCaretPosition) {
            var foldsAfterCaret = foldingPositions.Where(pair => pair.Key.Position >= prevCaretPosition);
            var foldsWithClosingTagAfterPosition = foldingPositions.Except(foldsAfterCaret)
                                                                   .Where(pair => pair.Value.Position != null && pair.Value.Position.Line >= prevCaretPosition.Line);
            foreach (var fold in foldsAfterCaret) {
                fold.Key.Position = new TextPosition(column: fold.Key.Position.Column, line: fold.Key.Position.Line + 1);

                if (fold.Value.Position != null) {
                    fold.Value.Position = new TextPosition(column: fold.Value.Position.Column, line: fold.Value.Position.Line + 1);
                }
            }
            foreach (var fold in foldsWithClosingTagAfterPosition) {
                fold.Value.Position = new TextPosition(column: fold.Value.Position.Column, line: fold.Value.Position.Line + 1);
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

        private IDictionary<TextPosition, TextPosition> GetPotentialFoldingPositions() =>
            foldingPositions.Where(pair => pair.Key.Deleted || pair.Value == null || pair.Value.Deleted || pair.Value.Position == null)
                            .ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);

        private IDictionary<FoldingPositionInfo, FoldingPositionInfo> GetClosedFoldingInfos() =>
            foldingPositions.Where(pair => !pair.Key.Deleted && pair.Value != null && !pair.Value.Deleted && pair.Value.Position != null)
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

        private void BalanceFolds() {
            var foldsInSameLine = foldingPositions.GroupBy(pair => pair.Key.Position.Line)
                                                  .Where(group => group.Count() > 1)
                                                  .SelectMany(group => group)
                                                  .Select(pair => pair.Key);

            foreach (var repeatingFold in foldsInSameLine) {
                foldingPositions.Remove(repeatingFold);
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
            } else {
                foldingPositions[info].Deleted = true;
            }
        }

        private void RedrawFolds() {
            visuals.Clear();
            symbols.Clear();

            foreach (var kvp in GetClosedFoldingInfos()) {
                var symbol = new VisualElementSymbol();
                int top = (int)kvp.Key.Position.GetPositionRelativeToParent(TextConfiguration.GetCharSize()).Y;

                symbol.DrawFolding(kvp.Key.State, top);

                symbols.Add(symbol);
                visuals.Add(symbol);
            }
        }

        #endregion

    }
}
