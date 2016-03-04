using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.DataStructures;

namespace CodeEditor.Views.BaseClasses {
    internal abstract class HelperViewBase : ViewBase {

        #region fields

        protected Brush bgBrush;

        #endregion

        #region constructor

        public HelperViewBase() : base() {
            bgBrush = Brushes.Transparent;
        }

        #endregion

        #region event handlers

        public abstract void HandleTextInput(string text, TextPosition activePosition);

        public abstract void HandleTextRemove(Key key, TextPosition activePosition);

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(bgBrush, null, new Rect(0, 0, GetWidth(), ActualHeight));

        #endregion

        #region methods

        protected abstract double GetWidth();

        #endregion

    }
}
