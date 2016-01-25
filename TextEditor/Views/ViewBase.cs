using System.Windows;
using System.Windows.Media;

namespace TextEditor.Views {
    public abstract class ViewBase : FrameworkElement {
        protected VisualCollection visuals;

        protected override int VisualChildrenCount {
            get { return visuals.Count; }
        }

        public ViewBase() {
            Focusable = true;

            visuals = new VisualCollection(this);
        }

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(Brushes.Transparent, null,
                new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        }

        protected override Visual GetVisualChild(int index) {
            return visuals[index];
        }
    }
}
