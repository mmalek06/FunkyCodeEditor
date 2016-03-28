using System;
using System.Windows.Input;
using CodeEditor.Views.Text;

namespace CodeEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected TextView.TextViewInfo viewInfo;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(TextView.TextViewInfo viewInfo) {
            this.viewInfo = viewInfo;
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
            if (viewInfo.ActivePosition.Line >= 0) {
                stateToUpdate.LineCount = viewInfo.LinesCount;
                stateToUpdate.ActiveLineIndex = viewInfo.ActivePosition.Line;
                stateToUpdate.ActiveColumnIndex = viewInfo.ActivePosition.Column;
                stateToUpdate.LineStates[viewInfo.ActivePosition.Line] = viewInfo.GetLine(viewInfo.ActivePosition.Line);
            }
        }

        #endregion

    }
}
