using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.DataStructures;

namespace CodeEditor.Adorners {
    internal abstract class ReactiveAdorner : Adorner {

        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        public ReactiveAdorner(UIElement adornedElement) : base(adornedElement) {
            visuals = new VisualCollection(this);
        }

        #endregion

        #region event handlers

        public abstract void HandleTextInput(KeyEventArgs e, TextPosition activePosition);

        public abstract void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition);

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => visuals[index];

        #endregion

    }
}
