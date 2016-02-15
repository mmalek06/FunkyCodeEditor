using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region properties

        Dictionary<TextPosition, TextPosition> FoldingPositions { get; }

        #endregion

        #region public methods

        void Update(string text, TextPosition position);
        
        #endregion

    }
}
