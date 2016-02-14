using System.Collections.Generic;
using CodeEditor.DataStructures;
using CodeEditor.Views.TextView;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region public methods

        IList<string> GetCollapsedLines(IEnumerable<string> textLines, TextPosition startPosition, TextInfo textInfo);

        #endregion

    }
}
