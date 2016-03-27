using System;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class CaretMoveCommand : ICommand {

        #region fields

        private TextView textView;

        private CaretView caretView;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public CaretMoveCommand(TextView textView, CaretView caretView) {
            this.textView = textView;
            this.caretView = caretView;
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
                keyboardEvent.Handled = true;
            } else if (mouseEvent != null) {
                newPos = mouseEvent.GetPosition(textView).GetDocumentPosition(TextConfiguration.GetCharSize());
            }
            if (newPos == null) {
                return;
            }

            int column = -1, line = -1;

            if (newPos.Column > textView.GetLineLength(newPos.Line)) {
                column = textView.GetLineLength(newPos.Line);
            }
            if (newPos.Line >= textView.GetLinesCount()) {
                line = textView.GetLinesCount() - 1;
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

            if (nextPosition.Line < 0 || nextPosition.Line >= textView.GetLinesCount()) {
                return false;
            }
            if (nextPosition.Column < 0) {
                return false;
            }

            return true;
        }

        private bool CanExecuteMouseMove(MouseButtonEventArgs mouseEvent) {
            var docPosition = mouseEvent.GetPosition(textView).GetDocumentPosition(TextConfiguration.GetCharSize());

            if (docPosition.Line < 0 || docPosition.Line >= textView.GetLinesCount()) {
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
