using System.Collections.Generic;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region public methods

        bool CanRun(string text);

        bool IsOpeningTag(string text);

        TextPosition DeleteFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        IDictionary<TextPosition, TextPosition> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        #endregion

    }
}
