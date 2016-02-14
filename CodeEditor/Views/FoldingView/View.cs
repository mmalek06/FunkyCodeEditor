using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using LocalTextInfo = CodeEditor.Views.TextView.TextInfo;

namespace CodeEditor.Views.FoldingView {
    internal class View : ViewBase {

        #region fields

        private LocalTextInfo textInfo;

        private TextRunProperties runProperties;

        //private Dictionary<int, FoldingInfo> folds;

        #endregion

        #region constructor

        public View(LocalTextInfo textInfo) : base() {
            this.textInfo = textInfo;
            runProperties = this.CreateGlobalTextRunProperties();
            //folds = new Dictionary<int, FoldingInfo>();
        }

        #endregion

        #region event handlers

        public void HandleTextEntered(TextCompositionEventArgs e, TextPosition position) {
            if (e.Text == "}") {
                //var matchPosition = (new BracesStrategy().GetMatch(textInfo, position, e.Text));
            }
        }

        #endregion

        #region public methods



        #endregion

        #region methods



        #endregion

    }
}
