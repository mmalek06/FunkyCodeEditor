using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region properties

        Dictionary<TextPosition, TextPosition> FoldingPositions { get; }

        #endregion

        #region public methods

        void RecreateFolds(char bracket, TextPosition position);

        void DeleteFolds(char bracket, TextPosition position);

        #endregion

    }
}
