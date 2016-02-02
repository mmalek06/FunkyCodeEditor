using System.Collections.Generic;
using System.Windows.Input;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {
        
        #region fields

        private HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        private Views.SelectionView.View selectionView;

        #endregion

        #region constructor

        public RemoveTextCommand(Views.SelectionView.View selectionView, Views.TextView.View view, LocalTextInfo textInfo) : base(view, textInfo) {
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e != null && removalKeys.Contains(e.Key)) {
                return textInfo.GetTextLinesCount() > 0;
            }

            return false;
        }

        public override void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var key = e.Key;

            UpdateCommandState(BeforeCommandExecutedState);

            var selectionArea = selectionView.GetCurrentSelectionArea();

            if (selectionArea == null) {
                view.RemoveText(key);
            } else {
                view.RemoveText(selectionArea);
            }

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

    }
}
