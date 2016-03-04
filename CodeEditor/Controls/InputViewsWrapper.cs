using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Commands;
using CodeEditor.Configuration;
using CodeEditor.Core.Controls;
using CodeEditor.DataStructures;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;
using LocalViewBase = CodeEditor.Views.InputViewBase;

namespace CodeEditor.Controls {
    internal class InputViewsWrapper : StackablePanel {

        #region fields

        private InputPanel master;
        private List<LocalViewBase> views;
        private LocalTextInfo textInfo;
        private TextView textView;
        private CaretView caretView;
        private SelectionView selectionView;

        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        public LocalTextInfo TextInfo => textInfo;

        protected override int VisualChildrenCount => views.Count;

        #endregion

        #region constructor

        public InputViewsWrapper(InputPanel parent) : base() {
            master = parent;
            views = new List<LocalViewBase>();
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
            var removeTextCmd = new RemoveTextCommand(selectionView, textView, textInfo);
            var caretMoveCmd = new CaretMoveCommand(textView, caretView, textInfo);
            var selectionCmd = new TextSelectionCommand(textView, selectionView, caretView, textInfo);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (removeTextCmd.CanExecute(e)) {
                ExecuteTextCommand(removeTextCmd, new UndoRemoveTextCommand(textView, textInfo), e);
                deselectionCmd.Execute();
            } else if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
                deselectionCmd.Execute();
            } else if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e) {
            var enterTextCmd = new EnterTextCommand(selectionView, textView, textInfo);
            var deselectionCmd = new TextDeselectionCommand(selectionView);

            if (enterTextCmd.CanExecute(e)) {
                ExecuteTextCommand(enterTextCmd, new UndoEnterTextCommand(textView, textInfo), e);
                deselectionCmd.Execute();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            var caretMoveCmd = new CaretMoveCommand(textView, caretView, textInfo);

            if (caretMoveCmd.CanExecute(e)) {
                caretMoveCmd.Execute(e);
            }

            selectionView.HandleMouseDown(e);
            textView.HandleMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            var selectionCmd = new TextSelectionCommand(textView, selectionView, caretView, textInfo);

            if (selectionCmd.CanExecute(e)) {
                selectionCmd.Execute(e);
            }
        }

        #endregion

        #region methods

        protected override Visual GetVisualChild(int index) => views[index];

        private void SetupViews() {
            textView = new TextView();
            textInfo = new LocalTextInfo(textView);
            selectionView = new SelectionView(textInfo);
            caretView = new CaretView(textInfo);

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                view.Margin = new Thickness(2, 0, 0, 0);

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
