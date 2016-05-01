﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Configuration;
using CodeEditor.Messaging;
using CodeEditor.Views.BaseClasses;
using CodeEditor.Views.Folding;
using CodeEditor.Views.Lines;

namespace CodeEditor.Controls {
    internal class HelperViewsWrapper : StackPanel {

        #region fields

        private List<HelperViewBase> views;
        private LinesView linesView;
        private FoldingView foldingView;

        #endregion

        #region constructor

        public HelperViewsWrapper() : base() {
            views = new List<HelperViewBase>();
            Orientation = Orientation.Horizontal;

            Postbox.Instance.For(typeof(TextAddedMessage)).Invoke(OnTextAdded)
                            .For(typeof(TextRemovedMessage)).Invoke(OnTextRemoved)
                            .For(typeof(LinesRemovedMessage)).Invoke(OnLineRemoved)
                            .For(typeof(FoldClickedMessage)).Invoke(OnFoldClicked);
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(EditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (linesView == null && foldingView == null) {
                SetupViews();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var focusScope = FocusManager.GetFocusScope(this);

            FocusManager.SetFocusedElement(focusScope, InputViewsWrapper.Instance);            
        }

        private void OnLineRemoved(object message) {
            var m = (LinesRemovedMessage)message;

            foreach (var view in views) {
                view.HandleLinesRemove(m);
            }
        }

        private void OnTextRemoved(object message) {
            var m = (TextRemovedMessage)message;

            foreach (var view in views) {
                view.HandleTextRemove(m);
            }
        }

        private void OnTextAdded(object message) {
            var m = (TextAddedMessage)message;

            foreach (var view in views) {
                view.HandleTextInput(m);
            }
        }

        private void OnFoldClicked(object message) {
            var m = (FoldClickedMessage)message;

            linesView.HandleFolding(m);
        }

        #endregion

        #region methods

        private void SetupViews() {
            linesView = new LinesView();
            foldingView = new FoldingView();

            views.Add(linesView);
            views.Add(foldingView);

            Children.Add(linesView);
            Children.Add(foldingView);
        }

        #endregion

    }
}
