﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Commands;
using CodeEditor.Configuration;
using CodeEditor.Controls.Base;
using CodeEditor.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalViewBase = CodeEditor.Views.BaseClasses.InputViewBase;

namespace CodeEditor.Controls {
    internal class InputViewsWrapper : StackablePanel {

        #region fields

        private Postbox postbox;

        private InputPanel master;

        private List<LocalViewBase> views;

        private TextView textView;

        private CaretView caretView;

        private SelectionView selectionView;

        #endregion

        #region properties

        protected override int VisualChildrenCount => views.Count;

        protected override FrameworkElement MeasurementElement => textView;

        #endregion

        #region constructor

        public InputViewsWrapper(InputPanel parent) {
            master = parent;
            views = new List<LocalViewBase>();
        }

        #endregion

        #region public methods

        public void SetUpMessaging() {
            postbox = Postbox.InstanceFor(this.GetEditor().GetHashCode());

            postbox.For<FoldClickedMessage>().Invoke(OnFoldClicked)
                   .For<ScrollChangedMessage>().Invoke(OnScrollChanged);
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(SharedEditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (textView == null && selectionView == null && caretView == null) {
                SetupViews();
                InitEvents();
                UpdateConfig();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            var removeTextCmd = new RemoveTextCommand(textView, caretView, selectionView);
            var caretMoveCmd = new CaretMoveCommand(caretView, textView);
            var selectionCmd = new TextSelectionCommand(textView, selectionView, caretView);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (removeTextCmd.CanExecute(e)) {
                ExecuteTextCommand(removeTextCmd, new UndoRemoveTextCommand(textView, caretView), e);
                deselectionCmd.Execute();
            } else if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
                deselectionCmd.Execute();
            } else if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e) {
            var enterTextCmd = new EnterTextCommand(textView, caretView, selectionView);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (enterTextCmd.CanExecute(e)) {
                ExecuteTextCommand(enterTextCmd, new UndoEnterTextCommand(textView, caretView), e);
                deselectionCmd.Execute();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var caretMoveCmd = new CaretMoveCommand(caretView, textView);

            if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
            }

            selectionView.HandleMouseDown(e);
            textView.HandleMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            var selectionCmd = new TextSelectionCommand(textView, selectionView, caretView);

            if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            var selectionCmd = new TextSelectionCommand(textView, selectionView, caretView);
            var canExecuteSelection = selectionCmd.CanExecute(e);
            
            if (canExecuteSelection) {
                selectionCmd.Execute(e);
            }
        }

        private void OnFoldClicked(object message) {
            var m = message as FoldClickedMessage;

            foreach (var view in views) {
                view.HandleTextFolding(m);
            }
        }

        private void OnScrollChanged(object message) =>
            textView.HandleScrolling(message as ScrollChangedMessage);

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => views[index];

        private void SetupViews() {
            var editorCode = this.GetEditor().GetHashCode();

            caretView = new CaretView();
            textView = new TextView(caretView);
            selectionView = new SelectionView(textView);

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                view.Margin = new Thickness(2, 0, 0, 0);
                view.HorizontalAlignment = HorizontalAlignment.Left;
                view.VerticalAlignment = VerticalAlignment.Top;
                
                views.Add(view);
                Children.Add(view);

                view.EditorCode = editorCode;
                view.Postbox = postbox;
            }
        }

        private void UpdateConfig() {
            var editor = master.GetEditor();
            var hash = editor.GetHashCode();
            var config = ConfigManager.GetConfig(hash);

            config.InputControl = this;
        }

        private void InitEvents() {
            GotFocus += textView.HandleGotFocus;
        }
        
        private void ExecuteTextCommand(BaseTextViewCommand doCommand, BaseTextViewCommand undoCommand, EventArgs e) {
            doCommand.Execute(e);
            master.DoCommands.Push(doCommand);
            master.UndoCommands.Push(undoCommand);

            undoCommand.BeforeCommandExecutedState = doCommand.AfterCommandExecutedState;
            undoCommand.AfterCommandExecutedState = doCommand.BeforeCommandExecutedState;
        }

        #endregion

    }
}
