using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;

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

        public virtual void HandleTextInput(string text, TextPosition activePosition) { }

        public virtual void HandleTextRemove(TextRemovedMessage message) { }

        public virtual void HandleLinesRemove(int count) { }

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(bgBrush, null, new Rect(0, 0, GetWidth(), ActualHeight));

        #endregion

        #region methods

        protected abstract double GetWidth();

        #endregion

    }
}
