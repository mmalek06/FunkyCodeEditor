using System.Windows;
using System.Windows.Media;

namespace CodeEditor.Views.BaseClasses {
    internal abstract class ViewBase : FrameworkElement {
        
        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count;

        #endregion

        #region constructor

        public ViewBase() {
            FocusVisualStyle = null;
            visuals = new VisualCollection(this);
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => visuals[index];

        #endregion

    }
}
