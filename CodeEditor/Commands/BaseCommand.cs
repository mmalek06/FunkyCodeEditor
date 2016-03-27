using System;
using System.Windows.Input;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected TextView view;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(TextView view) {
            this.view = view;
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
            if (view.ActivePosition.Line >= 0) {
                stateToUpdate.LineCount = view.LinesCount;
                stateToUpdate.ActiveLineIndex = view.ActivePosition.Line;
                stateToUpdate.ActiveColumnIndex = view.ActivePosition.Column;
                stateToUpdate.LineStates[view.ActivePosition.Line] = view.GetLine(view.ActivePosition.Line);
            }
        }

        #endregion

    }
}
