using System;
using System.Windows.Input;
using TextEditor.Views.TextView;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Commands {
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
            var e = parameter as KeyEventArgs;

            if (e == null || Keyboard.IsKeyDown(Key.RightShift)) {
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

        public void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var newPos = caretView.GetNextPosition(e.Key);

            if (newPos.Column > textInfo.GetTextLineLength(newPos.Line)) {
                newPos.Column = textInfo.GetTextLineLength(newPos.Line);
            }
            if (newPos.Line >= textInfo.GetTextLinesCount()) {
                newPos.Line = textInfo.GetTextLinesCount() - 1;
            }

            caretView.MoveCursor(newPos);
        }

        #endregion

    }
}
