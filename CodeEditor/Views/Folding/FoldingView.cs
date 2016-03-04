using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;

namespace CodeEditor.Views.Folding {
    internal class FoldingView : HelperViewBase {

        #region fields

        private readonly List<VisualElementSymbol> symbols;
        private IFoldingAlgorithm foldingAlgorithm;
        private Dictionary<TextPosition, TextPosition> foldingPositions;

        #endregion

        #region constructor

        public FoldingView() : base() {
            bgBrush = EditorConfiguration.GetFoldingColumnBrush();
            symbols = new List<VisualElementSymbol>();
            foldingAlgorithm = EditorConfiguration.GetFoldingAlgorithm();
            foldingPositions = new Dictionary<TextPosition, TextPosition>();
            Margin = new Thickness(EditorConfiguration.GetLinesColumnWidth(), 0, 0, 0);
        }

        #endregion

        #region event handlers

        public override void HandleTextInput(KeyEventArgs e, TextPosition activePosition) {
            
        }

        public override void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition) {
            char character = e.Text[0];

            if (!foldingAlgorithm.CanRun(e.Text)) {
                return;
            }

            var folds = foldingAlgorithm.CreateFolds(e.Text, activePosition, foldingPositions);

            if (folds == null || !folds.Any()) {
                return;
            }
            foreach (var kvp in folds) {
                foldingPositions[kvp.Key] = kvp.Value;
            }

            RedrawFolds();
        }

        #endregion

        #region methods

        protected override double GetWidth() => EditorConfiguration.GetFoldingColumnWidth();

        private void RedrawFolds() {
            visuals.Clear();
            symbols.Clear();

            foreach (var kvp in foldingPositions.Where(pair => pair.Value != null)) {
                var symbol = new VisualElementSymbol();
                int top = (int)kvp.Key.GetPositionRelativeToParent().Y;

                symbol.DrawFolding(TextConfiguration.GetGlobalTextRunProperties(), FoldingStates.EXPANDED, top);

                symbols.Add(symbol);
                visuals.Add(symbol);
            }
        }

        #endregion

    }
}
