using System.Collections.Generic;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.Selection {
    internal class TextSelector {

        #region fields

        private ITextViewRead textViewReader;

        private SelectionView parent;
        
        #endregion

        #region constructor

        public TextSelector(ITextViewRead textViewReader, SelectionView parent) {
            this.textViewReader = textViewReader;
            this.parent = parent;
        }

        #endregion

        #region public methods

        public TextPosition StandardSelection(MouseEventArgs mouseEvent) => mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());

        public SelectionInfo WordSelection(KeyEventArgs keyboardEvent) {
            var activeLine = textViewReader.GetLine(textViewReader.ActivePosition.Line);

            return null;
        }

        public SelectionInfo WordSelection(MouseButtonEventArgs mouseEvent) {
            var clickPosition = mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());
            var activeLine = textViewReader.GetVisualLine(clickPosition.Line);
            int lineLength = textViewReader.GetLineLength(clickPosition.Line);
            int[] selectionRange;

            if (!activeLine.GetCharInfoAt(clickPosition.Column).IsCharacter) {
                selectionRange = GetCollapseSelectionRange(clickPosition, activeLine, lineLength);
            } else {
                selectionRange = GetStandardWordSelectionRange(clickPosition, activeLine, lineLength);
            }

            return new SelectionInfo {
                StartPosition = new TextPosition(column: selectionRange[0], line: clickPosition.Line),
                EndPosition = new TextPosition(column: selectionRange[1], line: clickPosition.Line),
                CursorPosition = new TextPosition(column: selectionRange[1], line: clickPosition.Line)
            };
        }

        public SelectionInfo LineSelection(MouseButtonEventArgs mouseEvent) {
            var clickPosition = mouseEvent.GetPosition(parent).GetDocumentPosition(TextConfiguration.GetCharSize());
            var startPosition = new TextPosition(column: 0, line: clickPosition.Line);
            var endPosition = new TextPosition(column: textViewReader.GetLineLength(clickPosition.Line), line: clickPosition.Line);
            int cursorColumn = clickPosition.Line + 1 < textViewReader.LinesCount ? 0 : textViewReader.GetLineLength(clickPosition.Line);
            int cursorLine = clickPosition.Line + 1 < textViewReader.LinesCount ? clickPosition.Line + 1 : clickPosition.Line;

            return new SelectionInfo {
                StartPosition = startPosition,
                EndPosition = endPosition,
                CursorPosition = new TextPosition(column: cursorColumn, line: cursorLine)
            };
        }

        public TextPosition GetSelectionPosition(KeyEventArgs keyboardEvent) {
            TextPosition endingPosition = null;

            switch (keyboardEvent.Key) {
                case Key.Left:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column - 1, line: textViewReader.ActivePosition.Line);
                    break;
                case Key.Home:
                    endingPosition = new TextPosition(column: 0, line: textViewReader.ActivePosition.Line);
                    break;
                case Key.Right:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column + 1, line: textViewReader.ActivePosition.Line);
                    break;
                case Key.End:
                    endingPosition = new TextPosition(column: textViewReader.GetLineLength(textViewReader.ActivePosition.Line), line: textViewReader.ActivePosition.Line);
                    break;
                case Key.Up:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column, line: textViewReader.ActivePosition.Line - 1);
                    break;
                case Key.PageUp:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column, line: textViewReader.ActivePosition.Line - GlobalConstants.PageSize);
                    break;
                case Key.Down:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column, line: textViewReader.ActivePosition.Line + 1);
                    break;
                case Key.PageDown:
                    endingPosition = new TextPosition(column: textViewReader.ActivePosition.Column, line: textViewReader.ActivePosition.Line + GlobalConstants.PageSize);
                    break;
            }

            return endingPosition;
        }

        public IEnumerable<PointsPair> GetSelectionPointsForward(TextPosition start, TextPosition end) {
            var pairs = new List<PointsPair>();

            for (int i = start.Line; i <= end.Line; i++) {
                int tmpStartColumn = 0;
                int tmpStartLine = i;
                int tmpEndColumn = 0;
                int tmpEndLine = i;

                if (i == start.Line) {
                    tmpStartColumn = start.Column;
                }

                int lineLen = textViewReader.GetLineLength(i);

                if (i == end.Line) {
                    tmpEndColumn = end.Column > lineLen ? lineLen : end.Column;
                } else {
                    tmpEndColumn = lineLen;
                }

                pairs.Add(new PointsPair {
                    StartingPoint = (new TextPosition(column: tmpStartColumn, line: tmpStartLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                                  .AlignToVisualLineTop(TextConfiguration.GetCharSize()),
                    EndingPoint = (new TextPosition(column: tmpEndColumn, line: tmpEndLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                            .AlignToVisualLineBottom(TextConfiguration.GetCharSize())
                });
            }

            return pairs;
        }

        public IEnumerable<PointsPair> GetSelectionPointsInverted(TextPosition start, TextPosition end) {
            var pairs = new List<PointsPair>();

            for (int i = start.Line; i >= end.Line; i--) {
                int tmpStartColumn = 0;
                int tmpStartLine = i;
                int tmpEndColumn = 0;
                int tmpEndLine = i;
                int lineLen = textViewReader.GetLineLength(i);

                if (i == start.Line) {
                    tmpStartColumn = start.Column;
                } else if (i == end.Line) {
                    tmpStartColumn = end.Column > lineLen ? lineLen : end.Column;
                    tmpEndColumn = lineLen;
                } else {
                    tmpStartColumn = 0;
                    tmpEndColumn = lineLen;
                }

                pairs.Add(new PointsPair {
                    StartingPoint = (new TextPosition(column: tmpStartColumn, line: tmpStartLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                                  .AlignToVisualLineTop(TextConfiguration.GetCharSize()),
                    EndingPoint = (new TextPosition(column: tmpEndColumn, line: tmpEndLine)).GetPositionRelativeToParent(TextConfiguration.GetCharSize())
                                                                                            .AlignToVisualLineBottom(TextConfiguration.GetCharSize())
                });
            }

            return pairs;
        }

        #endregion

        #region methods

        private int[] GetStandardWordSelectionRange(TextPosition clickPosition, VisualTextLine activeLine, int lineLength) {
            int startColumn = clickPosition.Column;
            int endColumn = clickPosition.Column;

            if (endColumn + 1 <= lineLength) {
                endColumn += 1;
            } else {
                startColumn -= 1;
            }
            // move left from current position
            for (int i = clickPosition.Column; i >= 0; i--) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (charInfo.IsCharacter && char.IsLetterOrDigit(charInfo.Character)) {
                    startColumn = i;
                } else {
                    break;
                }
            }
            // move right from current position
            for (int i = clickPosition.Column; i < lineLength; i++) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (charInfo.IsCharacter && char.IsLetterOrDigit(charInfo.Character)) {
                    endColumn = i + 1;
                } else {
                    break;
                }
            }

            return new[] { startColumn, endColumn };
        }

        private int[] GetCollapseSelectionRange(TextPosition clickPosition, VisualTextLine activeLine, int lineLength) {
            int startColumn = clickPosition.Column;
            int endColumn = clickPosition.Column;

            if (endColumn + 1 <= lineLength) {
                endColumn += 1;
            } else {
                startColumn -= 1;
            }
            // move left from current position
            for (int i = clickPosition.Column; i >= 0; i--) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (!charInfo.IsCharacter && !char.IsLetterOrDigit(charInfo.Character)) {
                    startColumn = i;
                } else {
                    break;
                }
            }
            // move right from current position
            for (int i = clickPosition.Column; i < lineLength; i++) {
                var charInfo = activeLine.GetCharInfoAt(i);

                if (!charInfo.IsCharacter && !char.IsLetterOrDigit(charInfo.Character)) {
                    endColumn = i + 1;
                } else {
                    break;
                }
            }

            return new[] { startColumn, endColumn };
        }

        #endregion

    }
}
