using System;
using System.Windows.Input;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Commands {
    internal class CaretMoveCommand : ICommand {

        #region fields

        private Views.TextView.View textView;

        private Views.CaretView.View caretView;

        private LocalTextInfo textInfo;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public CaretMoveCommand(Views.TextView.View textView, Views.CaretView.View caretView, LocalTextInfo info) {
            this.textView = textView;
            this.caretView = caretView;
            textInfo = info;
        }

        #endregion

        #region public methods

        public bool CanExecute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseButtonEventArgs;

            if (keyboardEvent != null) {
                return CanExecuteKeyMove(keyboardEvent);
            } else if (mouseEvent != null) {
                return CanExecuteMouseMove(mouseEvent);
            }

            return false;
        }

        public void Execute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseButtonEventArgs;
            TextPosition newPos = null;

            if (keyboardEvent != null) {
                newPos = caretView.GetNextPosition(keyboardEvent.Key);
            } else if (mouseEvent != null) {
                newPos = mouseEvent.GetPosition(textView).GetDocumentPosition();
            }
            if (newPos == null) {
                return;
            }

            int column = -1, line = -1;

            if (newPos.Column > textInfo.GetTextLineLength(newPos.Line)) {
                column = textInfo.GetTextLineLength(newPos.Line);
            }
            if (newPos.Line >= textInfo.GetTextLinesCount()) {
                line = textInfo.GetTextLinesCount() - 1;
            }

            newPos = new TextPosition(column > -1 ? column : newPos.Column, line > -1 ? line : newPos.Line);

            caretView.MoveCursor(newPos);
        }

        #endregion

        #region methods

        private bool CanExecuteKeyMove(KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.RightShift)) {
                return false;
            }
            if (!caretView.StepKeys.Contains(e.Key) && !caretView.JumpKeys.Contains(e.Key)) {
                return false;
            }

            var nextPosition = caretView.GetNextPosition(e.Key);

            if (nextPosition.Line < 0 || nextPosition.Line >= textInfo.GetTextLinesCount()) {
                return false;
            }
            if (nextPosition.Column < 0) {
                return false;
            }

            return true;
        }

        private bool CanExecuteMouseMove(MouseButtonEventArgs mouseEvent) {
            var docPosition = mouseEvent.GetPosition(textView).GetDocumentPosition();

            if (docPosition.Line < 0 || docPosition.Line >= textInfo.GetTextLinesCount()) {
                return false;
            }
            if (docPosition.Column < 0) {
                return false;
            }

            return true;
        }

        #endregion

    }
}
