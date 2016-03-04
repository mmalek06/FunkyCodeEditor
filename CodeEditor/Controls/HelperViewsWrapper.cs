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
    internal class HelperViewsWrapper : StackPanel, IMessageReceiver {

        #region fields

        private List<HelperViewBase> views;
        private LinesView linesView;
        private FoldingView foldingView;

        #endregion

        #region constructor

        public HelperViewsWrapper() : base() {
            views = new List<HelperViewBase>();
            Orientation = Orientation.Horizontal;

            Postbox.Subscribe(this);
        }

        #endregion

        #region public methods

        public void Receive<TMessage>(TMessage message) {
            if (message is TextAddedMessage) {
                var m = message as TextAddedMessage;

                foreach (var view in views) {
                    view.HandleTextInput(m.Text, m.Position);
                }
            } else if (message is TextRemovedMessage) {
                var m = message as TextRemovedMessage;

                foreach (var view in views) {
                    view.HandleTextRemove(m.Key, m.Position);
                }
            }
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
