using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Views.FoldingView {
    internal class View : ViewBase {

        #region fields

        private LocalTextInfo textInfo;

        private TextRunProperties runProperties;

        private IFoldingAlgorithm foldingAlgorithm;

        #endregion

        #region constructor

        public View(LocalTextInfo textInfo, IFoldingAlgorithm foldingAlgorithm) : base() {
            this.textInfo = textInfo;
            this.foldingAlgorithm = foldingAlgorithm;
            runProperties = this.CreateGlobalTextRunProperties();
        }

        #endregion

        #region event handlers

        public void HandleTextPaste(string text, TextPosition position) {

        }

        public void HandleTextDelete(string text, TextPosition position) {

        }

        public void HandleTextInput(TextCompositionEventArgs e, TextPosition position) {
            char character = e.Text[0];

            if (!foldingAlgorithm.CanHandle(e.Text)) {
                return;
            }

            foldingAlgorithm.RecreateFolds(e.Text, position);
        }

        #endregion

        #region public methods



        #endregion

        #region methods



        #endregion

    }
}
