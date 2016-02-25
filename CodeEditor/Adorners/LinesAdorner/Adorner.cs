using System;
using System.Windows;
using System.Windows.Input;
using CodeEditor.DataStructures;

namespace CodeEditor.Adorners.LinesAdorner {
    internal class Adorner : ReactiveAdorner {

        #region constructor

        public Adorner(UIElement adornedElement) : base(adornedElement) {

        }

        #endregion

        #region event handlers

        public override void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition) {
            
        }

        #endregion

    }
}
