﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TextEditor.DataStructures;
using TextEditor.TextProperties;

namespace TextEditor.Views.TextView {
    internal class TextLineRemover {

        #region public methods

        public LinesRemovalInfo TransformLines(IList<SimpleTextSource> textSources, TextPositionsPair range) {
            var orderedRanges = (new[] { range.StartPosition, range.EndPosition }).OrderBy(elem => elem.Line).ThenBy(elem => elem.Column).ToArray();
            var pair = new TextPositionsPair {
                StartPosition = orderedRanges[0],
                EndPosition = orderedRanges[1]
            };

            return new LinesRemovalInfo {
                LinesAffected = new Dictionary<TextPosition, string> {
                    [new TextPosition { Column = pair.StartPosition.Column, Line = range.StartPosition.Line }] =
                        string.Concat(textSources[pair.StartPosition.Line].Text.Take(pair.StartPosition.Column)) +
                        string.Concat(textSources[pair.EndPosition.Line].Text.Skip(pair.EndPosition.Column))
                },
                LinesToRemove = Enumerable.Range(pair.StartPosition.Line + 1, textSources.Count - (pair.StartPosition.Line + 1))
            };
        }

        public LinesRemovalInfo TransformLines(IList<SimpleTextSource> textSources, TextPosition startingTextPosition, Key key) {
            if (key == Key.Delete) {
                bool isStartEqToTextLen = startingTextPosition.Column == textSources[startingTextPosition.Line].Text.Length;

                if (isStartEqToTextLen && startingTextPosition.Line == textSources.Count - 1) {
                    return new LinesRemovalInfo { LinesAffected = new Dictionary<TextPosition, string>(), LinesToRemove = new int[0] };
                }
                if (isStartEqToTextLen) {
                    return DeleteNextLine(textSources, startingTextPosition);
                } else {
                    return DeleteFromActiveLine(textSources, startingTextPosition);
                }
            } else {
                bool isStartEqZero = startingTextPosition.Column == 0;

                if (isStartEqZero && startingTextPosition.Line == 0) {
                    return new LinesRemovalInfo { LinesAffected = new Dictionary<TextPosition, string>(), LinesToRemove = new int[0] };
                }
                if (isStartEqZero) {
                    return RemoveThisLine(textSources, startingTextPosition);
                } else {
                    return RemoveFromActiveLine(textSources, startingTextPosition);
                }
            }
        }

        #endregion

        #region methods

        private LinesRemovalInfo RemoveFromActiveLine(IList<SimpleTextSource> textSources, TextPosition startingTextPosition) {
            string lineToModify = textSources[startingTextPosition.Line].Text;
            bool attachRest = startingTextPosition.Column < textSources[startingTextPosition.Line].Text.Length;
            string textAfterRemove = lineToModify.Substring(0, startingTextPosition.Column - 1) + (attachRest ? lineToModify.Substring(startingTextPosition.Column) : string.Empty);

            return new LinesRemovalInfo {
                LinesAffected = new Dictionary<TextPosition, string> {
                    [new TextPosition { Column = startingTextPosition.Column - 1, Line = startingTextPosition.Line }] = textAfterRemove },
                LinesToRemove = new int[0]
            };
        }

        private LinesRemovalInfo DeleteFromActiveLine(IList<SimpleTextSource> textSources, TextPosition startingTextPosition) {
            string lineToModify = textSources[startingTextPosition.Line].Text;
            string textAfterRemove = lineToModify.Substring(0, startingTextPosition.Column) + lineToModify.Substring(startingTextPosition.Column + 1);

            return new LinesRemovalInfo {
                LinesAffected = new Dictionary<TextPosition, string> {
                    [new TextPosition { Column = startingTextPosition.Column, Line = startingTextPosition.Line }] = textAfterRemove },
                LinesToRemove = new int[0]
            };
        }

        private LinesRemovalInfo RemoveThisLine(IList<SimpleTextSource> textSources, TextPosition startingTextPosition) {
            var linesAffected = new List<KeyValuePair<TextPosition, string>> {
                new KeyValuePair<TextPosition, string>(
                    new TextPosition { Column = textSources[startingTextPosition.Line - 1].Text.Length, Line = startingTextPosition.Line - 1 },
                    GetText(textSources, startingTextPosition.Line - 1) + textSources[startingTextPosition.Line].Text)
            };

            for (int i = startingTextPosition.Line + 1; i < textSources.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, string>(new TextPosition { Column = 0, Line = i - 1 }, textSources[i].Text));
            }

            return new LinesRemovalInfo {
                LinesAffected = linesAffected,
                LinesToRemove = new[] { startingTextPosition.Line }
            };
        }

        private LinesRemovalInfo DeleteNextLine(IList<SimpleTextSource> textSources, TextPosition startingTextPosition) {
            var linesAffected = new List<KeyValuePair<TextPosition, string>> {
                new KeyValuePair<TextPosition, string>(
                    new TextPosition { Column = startingTextPosition.Column, Line = startingTextPosition.Line },
                    textSources[startingTextPosition.Line].Text + GetText(textSources, startingTextPosition.Line + 1))
            };
            
            for (int i = startingTextPosition.Line + 2; i < textSources.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, string>(new TextPosition { Column = 0, Line = i - 1 }, textSources[i].Text));
            }

            return new LinesRemovalInfo {
                LinesAffected = linesAffected,
                LinesToRemove = new[] { textSources.Count - 1 }
            };
        }

        private string GetText(IList<SimpleTextSource> textSources, int lineIdx) {
            if (lineIdx < textSources.Count && !string.IsNullOrEmpty(textSources[lineIdx].Text)) {
                return textSources[lineIdx].Text;
            }

            return string.Empty;
        }

        #endregion

    }
}
