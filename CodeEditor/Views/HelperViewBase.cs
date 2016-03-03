using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.DataStructures;

namespace CodeEditor.Views {
    internal abstract class HelperViewBase : FrameworkElement {

        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        public HelperViewBase() {
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
