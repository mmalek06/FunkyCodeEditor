using System.Collections.Generic;
using System.Windows.Input;

namespace TextEditor.Commands {
    internal class RemoveTextCommand : BaseTextViewCommand {

        #region fields

        private HashSet<Key> removalKeys = new HashSet<Key> { Key.Delete, Key.Back };

        #endregion

        #region constructor

        public RemoveTextCommand(Views.TextView.View view) : base(view) { }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as KeyEventArgs;

            if (e != null && removalKeys.Contains(e.Key)) {
                return view.GetTextLinesCount() > 0;
            }

            return false;
        }

        public override void Execute(object parameter) {
            var e = parameter as KeyEventArgs;
            var key = e.Key;

            UpdateCommandState(BeforeCommandExecutedState);

            view.RemoveText(key);

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();

            e.Handled = true;
        }

        #endregion

    }
}
