using System;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Enums;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class CaretMoveCommand : ICommand {

        #region fields

        private CaretView caretView;

        private TextView.TextViewInfo textViewInfo;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public CaretMoveCommand(CaretView caretView, TextView.TextViewInfo textViewInfo) {
            this.caretView = caretView;
            this.textViewInfo = textViewInfo;
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
                newPos = mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize());
            }
            if (newPos == null) {
                return;
            }

            int column = -1, line = -1;

            if (newPos.Column > textViewInfo.GetLineLength(newPos.Line)) {
                column = textViewInfo.GetLineLength(newPos.Line);
            }
            if (newPos.Line >= textViewInfo.LinesCount) {
                line = textViewInfo.LinesCount - 1;
            }

            CaretMoveDirection moveDir = GetMoveDirection(newPos, textViewInfo.ActivePosition);
            newPos = new TextPosition(column > -1 ? column : newPos.Column, line > -1 ? line : newPos.Line);
            newPos = textViewInfo.AdjustStep(newPos, moveDir);

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

            if (nextPosition.Line < 0 || nextPosition.Line >= textViewInfo.LinesCount) {
                return false;
            }
            if (nextPosition.Column < 0) {
                return false;
            }

            return true;
        }

        private bool CanExecuteMouseMove(MouseButtonEventArgs mouseEvent) {
            var docPosition = mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize());

            if (docPosition.Line < 0 || docPosition.Line >= textViewInfo.LinesCount) {
                return false;
            }
            if (docPosition.Column < 0) {
                return false;
            }

            return true;
        }

        private CaretMoveDirection GetMoveDirection(TextPosition newPos, TextPosition activePosition) {
            if (newPos.Line == activePosition.Line) {
                if (newPos.Column > activePosition.Column) {
                    return CaretMoveDirection.RIGHT;
                }

                return CaretMoveDirection.LEFT;
            } else {
                if (newPos.Line > activePosition.Line) {
                    return CaretMoveDirection.BOTTOM;
                }

                return CaretMoveDirection.TOP;
            }
        }

        #endregion

    }
}
