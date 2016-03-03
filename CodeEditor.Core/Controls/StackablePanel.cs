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

        #endregion

    }
}
