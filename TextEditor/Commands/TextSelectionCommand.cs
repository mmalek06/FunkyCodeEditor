using System;
using System.Windows.Input;
using TextEditor.DataStructures;
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
            var args = parameter as KeyEventArgs;

            if (args == null) {
                return false;
            }

            return Keyboard.IsKeyDown(Key.RightShift) && 
                (args.Key == Key.Left || args.Key == Key.Right || 
                 args.Key == Key.Up || args.Key == Key.Down || 
                 args.Key == Key.Home || args.Key == Key.End ||
                 args.Key == Key.PageUp || args.Key == Key.PageDown);
        }

        public void Execute(object parameter) {
            var args = (KeyEventArgs)parameter;
            var area = selectionView.GetCurrentSelectionArea();
            TextPosition endingPosition = new TextPosition(0, 0);

            switch (args.Key) {
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

        #endregion

    }
}
