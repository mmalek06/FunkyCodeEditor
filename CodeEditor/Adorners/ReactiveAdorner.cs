using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using CodeEditor.DataStructures;

namespace CodeEditor.Adorners {
    internal abstract class ReactiveAdorner : Adorner {

        #region constructor

        public ReactiveAdorner(UIElement adornedElement) : base(adornedElement) { }

        #endregion

        #region event handlers

        public abstract void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition);

        #endregion

    }
}
