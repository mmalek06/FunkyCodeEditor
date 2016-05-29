using System.Windows;
using System.Windows.Controls;

namespace CodeEditor.Controls.Base {
    public abstract class StackablePanel : StackPanel {

        #region properties

        protected abstract FrameworkElement MeasurementElement { get; }

        #endregion

        #region methods

        protected override Size ArrangeOverride(Size arrangeSize) {
            foreach (var child in Children) {
                var uiElement = (UIElement)child;
                var rcChild = new Rect(0, 0, ActualWidth, ActualHeight);

                uiElement.Arrange(rcChild);
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint) {
            var finalSize = base.MeasureOverride(constraint);

            if (Children.Count == 0) {
                return finalSize;
            }

            double h = finalSize.Height;
            double w = finalSize.Width;

            if (h > MeasurementElement.Height) {
                h = MeasurementElement.Height;
            }
            if (w > MeasurementElement.Width) {
                w = MeasurementElement.Width;
            }

            return new Size(w, h);
        }

        #endregion

    }
}
