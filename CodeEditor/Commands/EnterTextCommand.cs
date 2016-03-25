using System.Windows.Input;
using CodeEditor.Messaging;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {

        #region fields

        private SelectionView selectionView;

        #endregion

        #region constructor

        public EnterTextCommand(SelectionView selectionView, TextView view, LocalTextInfo textInfo) : base(view, textInfo) {
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
                view.EnterText(e.Text);
            } else {
                view.ReplaceText(e.Text, selectionArea);
            }

            UpdateCommandState(AfterCommandExecutedState);

            view.TriggerTextChanged(e.Text);
            Postbox.Instance.Send(new TextAddedMessage {
                Text = e.Text,
                Position = view.ActivePosition
            });
        }

        #endregion

    }
}
