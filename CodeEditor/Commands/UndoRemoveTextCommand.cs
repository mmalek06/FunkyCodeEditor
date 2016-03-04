using CodeEditor.Views.Text;
using LocalTextInfo = CodeEditor.Views.Text.TextInfo;

namespace CodeEditor.Commands {
    internal class UndoRemoveTextCommand : BaseTextViewCommand {

        public UndoRemoveTextCommand(TextView view, LocalTextInfo textInfo) : base(view, textInfo) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }
    }
}
