using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Commands;
using CodeEditor.Configuration;
using CodeEditor.Core.Controls;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalViewBase = CodeEditor.Views.BaseClasses.InputViewBase;

namespace CodeEditor.Controls {
    internal class InputViewsWrapper : StackablePanel {

        #region fields

        private InputPanel master;
        private List<LocalViewBase> views;
        private TextView textView;
        private CaretView caretView;
        private SelectionView selectionView;

        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        protected override int VisualChildrenCount => views.Count;

        protected override FrameworkElement MeasurementElement => textView;

        #endregion

        #region constructor

        public InputViewsWrapper(InputPanel parent) : base() {
            master = parent;
            views = new List<LocalViewBase>();

            Postbox.Instance.For(typeof(FoldClickedMessage)).Invoke(OnFoldClicked);
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) {
            drawingContext.DrawRectangle(EditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (textView == null && selectionView == null && caretView == null) {
                SetupViews();
                InitEvents();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            var textViewInfo = TextView.TextViewInfo.GetInstance(textView);
            var removeTextCmd = new RemoveTextCommand(textView, selectionView, textViewInfo);
            var caretMoveCmd = new CaretMoveCommand(caretView, textViewInfo);
            var selectionCmd = new TextSelectionCommand(textViewInfo, selectionView, caretView);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (removeTextCmd.CanExecute(e)) {
                ExecuteTextCommand(removeTextCmd, new UndoRemoveTextCommand(textViewInfo), e);
                deselectionCmd.Execute();
            } else if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
                deselectionCmd.Execute();
            } else if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e) {
            var textViewInfo = TextView.TextViewInfo.GetInstance(textView);
            var enterTextCmd = new EnterTextCommand(textView, selectionView, textViewInfo);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (enterTextCmd.CanExecute(e)) {
                ExecuteTextCommand(enterTextCmd, new UndoEnterTextCommand(textViewInfo), e);
                deselectionCmd.Execute();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var textViewInfo = TextView.TextViewInfo.GetInstance(textView);
            var caretMoveCmd = new CaretMoveCommand(caretView, textViewInfo);

            if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
            }

            selectionView.HandleMouseDown(e);
            textView.HandleMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            var textViewInfo = TextView.TextViewInfo.GetInstance(textView);
            var selectionCmd = new TextSelectionCommand(textViewInfo, selectionView, caretView);

            if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            var textViewInfo = TextView.TextViewInfo.GetInstance(textView);
            var selectionCmd = new TextSelectionCommand(textViewInfo, selectionView, caretView);
            bool canExecuteSelection = selectionCmd.CanExecute(e);
            
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

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => views[index];

        private void SetupViews() {
            textView = new TextView();
            selectionView = new SelectionView(TextView.TextViewInfo.GetInstance(textView));
            caretView = new CaretView();

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                view.Margin = new Thickness(2, 0, 0, 0);
                view.HorizontalAlignment = HorizontalAlignment.Left;
                view.VerticalAlignment = VerticalAlignment.Top;

                views.Add(view);
                Children.Add(view);
            }
        }

        private void InitEvents() {
            GotFocus += textView.HandleGotFocus;

            // custom editor events
            textView.TextChanged += caretView.HandleTextChange;
            caretView.CaretMoved += textView.HandleCaretMove;
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
