using System;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Extensions;
using CodeEditor.Enums;
using CodeEditor.DataStructures;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class CaretMoveCommand : ICommand {

        #region fields

        private readonly CaretView caretView;

        private readonly ITextViewReadonly textViewReader;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public CaretMoveCommand(CaretView caretView, ITextViewReadonly textViewReader) {
            this.caretView = caretView;
            this.textViewReader = textViewReader;
        }

        #endregion

        #region public methods

        public bool CanExecute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseButtonEventArgs;

            if (keyboardEvent != null) {
                return CanExecuteKeyMove(keyboardEvent);
            }

            return mouseEvent != null && CanExecuteMouseMove(mouseEvent);
        }

        public void Execute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseButtonEventArgs;
            TextPosition newPos = null;

            if (keyboardEvent != null) {
                newPos = caretView.GetNextPosition(keyboardEvent.Key);
                keyboardEvent.Handled = true;
            } else if (mouseEvent != null) {
                newPos = mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize());
            }
            if (newPos == null) {
                return;
            }

            int column = -1, line = -1;

            if (newPos.Column > textViewReader.GetLineLength(newPos.Line)) {
                column = textViewReader.GetLineLength(newPos.Line);
            }
            if (newPos.Line >= textViewReader.LinesCount) {
                line = textViewReader.LinesCount - 1;
            }

            var moveDir = GetMoveDirection(newPos, caretView.CaretPosition);
            newPos = new TextPosition(column > -1 ? column : newPos.Column, line > -1 ? line : newPos.Line);
            newPos = textViewReader.AdjustStep(newPos, moveDir);

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

            if (nextPosition.Line < 0 || nextPosition.Line >= textViewReader.LinesCount) {
                return false;
            }

            return nextPosition.Column >= 0;
        }

        private bool CanExecuteMouseMove(MouseEventArgs mouseEvent) {
            var docPosition = mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize());

            if (docPosition.Line < 0 || docPosition.Line >= textViewReader.LinesCount) {
                return false;
            }

            return docPosition.Column >= 0;
        }

        private CaretMoveDirection GetMoveDirection(TextPosition newPos, TextPosition activePosition) {
            if (newPos != null && newPos.Line == activePosition.Line) {
                return newPos.Column > activePosition.Column ? CaretMoveDirection.RIGHT : CaretMoveDirection.LEFT;
            }

            return newPos != null && newPos.Line > activePosition.Line ? CaretMoveDirection.BOTTOM : CaretMoveDirection.TOP;
        }

        #endregion

    }
}
