using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Commands {
    internal class UndoRemoveTextCommand : BaseTextViewCommand {

        public UndoRemoveTextCommand(Views.TextView.View view, LocalTextInfo textInfo) : base(view, textInfo) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }
    }
}
