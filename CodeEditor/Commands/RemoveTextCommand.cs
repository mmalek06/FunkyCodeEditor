using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {
        
        #region fields

        private HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        private TextView textView;

        private SelectionView selectionView;

        #endregion

        #region constructor

        public RemoveTextCommand(TextView textView, SelectionView selectionView) : base(textView) {
            this.textView = textView;
            this.selectionView = selectionView;
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

            var area = selectionView.GetCurrentSelectionArea();

            if (e.Key == Key.Delete && textViewReader.ActivePosition.Line == textViewReader.LinesCount && textViewReader.ActivePosition.Column == textViewReader.GetLineLength(textViewReader.ActivePosition.Line) && area == null) { 
                return false;
            }
            if (e.Key == Key.Back && textViewReader.ActivePosition.Line == 0 && textViewReader.ActivePosition.Column == 0) {
                return false;
            }

            if (area != null && area.StartPosition != null && area.EndPosition != null) {
                return area.StartPosition != area.EndPosition;
            }

            return true;
        }

        public override void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var key = e.Key;
            var selectionArea = selectionView.GetCurrentSelectionArea();
            var prevPosition = textViewReader.ActivePosition;
            int linesCountBeforeRemove = textViewReader.LinesCount;
            int removedLinesCount = 0;
            string removedText = string.Empty;

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                removedText = GetRemovedText(key);
                
                if (removedText == string.Empty) {
                    removedLinesCount = 1;
                }

                textView.RemoveText(key);
            } else {
                removedText = string.Join("", textViewReader.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition));
                removedLinesCount = selectionArea.EndPosition.Line - selectionArea.StartPosition.Line;

                textView.RemoveText(selectionArea);
            }
            if (textViewReader.LinesCount < linesCountBeforeRemove) {
                Postbox.Instance.Send(new LinesRemovedMessage {
                    Count = removedLinesCount
                });
            }

            Postbox.Instance.Send(new TextRemovedMessage {
                Key = e.Key,
                Position = textViewReader.ActivePosition,
                RemovedText = removedText
            });

            UpdateCommandState(AfterCommandExecutedState);

            textView.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

        #region methods

        private string GetRemovedText(Key key) {
            if (textViewReader.GetLineLength(textViewReader.ActivePosition.Line) == 0 || (key == Key.Delete && textViewReader.ActivePosition.Column >= textViewReader.GetLine(textViewReader.ActivePosition.Line).Length)) {
                return string.Empty;
            }
            if (key == Key.Delete) {
                return textViewReader.GetCharAt(textViewReader.ActivePosition).ToString();
            } else {
                if (textViewReader.ActivePosition.Column == 0 && textViewReader.ActivePosition.Line > 0) {
                    return string.Empty;
                } else {
                    return textViewReader.GetCharAt(
                        new TextPosition(column: textViewReader.ActivePosition.Column > 0 ? textViewReader.ActivePosition.Column - 1 : 0, line: textViewReader.ActivePosition.Line)).ToString();
                }
            }
        }

        #endregion

    }
}
