using System.Collections.Generic;
using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region public methods

        bool CanRun(string text);

        void GetFoldsToDelete(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        IEnumerable<KeyValuePair<TextPosition, TextPosition>> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        #endregion

    }
}
