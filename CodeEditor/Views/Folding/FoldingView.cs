using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Extensions;
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

        public override void HandleTextInput(string text, TextPosition activePosition) {
            char character = text[0];

            if (!foldingAlgorithm.CanRun(text)) {
                return;
            }
            
            var folds = foldingAlgorithm.CreateFolds(text, new TextPosition(column: activePosition.Column - 1, line: activePosition.Line), GetPotentialFoldingPositions());

            if (folds == null || !folds.Any()) {
                return;
            }
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

            RedrawFolds();
        }

        public override void HandleTextRemove(string removedText, Key key, TextPosition activePosition) {
            if (removedText == string.Empty) {
                return;
            }

            var removedKey = foldingAlgorithm.DeleteFolds(removedText, activePosition, GetClosedFoldingPositions());

            if (removedKey == null) {
                return;
            }

            var info = foldingPositions.Keys.First(k => k.Position == removedKey);

            if (foldingAlgorithm.IsOpeningTag(removedText)) {
                if (foldingPositions[info] == null || foldingPositions[info].Deleted || foldingPositions[info].Position == null) {
                    foldingPositions.Remove(info);
                } else {
                    info.Deleted = true;
                }
            } else {
                foldingPositions[info].Deleted = true;
            }

            RedrawFolds();
        }

        #endregion

        #region methods

        protected override double GetWidth() => EditorConfiguration.GetFoldingColumnWidth();

        private IDictionary<TextPosition, TextPosition> GetPotentialFoldingPositions() =>
            foldingPositions.Where(pair => pair.Key.Deleted || pair.Value == null || pair.Value.Deleted || pair.Value.Position == null)
                            .ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);

        private IDictionary<TextPosition, TextPosition> GetClosedFoldingPositions() =>
            foldingPositions.Where(pair => !pair.Key.Deleted && pair.Value != null && !pair.Value.Deleted && pair.Value.Position != null)
                            .ToDictionary(pair => pair.Key.Position, pair => pair.Value.Position);

        private void RedrawFolds() {
            visuals.Clear();
            symbols.Clear();

            foreach (var kvp in GetClosedFoldingPositions()) {
                var symbol = new VisualElementSymbol();
                int top = (int)kvp.Key.GetPositionRelativeToParent().Y;

                symbol.DrawFolding(FoldingStates.EXPANDED, top);

                symbols.Add(symbol);
                visuals.Add(symbol);
            }
        }

        #endregion

        #region classes

        private class FoldingPositionInfo {

            #region properties

            public TextPosition Position { get; set; }

            public bool Deleted { get; set; }

            #endregion

            #region public methods

            public override bool Equals(object obj) {
                var other = (FoldingPositionInfo)obj;

                if (other == null || other.Position == null) {
                    return false;
                }

                return Position.Equals(other.Position);
            }

            public override int GetHashCode() {
                return Position.GetHashCode();
            }

            public override string ToString() {
                return Position.ToString() + ", Deleted: " + Deleted.ToString();
            }

            #endregion

        }

        #endregion

    }
}
