using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Commands;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;
using LocalViewBase = CodeEditor.Views.ViewBase;

namespace CodeEditor.Controls {
    public class TextArea : Control {

        #region fields

        private Editor master;
        private List<LocalViewBase> views;
        private LocalTextInfo textInfo;
        private Views.TextView.View textView;
        private Views.CaretView.View caretView;
        private Views.SelectionView.View selectionView;
        private Views.FoldingView.View foldingView;

        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        internal LocalTextInfo TextInfo => textInfo;

        protected override int VisualChildrenCount => views.Count;

        #endregion

        #region constructor

        public TextArea(Editor parent) {
            master = parent;
            views = new List<LocalViewBase>();
            
            SetViews();
            InitEvents();
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));

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

        private void SetViews() {
            textView = new Views.TextView.View();
            textInfo = new LocalTextInfo(textView);
            selectionView = new Views.SelectionView.View(textInfo);
            caretView = new Views.CaretView.View(textInfo);
            foldingView = new Views.FoldingView.View(textInfo, EditorConfiguration.GetFoldingAlgorithm());

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView, foldingView }) {
                views.Add(view);
                AddVisualChild(view);
                AddLogicalChild(view);
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
