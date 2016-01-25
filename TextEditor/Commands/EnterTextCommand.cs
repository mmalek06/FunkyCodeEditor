using System.Windows.Input;

namespace TextEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {

        #region constructor

        public EnterTextCommand(Views.TextView.View view) : base(view) { }

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

            UpdateCommandState(BeforeCommandExecutedState);

            view.EnterText(e.Text);

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged();
        }

        #endregion

    }
}
