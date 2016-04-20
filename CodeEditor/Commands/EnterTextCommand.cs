using System.Windows.Input;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {
        
        #region fields

        private SelectionView selectionView;

        private TextView textView;

        private CaretView caretView;

        #endregion

        #region constructor

        public EnterTextCommand(TextView textView, SelectionView selectionView, CaretView caretView) : base(textView, caretView) {
            this.textView = textView;
            this.selectionView = selectionView;
            this.caretView = caretView;
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

            caretView.HandleTextChange(e.Text);
            Postbox.Instance.Send(new TextAddedMessage {
                Text = e.Text,
                Position = caretViewReader.CaretPosition
            });
        }

        #endregion

    }
}
