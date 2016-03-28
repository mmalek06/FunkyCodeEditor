using System;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class TextSelectionCommand : ICommand {

        #region fields

        private TextView.TextViewInfo viewInfo;

        private CaretView caretView;

        private SelectionView selectionView;
        
        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextSelectionCommand(
            TextView.TextViewInfo viewInfo, 
            SelectionView selectionView, 
            CaretView caretView) 
        {
            this.viewInfo = viewInfo;
            this.selectionView = selectionView;
            this.caretView = caretView;
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
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Right && caretView.CaretPosition.Column == viewInfo.GetLineLength(caretView.CaretPosition.Line)) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Up && caretView.CaretPosition.Line == 0) {
                return false;
            }
            if (Keyboard.IsKeyDown(Key.RightShift) && keyboardEvent.Key == Key.Down && caretView.CaretPosition.Line == viewInfo.LinesCount - 1) {
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
            viewInfo.IsInTextRange(mouseEvent.GetPosition(caretView).GetDocumentPosition(TextConfiguration.GetCharSize()));

        private void ExecuteKeyboard(KeyEventArgs keyboardEvent) {
            TextPosition endingPosition = null;

            switch (keyboardEvent.Key) {
                case Key.Left:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column - 1, line: viewInfo.ActivePosition.Line);
                    break;
                case Key.Home:
                    endingPosition = new TextPosition(column: 0, line: viewInfo.ActivePosition.Line);
                    break;
                case Key.Right:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column + 1, line: viewInfo.ActivePosition.Line);
                    break;
                case Key.End:
                    endingPosition = new TextPosition(column: viewInfo.GetLineLength(viewInfo.ActivePosition.Line), line: viewInfo.ActivePosition.Line);
                    break;
                case Key.Up:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column, line: viewInfo.ActivePosition.Line - 1);
                    break;
                case Key.PageUp:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column, line: viewInfo.ActivePosition.Line - GlobalConstants.PageSize);
                    break;
                case Key.Down:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column, line: viewInfo.ActivePosition.Line + 1);
                    break;
                case Key.PageDown:
                    endingPosition = new TextPosition(column: viewInfo.ActivePosition.Column, line: viewInfo.ActivePosition.Line + GlobalConstants.PageSize);
                    break;
            }
            if (!selectionView.HasSelection()) {
                selectionView.Select(new TextPosition(column: viewInfo.ActivePosition.Column, line: viewInfo.ActivePosition.Line));
            }

            selectionView.Select(endingPosition);
            caretView.MoveCursor(endingPosition);
        }

        private void ExecuteMouse(MouseEventArgs mouseEvent) {
            var endPosition = mouseEvent.GetPosition(selectionView).GetDocumentPosition(TextConfiguration.GetCharSize());

            selectionView.Select(endPosition);
            caretView.MoveCursor(endPosition);
        }

        #endregion

    }
}
