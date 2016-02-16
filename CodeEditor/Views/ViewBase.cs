using System.Windows;
using System.Windows.Media;

namespace CodeEditor.Views {
    public abstract class ViewBase : FrameworkElement {

        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        protected override int VisualChildrenCount {
            get { return visuals.Count; }
        }

        #endregion

        #region constructor

        public ViewBase() {
            Focusable = true;
            visuals = new VisualCollection(this);
        }

        #endregion

        #region methods

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(Brushes.Transparent, null,
                new Rect(Margin.Left, Margin.Top, RenderSize.Width - Margin.Right, RenderSize.Height - Margin.Bottom));
        }

        protected override Visual GetVisualChild(int index) {
            return visuals[index];
        }

        #endregion

    }
}
