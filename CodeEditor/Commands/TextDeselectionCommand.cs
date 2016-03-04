using System;
using System.Windows.Input;
using CodeEditor.Views.Selection;

namespace CodeEditor.Commands {
    internal class TextDeselectionCommand : ICommand {

        #region fields

        private SelectionView selectionView;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextDeselectionCommand(SelectionView selectionView) {
            this.selectionView = selectionView;
        }

        #endregion

        #region public methods

        public void Execute() {
            selectionView.Deselect();
        }

        public void Execute(object parameter) {
            Execute();
        }

        public bool CanExecute(object parameter) => true;

        #endregion

    }
}
