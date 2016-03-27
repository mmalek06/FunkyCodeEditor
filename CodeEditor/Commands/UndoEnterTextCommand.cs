using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class UndoEnterTextCommand : BaseTextViewCommand {

        public UndoEnterTextCommand(TextView view) : base(view) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }

    }
}
