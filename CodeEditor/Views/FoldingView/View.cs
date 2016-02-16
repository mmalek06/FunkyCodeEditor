using System.Windows.Input;
using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Views.FoldingView {
    internal class View : ViewBase {

        #region fields

        private LocalTextInfo textInfo;

        private IFoldingAlgorithm foldingAlgorithm;

        #endregion

        #region constructor

        public View(LocalTextInfo textInfo, IFoldingAlgorithm foldingAlgorithm) : base() {
            this.textInfo = textInfo;
            this.foldingAlgorithm = foldingAlgorithm;
        }

        #endregion

        #region event handlers

        public void HandleTextPaste(string text, TextPosition position) {

        }

        public void HandleTextDelete(string text, TextPosition position) {

        }

        public void HandleTextInput(TextCompositionEventArgs e, TextPosition position) {
            char character = e.Text[0];

            if (!foldingAlgorithm.CanRun(e.Text)) {
                return;
            }

            foldingAlgorithm.RecreateFolds(e.Text, position);
        }

        #endregion

        #region methods

        private void RedrawFolds() {

        }

        #endregion

    }
}
