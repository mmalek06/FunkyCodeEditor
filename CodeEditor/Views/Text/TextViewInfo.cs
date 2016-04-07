using System;
using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Extensions;
using CodeEditor.Enums;
using CodeEditor.TextProperties;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    /// <summary>
    /// provides methods to read data from TextView instance
    /// since there may be many TextViews, each should have it's own TextViewInfo instance - hence the instanceMap variable
    /// </summary>
    internal partial class TextView {

        public class TextViewInfo {

            #region fields

            private TextView parent;

            private static Dictionary<TextView, TextViewInfo> instanceMap;

            #endregion

            #region properties

            public int LinesCount => parent.visuals.Count;

            public TextPosition ActivePosition => parent.ActivePosition;

            #endregion

            #region constructors

            private TextViewInfo(TextView parent) {
                this.parent = parent;
            }

            public static TextViewInfo GetInstance(TextView parent) {
                if (instanceMap == null) {
                    instanceMap = new Dictionary<TextView, TextViewInfo>();
                }
                if (!instanceMap.ContainsKey(parent)) {
                    instanceMap[parent] = new TextViewInfo(parent);
                }

                return instanceMap[parent];
            }

            #endregion

            #region public methods

            public IReadOnlyList<string> GetScreenLines() =>
                parent.visuals.ToEnumerableOf<VisualTextLine>().Select(line => line.RenderedText).ToArray();

            public IReadOnlyList<string> GetActualLines() => 
                GetTextPartsBetweenPositions(new TextPosition(column: 0, line: 0), new TextPosition(column: GetLineLength(LinesCount - 1), line: LinesCount - 1)).ToArray();

            public IReadOnlyList<VisualTextLine> GetVisualLines() =>
                parent.visuals.ToEnumerableOf<VisualTextLine>().ToArray();

            public TextPosition AdjustStep(TextPosition newPosition, CaretMoveDirection moveDirection) {
                var line = ((VisualTextLine)parent.visuals[newPosition.Line]);
                CharInfo charInfo = line.RenderedText != string.Empty && newPosition.Column < GetLineLength(newPosition.Line) ? line.GetCharInfoAt(newPosition.Column) : null;

                if (charInfo != null) {
                    if (charInfo.IsCharacter) {
                        return newPosition;
                    } else {
                        return GetAdjustedPosition(charInfo, moveDirection);
                    }
                }

                return newPosition;
            }

            public char GetCharAt(TextPosition position) => ((VisualTextLine)parent.visuals[position.Line]).GetCharInfoAt(position.Column).Character;

            public int GetLineLength(int index) => parent.visuals.Count == 0 ? 0 : ((VisualTextLine)parent.visuals[index]).Length;

            public string GetLine(int index) => index >= parent.visuals.Count ? string.Empty : ((VisualTextLine)parent.visuals[index]).RenderedText;

            public VisualTextLine GetVisualLine(int index) => index >= parent.visuals.Count ? null : ((VisualTextLine)parent.visuals[index]);

            public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
                var parts = parent.visuals.ToEnumerableOf<VisualTextLine>().SelectMany(line => line.GetStringContents()).ToList();
                
                if (parts.Count == 0) {
                    return parts;
                }

                if (startPosition.Line == endPosition.Line) {
                    parts[0] = parts[0].Substring(startPosition.Column, endPosition.Column);
                } else {
                    parts[0] = parts[0].Substring(startPosition.Column);
                }

                int lastIndex = parts.Count - 1;
                int substringTo = endPosition.Column;

                if (startPosition.Line == endPosition.Line) {
                    substringTo = endPosition.Column - startPosition.Column;
                }

                parts[lastIndex] = parts[lastIndex].Substring(0, substringTo);

                return parts;
            }

            public bool IsInTextRange(TextPosition position) {
                if (position.Column < 0 || position.Line < 0) {
                    return false;
                }
                if (position.Line >= LinesCount) {
                    return false;
                }
                if (position.Column > GetLineLength(position.Line)) {
                    return false;
                }

                return true;
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
}
