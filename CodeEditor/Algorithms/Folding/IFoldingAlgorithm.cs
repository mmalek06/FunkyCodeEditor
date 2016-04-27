using System.Collections.Generic;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.Folding {
    internal interface IFoldingAlgorithm {

        #region public methods

        bool CanRun(string text);

        bool IsOpeningTag(string text);

        bool IsCollapseRepresentation(string text);

        string GetOpeningTag();

        string GetClosingTag();

        string GetCollapsibleRepresentation();

        TextPosition DeleteFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        IDictionary<TextPosition, TextPosition> CreateFolds(string text, TextPosition position, IDictionary<TextPosition, TextPosition> foldingPositions);

        IEnumerable<TextPosition> GetRepeatingFolds(IDictionary<TextPosition, TextPosition> folds);

        #endregion

    }
}
