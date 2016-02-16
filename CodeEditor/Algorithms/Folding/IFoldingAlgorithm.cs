using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region properties

        Dictionary<TextPosition, TextPosition> FoldingPositions { get; }

        #endregion

        #region public methods

        bool CanRun(string text);

        void RecreateFolds(string text, TextPosition position);

        void DeleteFolds(string text, TextPosition position);

        #endregion

    }
}
