using System;
using System.Windows.Input;
using TextEditor.DataStructures;
using TextEditor.Extensions;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Commands {
    internal class TextSelectionCommand : ICommand {

        #region fields

        private Views.TextView.View textView;

        private Views.CaretView.View caretView;

        private Views.SelectionView.View selectionView;

        private LocalTextInfo textInfo;
        
        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextSelectionCommand(
            Views.TextView.View textView, 
            Views.SelectionView.View selectionView, 
            Views.CaretView.View caretView,
            LocalTextInfo textInfo) 
        {
            this.textView = textView;
            this.selectionView = selectionView;
            this.caretView = caretView;
            this.textInfo = textInfo;
        }

        #endregion

        #region public methods

        public bool CanExecute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseEventArgs;

            if (keyboardEvent != null) {
                return CanExecuteKeyboard(keyboardEvent);
            } else if (mouseEvent != null) {
                return CanExecuteMouse(mouseEvent);
            }

            return false;
        }

        public void Execute(object parameter) {
            var keyboardEvent = parameter as KeyEventArgs;
            var mouseEvent = parameter as MouseEventArgs;
            
            if (keyboardEvent != null) {
                ExecuteKeyboard(keyboardEvent);
            } else if (mouseEvent != null) {
                ExecuteMouse(mouseEvent);
            }
        }

        #endregion

        #region methods

        private bool CanExecuteKeyboard(KeyEventArgs keyboardEvent) {
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Left && caretView.CaretPosition.Column == 0) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Right && caretView.CaretPosition.Column == textInfo.GetTextLineLength(caretView.CaretPosition.Line)) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Up && caretView.CaretPosition.Line == 0) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Down && caretView.CaretPosition.Line == textInfo.GetTextLinesCount() - 1) {
                return false;
            }

            return Keyboard.IsKeyDown(Key.RightShift) &&
                (keyboardEvent.Key == Key.Left || keyboardEvent.Key == Key.Right ||
                 keyboardEvent.Key == Key.Up || keyboardEvent.Key == Key.Down ||
                 keyboardEvent.Key == Key.Home || keyboardEvent.Key == Key.End ||
                 keyboardEvent.Key == Key.PageUp || keyboardEvent.Key == Key.PageDown);
        }

        private bool CanExecuteMouse(MouseEventArgs mouseEvent) => 
            mouseEvent.LeftButton == MouseButtonState.Pressed && 
            textInfo.IsInTextRange(mouseEvent.GetPosition(textView).GetDocumentPosition());

        private void ExecuteKeyboard(KeyEventArgs keyboardEvent) {
            var area = selectionView.GetCurrentSelectionArea();
            TextPosition endingPosition = new TextPosition(0, 0);

#if DEBUG
            Console.WriteLine("Selection command executed");
#endif

            switch (keyboardEvent.Key) {
                case Key.Left:
                    endingPosition.Column = area == null ? textView.ActivePosition.Column - 1 : area.EndPosition.Column - 1;
                    endingPosition.Line = textView.ActivePosition.Line;
                    break;
                case Key.Home:
                    endingPosition.Column = 0;
                    endingPosition.Line = textView.ActivePosition.Line;
                    break;
                case Key.Right:
                    endingPosition.Column = area == null ? textView.ActivePosition.Column + 1 : area.EndPosition.Column + 1;
                    endingPosition.Line = textView.ActivePosition.Line;
                    break;
                case Key.End:
                    endingPosition.Column = textInfo.GetTextLineLength(textView.ActivePosition.Line);
                    endingPosition.Line = textView.ActivePosition.Line;
                    break;
                case Key.Up:
                    endingPosition.Column = area == null ? textView.ActivePosition.Column : area.EndPosition.Column;
                    endingPosition.Line = area == null ? textView.ActivePosition.Line - 1 : area.EndPosition.Line - 1;
                    break;
                case Key.PageUp:
                    break;
                case Key.Down:
                    endingPosition.Column = area == null ? textView.ActivePosition.Column : area.EndPosition.Column;
                    endingPosition.Line = area == null ? textView.ActivePosition.Line + 1 : area.EndPosition.Line + 1;
                    break;
                case Key.PageDown:
                    break;
            }
            if (area == null) {
                selectionView.Select(new TextPosition {
                    Column = textView.ActivePosition.Column,
                    Line = textView.ActivePosition.Line
                });
            }

            selectionView.Select(endingPosition);
            caretView.MoveCursor(endingPosition);
        }

        private void ExecuteMouse(MouseEventArgs mouseEvent) {
            var endPosition = mouseEvent.GetPosition(selectionView).GetDocumentPosition();

            selectionView.Select(endPosition);
            caretView.MoveCursor(endPosition);
        }

        #endregion

    }
}
