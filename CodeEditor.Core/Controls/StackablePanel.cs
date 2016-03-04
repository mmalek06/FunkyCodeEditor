using System.Windows;
using System.Windows.Controls;

namespace CodeEditor.Core.Controls {
    public abstract class StackablePanel : StackPanel {

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

            if (h > ((FrameworkElement)Children[0]).Height) {
                h = ((FrameworkElement)Children[0]).Height;
            }
            if (w > ((FrameworkElement)Children[0]).Width) {
                w = ((FrameworkElement)Children[0]).Width;
            }

            return new Size(w, h);
        }

        #endregion

    }
}
