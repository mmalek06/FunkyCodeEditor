using System;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Commands {
    internal class TextSelectionCommand : ICommand {

        #region fields

        private TextView textView;

        private CaretView caretView;

        private SelectionView selectionView;

        private LocalTextInfo textInfo;
        
        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextSelectionCommand(
            TextView textView, 
            SelectionView selectionView, 
            CaretView caretView,
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

                keyboardEvent.Handled = true;
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
            TextPosition endingPosition = null;

            switch (keyboardEvent.Key) {
                case Key.Left:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column - 1, line: textView.ActivePosition.Line);
                    break;
                case Key.Home:
                    endingPosition = new TextPosition(column: 0, line: textView.ActivePosition.Line);
                    break;
                case Key.Right:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column + 1, line: textView.ActivePosition.Line);
                    break;
                case Key.End:
                    endingPosition = new TextPosition(column: textInfo.GetTextLineLength(textView.ActivePosition.Line), line: textView.ActivePosition.Line);
                    break;
                case Key.Up:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column, line: textView.ActivePosition.Line - 1);
                    break;
                case Key.PageUp:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column, line: textView.ActivePosition.Line - GlobalConstants.PageSize);
                    break;
                case Key.Down:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column, line: textView.ActivePosition.Line + 1);
                    break;
                case Key.PageDown:
                    endingPosition = new TextPosition(column: textView.ActivePosition.Column, line: textView.ActivePosition.Line + GlobalConstants.PageSize);
                    break;
            }
            if (!selectionView.HasSelection()) {
                selectionView.Select(new TextPosition(column: textView.ActivePosition.Column, line: textView.ActivePosition.Line));
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
