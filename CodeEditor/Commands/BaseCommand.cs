using System;
using System.Windows.Input;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected ITextViewReadonly textViewReader;

        protected ICaretViewReadonly caretViewReader;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(ITextViewReadonly textViewReader, ICaretViewReadonly caretViewReader) {
            this.textViewReader = textViewReader;
            this.caretViewReader = caretViewReader;
            BeforeCommandExecutedState = new ViewState();
            AfterCommandExecutedState = new ViewState();
        }

        #endregion
        
        #region public methods

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        #endregion

        #region methods

        protected void UpdateCommandState(ViewState stateToUpdate) {
            if (caretViewReader.CaretPosition.Line >= 0) {
                stateToUpdate.LineCount = textViewReader.LinesCount;
                stateToUpdate.ActiveLineIndex = caretViewReader.CaretPosition.Line;
                stateToUpdate.ActiveColumnIndex = caretViewReader.CaretPosition.Column;
                stateToUpdate.LineStates[caretViewReader.CaretPosition.Line] = textViewReader.GetLine(caretViewReader.CaretPosition.Line);
            }
        }

        #endregion

    }
}
