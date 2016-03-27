﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextRemover {

        #region public methods

        public LinesRemovalInfo RemoveLines(IList<string> lines, TextPositionsPair range) {
            var orderedRanges = (new[] { range.StartPosition, range.EndPosition }).OrderBy(elem => elem.Line).ThenBy(elem => elem.Column).ToArray();
            var pair = new TextPositionsPair {
                StartPosition = orderedRanges[0],
                EndPosition = orderedRanges[1]
            };
            int rangeEnd = -1;

            if (pair.StartPosition.Line == pair.EndPosition.Line) {
                rangeEnd = pair.EndPosition.Line;
            } else {
                rangeEnd = pair.EndPosition.Line + 1;
            }

            return new LinesRemovalInfo {
                LinesToChange = new Dictionary<TextPosition, string> {
                    [new TextPosition(pair.StartPosition.Column, pair.StartPosition.Line)] =
                        string.Concat(lines[pair.StartPosition.Line].Take(pair.StartPosition.Column)) +
                        string.Concat(lines[pair.EndPosition.Line].Skip(pair.EndPosition.Column))
                },
                LinesToRemove = Enumerable.Range(pair.StartPosition.Line, rangeEnd)
            };
        }

        public LinesRemovalInfo RemoveLines(IList<string> lines, TextPosition startingTextPosition, Key key) {
            if (key == Key.Delete) {
                bool isStartEqToTextLen = startingTextPosition.Column == lines[startingTextPosition.Line].Length;

                if (isStartEqToTextLen && startingTextPosition.Line == lines.Count - 1) {
                    return new LinesRemovalInfo { LinesToChange = new Dictionary<TextPosition, string>(), LinesToRemove = new int[0] };
                }
                if (isStartEqToTextLen) {
                    return DeleteNextLine(lines, startingTextPosition);
                } else {
                    return DeleteFromActiveLine(lines, startingTextPosition);
                }
            } else {
                bool isStartEqZero = startingTextPosition.Column == 0;

                if (isStartEqZero && startingTextPosition.Line == 0) {
                    return new LinesRemovalInfo { LinesToChange = new Dictionary<TextPosition, string>(), LinesToRemove = new[] { startingTextPosition.Line } };
                }
                if (isStartEqZero) {
                    return RemoveThisLine(lines, startingTextPosition);
                } else {
                    return RemoveFromActiveLine(lines, startingTextPosition);
                }
            }
        }

        #endregion

        #region methods

        private LinesRemovalInfo RemoveFromActiveLine(IList<string> lines, TextPosition startingTextPosition) {
            string lineToModify = lines[startingTextPosition.Line];
            bool attachRest = startingTextPosition.Column < lines[startingTextPosition.Line].Length;
            string textAfterRemove = lineToModify.Substring(0, startingTextPosition.Column - 1) + (attachRest ? lineToModify.Substring(startingTextPosition.Column) : string.Empty);

            return new LinesRemovalInfo {
                LinesToChange = new Dictionary<TextPosition, string> {
                    [new TextPosition(startingTextPosition.Column - 1, startingTextPosition.Line)] = textAfterRemove },
                LinesToRemove = new int[0]
            };
        }

        private LinesRemovalInfo DeleteFromActiveLine(IList<string> lines, TextPosition startingTextPosition) {
            string lineToModify = lines[startingTextPosition.Line];
            string textAfterRemove = lineToModify.Substring(0, startingTextPosition.Column) + lineToModify.Substring(startingTextPosition.Column + 1);

            return new LinesRemovalInfo {
                LinesToChange = new Dictionary<TextPosition, string> {
                    [new TextPosition(startingTextPosition.Column, startingTextPosition.Line)] = textAfterRemove },
                LinesToRemove = new int[0]
            };
        }

        private LinesRemovalInfo RemoveThisLine(IList<string> lines, TextPosition startingTextPosition) {
            var linesAffected = new List<KeyValuePair<TextPosition, string>> {
                new KeyValuePair<TextPosition, string>(
                    new TextPosition(lines[startingTextPosition.Line - 1].Length, startingTextPosition.Line - 1),
                    GetText(lines, startingTextPosition.Line - 1) + lines[startingTextPosition.Line])
            };

            for (int i = startingTextPosition.Line + 1; i < lines.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, string>(new TextPosition(0, i - 1), lines[i]));
            }

            return new LinesRemovalInfo {
                LinesToChange = linesAffected,
                LinesToRemove = new[] { lines.Count - 1 }
            };
        }

        private LinesRemovalInfo DeleteNextLine(IList<string> lines, TextPosition startingTextPosition) {
            var linesAffected = new List<KeyValuePair<TextPosition, string>> {
                new KeyValuePair<TextPosition, string>(
                    new TextPosition(startingTextPosition.Column, startingTextPosition.Line),
                    lines[startingTextPosition.Line] + GetText(lines, startingTextPosition.Line + 1))
            };
            
            for (int i = startingTextPosition.Line + 2; i < lines.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, string>(new TextPosition(0, i - 1), lines[i]));
            }

            return new LinesRemovalInfo {
                LinesToChange = linesAffected,
                LinesToRemove = new[] { lines.Count - 1 }
            };
        }

        private string GetText(IList<string> lines, int lineIdx) {
            if (lineIdx < lines.Count && !string.IsNullOrEmpty(lines[lineIdx])) {
                return lines[lineIdx];
            }

            return string.Empty;
        }

        #endregion

    }
}
