using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TextEditor.Commands;
using TextEditor.DataStructures;
using LocalViewBase = TextEditor.Views.ViewBase;

namespace TextEditor.Controls {
    public class TextArea : Control {

        #region fields

        private Editor master;
        private List<LocalViewBase> views;
        private Views.TextView.View textView;
        private Views.CaretView.View caretView;
        private Views.SelectionView.View selectionView;
        
        #endregion

        #region properties

        public TextPosition CaretPosition => caretView.CaretPosition;

        protected override int VisualChildrenCount => views.Count;

        #endregion

        #region constructor

        public TextArea(Editor parent) {
            master = parent;
            views = new List<LocalViewBase>();
            
            SetLayers();
            InitEvents();
        }

        #endregion

        #region event handlers

        protected override void OnRender(DrawingContext drawingContext) =>
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));

        protected override void OnKeyDown(KeyEventArgs e) {
            var removeTextCmd = new RemoveTextCommand(textView);

            if (removeTextCmd.CanExecute(e)) {
                ExecuteTextCommand(removeTextCmd, new UndoRemoveTextCommand(textView), e);
            } else {
                var caretMoveCmd = new CaretMoveCommand(textView, caretView);

                if (caretMoveCmd.CanExecute(e)) {
                    caretMoveCmd.Execute(e);
                }
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e) {
            var enterTextCmd = new EnterTextCommand(textView);

            if (enterTextCmd.CanExecute(e)) {
                ExecuteTextCommand(enterTextCmd, new UndoEnterTextCommand(textView), e);
            }
        }

        #endregion

        #region methods

        internal int GetLinesCount() => textView.GetTextLinesCount();

        protected override Visual GetVisualChild(int index) => views[index];

        private void SetLayers() {
            textView = new Views.TextView.View();
            selectionView = new Views.SelectionView.View();
            caretView = new Views.CaretView.View();

            foreach (var view in new LocalViewBase[] { selectionView, textView, caretView }) {
                views.Add(view);
                AddVisualChild(view);
                AddLogicalChild(view);
            }
        }

        private void InitEvents() {
            // standard control events
            MouseMove += selectionView.HandleMouseMove;
            MouseDown += selectionView.HandleMouseDown;
            MouseDown += textView.HandleMouseDown;
            MouseDown += caretView.HandleMouseDown;
            MouseUp += selectionView.HandleMouseUp;
            GotFocus += textView.HandleGotFocus;

            // custom editor events
            textView.TextChanged += selectionView.HandleTextChange;
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
