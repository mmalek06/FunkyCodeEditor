using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
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
            
            var folds = foldingAlgorithm.CreateFolds(text, new TextPosition(column: activePosition.Column - 1, line: activePosition.Line), GetTextPositions());

            if (folds == null || !folds.Any()) {
                return;
            }
            foreach (var kvp in folds) {
                foldingPositions[new FoldingPositionInfo { Position = kvp.Key, Deleted = false }] = new FoldingPositionInfo { Position = kvp.Value, Deleted = false };
            }

            RedrawFolds();
        }

        public override void HandleTextRemove(string removedText, Key key, TextPosition activePosition) {
            if (removedText == string.Empty) {
                return;
            }

            var removedKey = foldingAlgorithm.DeleteFolds(removedText, activePosition, GetTextPositions());

            if (removedKey != null) {
                var info = foldingPositions.Keys.First(k => k.Position == removedKey);

                info.Deleted = true;

                foldingPositions[new FoldingPositionInfo { Position = removedKey, Deleted = true }] = null;

                RedrawFolds();
            }
        }

        #endregion

        #region methods

        protected override double GetWidth() => EditorConfiguration.GetFoldingColumnWidth();

        private IDictionary<TextPosition, TextPosition> GetTextPositions() =>
            foldingPositions.Where(kvp => !kvp.Key.Deleted && kvp.Value.Position != null && !kvp.Value.Deleted)
                            .Select(kvp => new KeyValuePair<TextPosition, TextPosition>(kvp.Key.Position, kvp.Value.Position))
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        private void RedrawFolds() {
            visuals.Clear();
            symbols.Clear();

            foreach (var kvp in GetTextPositions().Where(kvp => kvp.Value != null)) {
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

            public override int GetHashCode() {
                return Position.GetHashCode();
            }

            #endregion

        }

        #endregion

    }
}
