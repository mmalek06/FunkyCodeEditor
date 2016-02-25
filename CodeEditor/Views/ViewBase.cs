﻿using System.Windows;
using System.Windows.Media;

namespace CodeEditor.Views {
    public abstract class ViewBase : FrameworkElement {

        #region fields

        protected VisualCollection visuals;

        #endregion

        #region properties

        protected override int VisualChildrenCount => visuals.Count; 

        #endregion

        #region constructor

        public ViewBase() {
            Focusable = true;
            visuals = new VisualCollection(this);
        }

        #endregion

        #region methods

        protected override void OnRender(DrawingContext drawingContext) => 
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, 1), new Rect(0, 0, Width, Height));

        protected override Visual GetVisualChild(int index) => visuals[index];

        #endregion

    }
}
