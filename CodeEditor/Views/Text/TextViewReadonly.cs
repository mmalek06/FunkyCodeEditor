﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.Enums;
using CodeEditor.TextProperties;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Views.Text {
    internal partial class TextView : ITextViewReadonly {
        
        #region properties

        public int LinesCount => visuals.Count;

        #endregion

        #region public methods

        public IReadOnlyList<string> GetScreenLines() =>
            visuals.ToEnumerableOf<VisualTextLine>().Select(line => line.RenderedText).ToArray();

        public IReadOnlyList<string> GetActualLines() => 
            GetTextPartsBetweenPositions(new TextPosition(column: 0, line: 0), new TextPosition(column: GetLineLength(LinesCount - 1), line: LinesCount - 1)).ToArray();

        public IReadOnlyList<VisualTextLine> GetVisualLines() => visuals.ToEnumerableOf<VisualTextLine>().ToArray();

        public TextPosition AdjustStep(TextPosition newPosition, CaretMoveDirection moveDirection) {
            var line = ((VisualTextLine)visuals[newPosition.Line]);
            var charInfo = line.RenderedText != string.Empty && newPosition.Column < GetLineLength(newPosition.Line) ? line.GetCharInfoAt(newPosition.Column) : null;

            if (charInfo != null) {
                return charInfo.IsCharacter ? newPosition : GetAdjustedPosition(charInfo, moveDirection);
            }

            return newPosition;
        }

        public char GetCharAt(TextPosition position) {
            var info = ((VisualTextLine)visuals[position.Line]).GetCharInfoAt(position.Column);

            return info.IsCharacter ? info.Text[0] : default(char);
        }

        public int GetLineLength(int index) => visuals.Count == 0 ? 0 : ((VisualTextLine)visuals[index]).Length;

        public string GetLine(int index) => index >= visuals.Count ? string.Empty : ((VisualTextLine)visuals[index]).RenderedText;

        public VisualTextLine GetVisualLine(int index) => index >= visuals.Count ? null : ((VisualTextLine)visuals[index]);

        public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
            var parts = visuals.ToEnumerableOf<VisualTextLine>().SelectMany(line => line.GetStringContents()).ToList();
                
            if (parts.Count == 0) {
                return parts;
            }
            if (startPosition.Line == endPosition.Line) {
                parts[0] = string.Join("", parts[0].Skip(startPosition.Column).Take(endPosition.Column - startPosition.Column));
            } else {
                parts[0] = parts[0].Substring(startPosition.Column);
            }

            var lastIndex = parts.Count - 1;
            var substringTo = endPosition.Column;

            if (startPosition.Line == endPosition.Line) {
                substringTo = endPosition.Column - startPosition.Column;
            }

            parts[lastIndex] = string.Join("", parts[lastIndex].Take(substringTo));

            return parts;
        }

        public bool IsInTextRange(TextPosition position) {
            if (position.Column < 0 || position.Line < 0) {
                return false;
            }
            if (position.Line >= LinesCount) {
                return false;
            }

            return position.Column <= GetLineLength(position.Line);
        }

        #endregion

        #region methods

        private TextPosition GetAdjustedPosition(CharInfo charInfo, CaretMoveDirection moveDirection) {
            switch (moveDirection) {
                case CaretMoveDirection.LEFT:
                    return charInfo.PrevCharPosition;
                case CaretMoveDirection.RIGHT:
                    return charInfo.NextCharPosition;
                case CaretMoveDirection.BOTTOM:
                case CaretMoveDirection.TOP:
                    return charInfo.PrevCharPosition;
            }

            throw new ArgumentException(nameof(moveDirection));
        }

        #endregion

    }
}
