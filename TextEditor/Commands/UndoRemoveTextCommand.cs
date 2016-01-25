namespace TextEditor.Commands {
    internal class UndoRemoveTextCommand : BaseTextViewCommand {

        public UndoRemoveTextCommand(Views.TextView.View view) : base(view) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }
    }
}
