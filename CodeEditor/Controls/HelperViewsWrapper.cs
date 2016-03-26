using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

            Postbox.Instance.For(typeof(TextAddedMessage))
                            .Invoke(OnTextAdded)
                            .For(typeof(TextRemovedMessage))
                            .Invoke(OnTextRemoved)
                            .For(typeof(LineRemovedMessage))
                            .Invoke(OnLineRemoved)
                            .For(typeof(FoldClickedMessage))
                            .Invoke(OnFoldClicked);
        }

        #endregion

        #region event handlers

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            
            InitEvents();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(EditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (linesView == null && foldingView == null) {
                SetupViews();
            }
        }

        private void OnLineRemoved(object message) {
            var m = message as LineRemovedMessage;

            foreach (var view in views) {
                view.HandleLineRemove(m.Key, m.Position, m.LineLength);
            }
        }

        private void OnTextRemoved(object message) {
            var m = message as TextRemovedMessage;

            foreach (var view in views) {
                view.HandleTextRemove(m.RemovedText, m.Key, m.Position);
            }
        }

        private void OnTextAdded(object message) {
            var m = message as TextAddedMessage;

            foreach (var view in views) {
                view.HandleTextInput(m.Text, m.Position);
            }
        }

        private void OnFoldClicked(object obj) {
            var m = obj as FoldClickedMessage;

            linesView.HandleFoldRemove(m);
        }

        #endregion

        #region methods

        private void InitEvents() {
            
        }

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
