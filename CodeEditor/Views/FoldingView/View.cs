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

        public void HandleTextInput(TextCompositionEventArgs e, TextPosition position) => foldingAlgorithm.RecreateFolds(e.Text[0], position);

        #endregion

        #region public methods

        public void MarkFoldingOption(int line) {

        }

        #endregion

        #region methods

        private void RunAnalysis() {

        }

        #endregion

    }
}
