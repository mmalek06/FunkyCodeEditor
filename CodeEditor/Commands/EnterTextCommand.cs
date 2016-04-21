using System.Windows.Input;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class EnterTextCommand : BaseTextViewCommand {

        #region fields

        private TextView textView;

        private CaretView caretView;

        private ISelectionViewReadonly selectionViewReader;

        #endregion

        #region constructor

        public EnterTextCommand(TextView textView, CaretView caretView, ISelectionViewReadonly selectionViewReader) : base(textView, caretView) {
            this.textView = textView;
            this.selectionViewReader = selectionViewReader;
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
            var selectionArea = selectionViewReader.GetCurrentSelectionArea();
            var prevCaretPosition = caretViewReader.CaretPosition;

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
                PrevCaretPosition = prevCaretPosition,
                NewCaretPosition = caretViewReader.CaretPosition
            });
        }

        #endregion

    }
}
