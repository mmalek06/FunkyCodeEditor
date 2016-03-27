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

        private SelectionView selectionView;

        #endregion

        #region constructor

        public RemoveTextCommand(SelectionView selectionView, TextView view) : base(view) {
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

            if (e.Key == Key.Delete && view.ActivePosition.Line == view.GetLinesCount() - 1 && view.ActivePosition.Column == view.GetLineLength(view.ActivePosition.Line) && area == null) { 
                return false;
            }
            if (e.Key == Key.Back && view.ActivePosition.Line == 0 && view.ActivePosition.Column == 0) {
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
            var prevPosition = view.ActivePosition;
            int linesCountBeforeRemove = view.GetLinesCount();
            int removedLinesCount = 0;
            string removedText = string.Empty;

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                removedText = GetRemovedText(key);
                
                if (removedText == string.Empty) {
                    removedLinesCount = 1;
                }

                view.RemoveText(key);
            } else {
                removedText = string.Join("", view.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition));
                removedLinesCount = selectionArea.EndPosition.Line - selectionArea.StartPosition.Line;

                view.RemoveText(selectionArea);
            }
            if (view.GetLinesCount() < linesCountBeforeRemove) {
                Postbox.Instance.Send(new LinesRemovedMessage {
                    Count = removedLinesCount
                });
            }

            Postbox.Instance.Send(new TextRemovedMessage {
                Key = e.Key,
                Position = view.ActivePosition,
                RemovedText = removedText
            });

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

        #region methods

        private string GetRemovedText(Key key) {
            if (view.GetLineLength(view.ActivePosition.Line) == 0 || (key == Key.Delete && view.ActivePosition.Column >= view.GetLine(view.ActivePosition.Line).Length)) {
                return string.Empty;
            }
            if (key == Key.Delete) {
                return view.GetCharAt(view.ActivePosition).ToString();
            } else {
                return view.GetCharAt(
                    new TextPosition(column: view.ActivePosition.Column > 0 ? view.ActivePosition.Column - 1 : 0, line: view.ActivePosition.Line)).ToString();
            }
        }

        #endregion

    }
}
