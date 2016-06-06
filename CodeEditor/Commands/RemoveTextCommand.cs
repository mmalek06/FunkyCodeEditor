using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {
        
        #region fields

        private readonly HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        private readonly TextView textView;

        private readonly CaretView caretView;

        private readonly ISelectionViewReadonly selectionViewReader;

        #endregion

        #region constructor

        public RemoveTextCommand(TextView textView, CaretView caretView, ISelectionViewReadonly selectionViewReader) : base(textView, caretView) {
            this.textView = textView;
            this.selectionViewReader = selectionViewReader;
            this.caretView = caretView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e == null) {
                return false;
            }
            if (!removalKeys.Contains(e.Key)) {
                return false;
            }

            var area = selectionViewReader.GetCurrentSelectionArea();

            if (e.Key == Key.Delete && caretViewReader.CaretPosition.Line == textViewReader.LinesCount && caretViewReader.CaretPosition.Column == textViewReader.GetLineLength(caretViewReader.CaretPosition.Line) && area == null) { 
                return false;
            }
            if (e.Key == Key.Back && caretViewReader.CaretPosition.Line == 0 && caretViewReader.CaretPosition.Column == 0) {
                return false;
            }

            if (area != null && area.StartPosition != null && area.EndPosition != null) {
                return area.StartPosition != area.EndPosition;
            }

            return true;
        }

        public override void Execute(object parameter) {
            var e = (KeyEventArgs)parameter;
            var key = e.Key;
            var selectionArea = selectionViewReader.GetCurrentSelectionArea();
            var linesCountBeforeRemove = textViewReader.LinesCount;
            var removedLinesCount = 0;
            var removedText = string.Empty;
            TextPosition positionAfterRemove;

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                removedText = GetRemovedText(key);
                positionAfterRemove = GetPositionAfterKeypress(key, removedText);

                if (removedText == string.Empty) {
                    removedLinesCount = 1;
                }

                textView.RemoveText(key);
            } else {
                removedText = string.Join("", textViewReader.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition));
                removedLinesCount = selectionArea.EndPosition.Line - selectionArea.StartPosition.Line;
                positionAfterRemove = selectionArea.StartPosition;

                textView.RemoveText(selectionArea);
            }
            if (textViewReader.LinesCount < linesCountBeforeRemove) {
                textView.Postbox.Put(new LinesRemovedMessage {
                    Count = removedLinesCount
                });
            }

            textView.Postbox.Put(new TextRemovedMessage {
                Key = e.Key,
                OldCaretPosition = caretViewReader.CaretPosition,
                NewCaretPosition = positionAfterRemove,
                RemovedText = removedText
            });
            caretView.HandleTextRemove(positionAfterRemove);

            UpdateCommandState(AfterCommandExecutedState);

            e.Handled = true;
        }

        #endregion

        #region methods

        private TextPosition GetPositionAfterKeypress(Key key, string removedText) {
            if (key == Key.Back) {
                if (removedText == string.Empty) {
                    return new TextPosition(column: textViewReader.GetLineLength(caretViewReader.CaretPosition.Line - 1), line: caretViewReader.CaretPosition.Line - 1);
                }

                return new TextPosition(column: caretViewReader.CaretPosition.Column - removedText.Length, line: caretViewReader.CaretPosition.Line);
            }

            return caretViewReader.CaretPosition;
        }

        private string GetRemovedText(Key key) {
            if (textViewReader.GetLineLength(caretViewReader.CaretPosition.Line) == 0 || (key == Key.Delete && caretViewReader.CaretPosition.Column >= textViewReader.GetLine(caretViewReader.CaretPosition.Line).Length)) {
                return string.Empty;
            }
            if (key == Key.Delete) {
                return textViewReader.GetCharAt(caretViewReader.CaretPosition).ToString();
            }
            if (caretViewReader.CaretPosition.Column == 0 && caretViewReader.CaretPosition.Line > 0) {
                return string.Empty;
            } 

            var line = textViewReader.GetVisualLine(caretViewReader.CaretPosition.Line);
            var info = line.GetCharInfoAt(caretViewReader.CaretPosition.Column > 0 ? caretViewReader.CaretPosition.Column - 1 : 0);

            if (!info.IsCharacter) {
                return info.Text;
            } 

            return textViewReader.GetCharAt(
                new TextPosition(
                    column: caretViewReader.CaretPosition.Column > 0 ? caretViewReader.CaretPosition.Column - 1 : 0, 
                    line: caretViewReader.CaretPosition.Line)).ToString();
        }

        #endregion

    }
}
