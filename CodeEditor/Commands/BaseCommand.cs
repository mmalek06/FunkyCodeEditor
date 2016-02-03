using System;
using System.Windows.Input;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Commands {
    internal abstract class BaseTextViewCommand : ICommand {

        #region fields

        protected Views.TextView.View view;

        protected LocalTextInfo textInfo;

        #endregion

        #region properties

        public ViewState BeforeCommandExecutedState { get; set; }

        public ViewState AfterCommandExecutedState { get; set; }

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public BaseTextViewCommand(Views.TextView.View view, LocalTextInfo info) {
            this.view = view;
            textInfo = info;
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
                stateToUpdate.LineCount = textInfo.GetTextLinesCount();
                stateToUpdate.ActiveLineIndex = view.ActivePosition.Line;
                stateToUpdate.ActiveColumnIndex = view.ActivePosition.Column;
                stateToUpdate.LineStates[view.ActivePosition.Line] = textInfo.GetTextLine(view.ActivePosition.Line);
            }
        }

        #endregion

    }
}
