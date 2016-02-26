using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeEditor.Adorners;
using CodeEditor.Commands;
using CodeEditor.Configuration;
using CodeEditor.Core.Controls;
using CodeEditor.DataStructures;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;
using LocalViewBase = CodeEditor.Views.ViewBase;

namespace CodeEditor.Controls {
    internal class ViewsWrapper : StackablePanel {

        #region fields

        private Editor master;
        private List<LocalViewBase> views;
        private LocalTextInfo textInfo;
        private Views.TextView.View textView;
        private Views.CaretView.View caretView;
        private Views.SelectionView.View selectionView;

        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        public LocalTextInfo TextInfo => textInfo;

        public IEnumerable<ReactiveAdorner> Adorners { get; set; }

        protected override int VisualChildrenCount => views.Count;

        #endregion

        #region constructor

        public ViewsWrapper(Editor parent) : base() {
            master = parent;
            views = new List<LocalViewBase>();
            Width = parent.Width;
            Height = parent.Height;

            SetupViews();
            InitEvents();
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) => 
            drawingContext.DrawRectangle(EditorConfiguration.GetEditorBrush(), null, new Rect(0, 0, Width, Height));

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
                RunAdorners(e);
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
            textView = new Views.TextView.View();
            textInfo = new LocalTextInfo(textView);
            selectionView = new Views.SelectionView.View(textInfo);
            caretView = new Views.CaretView.View(textInfo);

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                view.Margin = new Thickness(EditorConfiguration.GetTextAreaLeftMargin() + 2, 0, 0, 0);
                view.Width = Width - EditorConfiguration.GetTextAreaLeftMargin() - 2;
                view.Height = Height;

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

        private void RunAdorners(TextCompositionEventArgs e) {
            foreach (var adorner in Adorners) {
                adorner.HandleTextInput(e, textView.ActivePosition);
            }
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
