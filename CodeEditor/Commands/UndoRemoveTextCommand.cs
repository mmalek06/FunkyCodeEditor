﻿using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal class UndoRemoveTextCommand : BaseTextViewCommand {

        public UndoRemoveTextCommand(ITextViewReader textViewReader, ICaretViewReader caretViewReader) : base(textViewReader, caretViewReader) { }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) {
            
        }
    }
}
