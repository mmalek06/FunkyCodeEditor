using System.Windows.Input;
using CodeEditor.Messaging;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {

        #region fields

        private SelectionView selectionView;

        private TextView textView;

        #endregion

        #region constructor

        public EnterTextCommand(TextView textView, SelectionView selectionView) : base(textView) {
            this.textView = textView;
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public override bool CanExecute(object parameter) {
            var e = parameter as TextCompositionEventArgs;

            if (e != null) {
                return e.Text != TextProperties.Properties.BACKSPACE;
            }

            return false;
        }

        public override void Execute(object parameter) {
            var e = parameter as TextCompositionEventArgs;
            var selectionArea = selectionView.GetCurrentSelectionArea();

            UpdateCommandState(BeforeCommandExecutedState);

            if (selectionArea == null) {
                textView.EnterText(e.Text);
            } else {
                textView.ReplaceText(e.Text, selectionArea);
            }

            UpdateCommandState(AfterCommandExecutedState);

            textView.TriggerTextChanged(e.Text);
            Postbox.Instance.Send(new TextAddedMessage {
                Text = e.Text,
                Position = textViewReader.ActivePosition
            });
        }

        #endregion

    }
}
