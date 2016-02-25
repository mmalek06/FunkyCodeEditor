using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;

namespace CodeEditor.Adorners.FoldingAdorner {
    internal class Adorner : ReactiveAdorner {

        #region fields

        private readonly VisualCollection visuals;

        private readonly List<VisualElementSymbol> symbols;

        private IFoldingAlgorithm foldingAlgorithm;

        private Dictionary<TextPosition, TextPosition> foldingPositions;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        public Adorner(UIElement adornedElement) : base(adornedElement) {
            visuals = new VisualCollection(this);
            symbols = new List<VisualElementSymbol>();
            foldingAlgorithm = EditorConfiguration.GetFoldingAlgorithm();
            foldingPositions = new Dictionary<TextPosition, TextPosition>();
            Margin = new Thickness(EditorConfiguration.GetLinesColumnWidth(), 0, 0, 0);
        }

        #endregion

        #region event handlers

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

        protected override Visual GetVisualChild(int index) => visuals[index];

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(
                0, 0, EditorConfiguration.GetFoldingColumnWidth(), RenderSize.Height));

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
