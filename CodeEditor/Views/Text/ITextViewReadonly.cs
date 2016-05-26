using System.Collections.Generic;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Enums;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    internal interface ITextViewReadonly {

        #region properties

        int LinesCount { get; }

        #endregion

        #region public methods

        IReadOnlyList<string> GetScreenLines();

        IReadOnlyList<string> GetActualLines();

        IReadOnlyList<VisualTextLine> GetVisualLines();

        TextPosition AdjustStep(TextPosition newPosition, CaretMoveDirection moveDirection);

        char GetCharAt(TextPosition position);

        int GetLineLength(int index);

        string GetLine(int index);

        VisualTextLine GetVisualLine(int index);

        IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition);

        bool IsInTextRange(TextPosition position);

        #endregion

    }
}
