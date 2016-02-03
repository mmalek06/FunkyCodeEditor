using System.Windows.Input;
using LocalTextInfo = TextEditor.Views.TextView.TextInfo;

namespace TextEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {

        #region fields

        private TextEditor.Views.SelectionView.View selectionView;

        #endregion

        #region constructor

        public EnterTextCommand(Views.SelectionView.View selectionView, Views.TextView.View view, LocalTextInfo textInfo) : base(view, textInfo) {
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as TextCompositionEventArgs;

            if (e != null) {
                return e.Text != Views.TextView.TextConfiguration.BACKSPACE;
            }

            return false;
        }

        public override void Execute(object parameter) {
            var e = parameter as TextCompositionEventArgs;
            var selectionArea = selectionView.GetCurrentSelectionArea();

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                view.EnterText(e.Text);
            } else {
                view.EnterText(e.Text, selectionArea);
            }

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();
        }

        #endregion

    }
}
