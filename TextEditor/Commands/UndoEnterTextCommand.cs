namespace TextEditor.Commands {
    internal class UndoEnterTextCommand : BaseTextViewCommand {

        public UndoEnterTextCommand(Views.TextView.View view) : base(view) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }

    }
}
