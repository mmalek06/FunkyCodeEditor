using System;
using System.Windows.Input;
using TextEditor.DataStructures;
using TextEditor.Views.TextView;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Commands {
    internal class TextSelectionCommand : ICommand {

        #region fields

        private Views.TextView.View textView;

        private Views.SelectionView.View selectionView;

        private LocalTextInfo textInfo;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextSelectionCommand(Views.TextView.View textView, Views.SelectionView.View selectionView, LocalTextInfo textInfo) {
            this.textView = textView;
            this.selectionView = selectionView;
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
            TextPosition startingPosition = new TextPosition {
                Column = textView.ActiveColumnIndex,
                Line = textView.ActiveLineIndex
            };
            TextPosition endingPosition = null;
            int column = 0;

            switch (args.Key) {
                case Key.Left:
                    column = area == null ? textView.ActiveColumnIndex - 1 : area.EndPosition.Column - 1;
                    endingPosition = new TextPosition { Column = column, Line = textView.ActiveLineIndex };
                    break;
                case Key.Home:
                    endingPosition = new TextPosition { Column = 0, Line = textView.ActiveLineIndex };
                    break;
                case Key.Right:
                    column = area == null ? textView.ActiveColumnIndex + 1 : area.EndPosition.Column + 1;
                    endingPosition = new TextPosition { Column = column, Line = textView.ActiveLineIndex };
                    break;
                case Key.End:
                    endingPosition = new TextPosition { Column = textInfo.GetTextLineLength(textView.ActiveLineIndex), Line = textView.ActiveLineIndex };
                    break;
                case Key.Up:
                    break;
                case Key.PageUp:
                    break;
                case Key.Down:
                    break;
                case Key.PageDown:
                    break;
            }

            selectionView.Select(startingPosition, endingPosition);
        }

        #endregion

    }
}
