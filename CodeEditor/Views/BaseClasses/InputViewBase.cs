using System.Windows;
using System.Windows.Media;

namespace CodeEditor.Views.BaseClasses {
    internal abstract class InputViewBase : ViewBase {
        
        #region constructor

        public InputViewBase() : base() {
            Focusable = true;
        }

        #endregion

        #region methods

        protected override void OnRender(DrawingContext drawingContext) => 
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Width, Height));

        #endregion

    }
}
