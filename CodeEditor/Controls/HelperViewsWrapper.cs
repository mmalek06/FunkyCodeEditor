﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeEditor.Configuration;
using CodeEditor.Views;

namespace CodeEditor.Controls {
    internal class HelperViewsWrapper : StackPanel {

        #region fields

        private List<HelperViewBase> views;
        private Views.LinesView.View linesView;
        private Views.FoldingView.View foldingView;

        #endregion

        #region constructor

        public HelperViewsWrapper() : base() {
            views = new List<HelperViewBase>();
            Orientation = Orientation.Horizontal;
        }

        #endregion

        #region event handlers

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            
            InitEvents();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(EditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, Width, Height));

            if (linesView == null && foldingView == null) {
                SetupViews();
            }
        }

        #endregion

        #region methods

        private void InitEvents() {
            
        }

        private void SetupViews() {
            linesView = new Views.LinesView.View { Width = EditorConfiguration.GetLinesColumnWidth(), Height = ActualHeight };
            foldingView = new Views.FoldingView.View { Width = EditorConfiguration.GetFoldingColumnWidth(), Height = ActualHeight };

            views.Add(linesView);
            views.Add(foldingView);

            Children.Add(linesView);
            Children.Add(foldingView);
        }

        #endregion

    }
}
