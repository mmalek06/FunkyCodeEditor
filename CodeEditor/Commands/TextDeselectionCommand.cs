using System;
using System.Windows.Input;
using TextEditor.Views.SelectionView;

namespace TextEditor.Commands {
    internal class TextDeselectionCommand : ICommand {

        #region fields

        private View selectionView;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region constructor

        public TextDeselectionCommand(View selectionView) {
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
