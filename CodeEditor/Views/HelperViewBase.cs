using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.DataStructures;

namespace CodeEditor.Views {
    internal abstract class HelperViewBase : FrameworkElement {

        #region fields

        protected VisualCollection visuals;

        protected Brush bgBrush;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        public HelperViewBase() {
            visuals = new VisualCollection(this);
            bgBrush = Brushes.Transparent;
        }

        #endregion

        #region event handlers

        public abstract void HandleTextInput(KeyEventArgs e, TextPosition activePosition);

        public abstract void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition);

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(bgBrush, null, new Rect(0, 0, GetWidth(), ActualHeight));

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => visuals[index];

        protected abstract double GetWidth();

        #endregion

    }
}
