using System.Windows;
using System.Windows.Media;
using CodeEditor.Messaging;

namespace CodeEditor.Views.BaseClasses {
    internal abstract class ViewBase : FrameworkElement {

        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        public int EditorCode { get; set; }

        public Postbox Postbox { get; set; }

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        protected ViewBase() {
            FocusVisualStyle = null;
            visuals = new VisualCollection(this);
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => visuals[index];

        #endregion

    }
}
