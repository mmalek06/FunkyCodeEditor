using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Messaging;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {
        
        #region fields

        private HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        private SelectionView selectionView;

        #endregion

        #region constructor

        public RemoveTextCommand(SelectionView selectionView, TextView view, LocalTextInfo textInfo) : base(view, textInfo) {
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e != null && removalKeys.Contains(e.Key)) {
                return textInfo.GetTextLinesCount() > 0;
            }
            if (view.ActivePosition.Line == 0 && view.ActivePosition.Column == 0) {
                return false;
            }

            return false;
        }

        public override void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var key = e.Key;
            var selectionArea = selectionView.GetCurrentSelectionArea();
            var prevPosition = view.ActivePosition;
            int linesCountBeforeRemove = textInfo.GetTextLinesCount();

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                view.RemoveText(key);
            } else {
                view.RemoveText(selectionArea);
            }
            if (textInfo.GetTextLinesCount() < linesCountBeforeRemove) {
                Postbox.Send(new LineRemovedMessage {
                    Key = key,
                    Position = prevPosition,
                    LineLength = textInfo.GetTextLineLength(prevPosition.Line)
                });
            }

            Postbox.Send(new TextRemovedMessage {
                Key = e.Key,
                Position = view.ActivePosition
            });

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

    }
}
