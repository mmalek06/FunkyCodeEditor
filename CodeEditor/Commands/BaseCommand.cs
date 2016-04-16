using System;
using System.Windows.Input;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected ITextViewRead textViewReader;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(ITextViewRead textViewReader) {
            this.textViewReader = textViewReader;
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
            if (textViewReader.ActivePosition.Line >= 0) {
                stateToUpdate.LineCount = textViewReader.LinesCount;
                stateToUpdate.ActiveLineIndex = textViewReader.ActivePosition.Line;
                stateToUpdate.ActiveColumnIndex = textViewReader.ActivePosition.Column;
                stateToUpdate.LineStates[textViewReader.ActivePosition.Line] = textViewReader.GetLine(textViewReader.ActivePosition.Line);
            }
        }

        #endregion

    }
}
