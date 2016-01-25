using System;
using System.Windows.Input;

namespace TextEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected Views.TextView.View view;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(Views.TextView.View view) {
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
            if (view.ActiveLineIndex >= 0) {
                stateToUpdate.LineCount = view.GetTextLinesCount();
                stateToUpdate.ActiveLineIndex = view.ActiveLineIndex;
                stateToUpdate.ActiveColumnIndex = view.ActiveColumnIndex;
                stateToUpdate.LineStates[view.ActiveLineIndex] = view.GetTextLine(view.ActiveLineIndex);
            }
        }

        #endregion

    }
}
