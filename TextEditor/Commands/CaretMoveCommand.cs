using System;
using System.Windows.Input;
using TextEditor.Views.CaretView;
using TextEditor.Views.TextView;

namespace TextEditor.Commands {
    internal class CaretMoveCommand : ICommand {

        #region fields

        private Views.TextView.View textView;

        private Views.CaretView.View caretView;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public CaretMoveCommand(Views.TextView.View textView, Views.CaretView.View caretView) {
            this.textView = textView;
            this.caretView = caretView;
        }

        #endregion

        #region public methods

        public bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e == null) {
                return false;
            }
            if (!caretView.IsMoveRequested(e)) {
                return false;
            }

            var nextPosition = caretView.GetNextPosition(e.Key);
            
            if (nextPosition.Line < 0 || nextPosition.Line >= textView.GetTextLinesCount()) {
                return false;
            }
            if (nextPosition.Column < 0) {
                return false;
            }

            return true;
        }

        public void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var newPos = caretView.GetNextPosition(e.Key);

            if (newPos.Column > textView.GetTextLineLength(newPos.Line)) {
                newPos.Column = textView.GetTextLineLength(newPos.Line);
            }
            if (newPos.Line >= textView.GetTextLinesCount()) {
                newPos.Line = textView.GetTextLinesCount() - 1;
            }

            caretView.MoveCursor(newPos);
        }

        #endregion

    }
}
