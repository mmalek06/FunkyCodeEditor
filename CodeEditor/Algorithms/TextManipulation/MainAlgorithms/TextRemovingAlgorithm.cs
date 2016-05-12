using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.TextProperties;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextRemovingAlgorithm {

        #region public methods

        public ChangeInLinesInfo GetChangeInLines(IReadOnlyList<VisualTextLine> lines, TextRange range) {
            var orderedRanges = (new[] { range.StartPosition, range.EndPosition }).OrderBy(elem => elem.Line).ThenBy(elem => elem.Column).ToArray();
            var pair = new TextRange {
                StartPosition = orderedRanges[0],
                EndPosition = orderedRanges[1]
            };
            int rangeEnd = -1;

            if (pair.StartPosition.Line == pair.EndPosition.Line) {
                rangeEnd = pair.EndPosition.Line;
            } else {
                rangeEnd = pair.EndPosition.Line + 1;
            }
            
            var firstPart = Cut(lines[pair.StartPosition.Line], 0, pair.StartPosition.Column);
            var secondPart = Cut(lines[pair.EndPosition.Line], pair.EndPosition.Column);

            return new ChangeInLinesInfo {
                LinesToChange = new Dictionary<TextPosition, VisualTextLine> {
                    [new TextPosition(pair.StartPosition.Column, pair.StartPosition.Line)] =
                        VisualTextLine.MergeLines(new[] { firstPart, secondPart }, firstPart.Index)
                },
                LinesToRemove = Enumerable.Range(pair.StartPosition.Line, rangeEnd).ToList()
            };
        }

        public ChangeInLinesInfo GetChangeInLines(IReadOnlyList<VisualTextLine> lines, TextPosition startingTextPosition, Key key) {
            if (key == Key.Delete) {
                bool isStartEqToTextLen = startingTextPosition.Column == lines[startingTextPosition.Line].Length;

                if (isStartEqToTextLen && startingTextPosition.Line == lines.Count) {
                    return new ChangeInLinesInfo { LinesToChange = new Dictionary<TextPosition, VisualTextLine>(), LinesToRemove = new int[0] };
                }
                if (isStartEqToTextLen) {
                    return DeleteNextLine(lines, startingTextPosition);
                } else {
                    return DeleteFromActiveLine(lines, startingTextPosition);
                }
            } else {
                bool isStartEqZero = startingTextPosition.Column == 0;

                if (isStartEqZero && startingTextPosition.Line == 0) {
                    return new ChangeInLinesInfo { LinesToChange = new Dictionary<TextPosition, VisualTextLine>(), LinesToRemove = new[] { startingTextPosition.Line } };
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

        private ChangeInLinesInfo RemoveFromActiveLine(IReadOnlyList<VisualTextLine> lines, TextPosition startingTextPosition) {
            var currentLine = lines[startingTextPosition.Line];
            bool attachRest = startingTextPosition.Column < lines[startingTextPosition.Line].Length;
            var firstPart = Cut(currentLine, 0, startingTextPosition.Column - 1);
            var secondPart = attachRest ? Cut(currentLine, startingTextPosition.Column) : null;
            var lineAfterRemove = secondPart != null ? VisualTextLine.MergeLines(new[] { firstPart, secondPart }, currentLine.Index) : firstPart;

            return new ChangeInLinesInfo {
                LinesToChange = new Dictionary<TextPosition, VisualTextLine> {
                    [new TextPosition(startingTextPosition.Column - 1, startingTextPosition.Line)] = lineAfterRemove
                },
                LinesToRemove = new int[0]
            };
        }

        private ChangeInLinesInfo DeleteFromActiveLine(IReadOnlyList<VisualTextLine> lines, TextPosition startingTextPosition) {
            var currentLine = lines[startingTextPosition.Line];
            var firstPart = Cut(currentLine, 0, startingTextPosition.Column);
            var secondPart = Cut(currentLine, startingTextPosition.Column + 1);
            var lineAfterRemove = VisualTextLine.MergeLines(new[] { firstPart, secondPart }, currentLine.Index);

            return new ChangeInLinesInfo {
                LinesToChange = new Dictionary<TextPosition, VisualTextLine> {
                    [new TextPosition(startingTextPosition.Column, startingTextPosition.Line)] = lineAfterRemove
                },
                LinesToRemove = new int[0]
            };
        }

        private ChangeInLinesInfo RemoveThisLine(IReadOnlyList<VisualTextLine> lines, TextPosition startingTextPosition) {
            var firstLine = lines[startingTextPosition.Line - 1];
            var linesAffected = new List<KeyValuePair<TextPosition, VisualTextLine>> {
                new KeyValuePair<TextPosition, VisualTextLine>(
                    new TextPosition(lines[startingTextPosition.Line - 1].Length, startingTextPosition.Line - 1),
                    VisualTextLine.MergeLines(new[] { firstLine, lines[startingTextPosition.Line] }, firstLine.Index))
            };

            for (int i = startingTextPosition.Line + 1; i < lines.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, VisualTextLine>(new TextPosition(0, i - 1), lines[i].CloneWithIndexChange(i - 1)));
            }

            return new ChangeInLinesInfo {
                LinesToChange = linesAffected,
                LinesToRemove = new[] { lines.Count - 1 }
            };
        }

        private ChangeInLinesInfo DeleteNextLine(IReadOnlyList<VisualTextLine> lines, TextPosition startingTextPosition) {
            var firstLine = lines[startingTextPosition.Line];
            var secondLine = startingTextPosition.Line + 1 < lines.Count ? lines[startingTextPosition.Line + 1] : null;
            var linesToMerge = secondLine == null ? new[] { firstLine } : new[] { firstLine, secondLine };
            var linesAffected = new List<KeyValuePair<TextPosition, VisualTextLine>> {
                new KeyValuePair<TextPosition, VisualTextLine>(
                    new TextPosition(startingTextPosition.Column, startingTextPosition.Line),
                    VisualTextLine.MergeLines(linesToMerge, firstLine.Index))
            };

            for (int i = startingTextPosition.Line + 2; i < lines.Count; i++) {
                linesAffected.Add(new KeyValuePair<TextPosition, VisualTextLine>(new TextPosition(0, i - 1), lines[i].CloneWithIndexChange(i - 1)));
            }

            return new ChangeInLinesInfo {
                LinesToChange = linesAffected,
                LinesToRemove = new[] { lines.Count - 1 }
            };
        }

        private VisualTextLine Cut(VisualTextLine line, int startIndex, int? count = null) {
            int substringLength = count == null ? line.Length - startIndex : count.Value;

            if (line is CollapsedVisualTextLine) {
                var charInfos = Enumerable.Range(startIndex, substringLength).Select(index => line.GetCharInfoAt(index));

                if (charInfos.Any(info => !info.IsCharacter)) {
                    return GetPartialLineBeforeCollapse(charInfos, line, startIndex, substringLength);
                } else {
                    return GetPartialLineAfterCollapse(line, startIndex, substringLength);
                }                
            } else {
                return CutStandardLine(line, startIndex, substringLength);
            }
        }

        private VisualTextLine CutStandardLine(VisualTextLine line, int startIndex, int count) {
            string contents = line.GetStringContents()[0];
            int length = count - startIndex;
            string newText = string.Empty;

            if (startIndex + count == line.Length) {
                newText = contents.Substring(startIndex);
            } else {
                newText = contents.Substring(startIndex, count - startIndex);
            }

            return VisualTextLine.Create(newText, line.Index);
        }

        private VisualTextLine GetPartialLineAfterCollapse(VisualTextLine line, int startIndex, int count) {
            string firstPart = line.RenderedText.Substring(startIndex, count);

            return VisualTextLine.Create(firstPart, line.Index);
        }

        private VisualTextLine GetPartialLineBeforeCollapse(IEnumerable<CharInfo> charInfos, VisualTextLine line, int startIndex, int count) {
            var collapseInfo = charInfos.First(info => !info.IsCharacter);
            var contents = line.RenderedText;
            string textBeforeCollapse = string.Empty;

            if (collapseInfo.PrevCharPosition.Column >= startIndex) {
                textBeforeCollapse = contents.Substring(startIndex, collapseInfo.PrevCharPosition.Column);
            } else {
                textBeforeCollapse = contents.Substring(startIndex);
            }

            int diff = 0;

            if (startIndex + count >= collapseInfo.NextCharPosition.Column) {
                diff = (startIndex + count) - collapseInfo.NextCharPosition.Column;
            }

            string textAfterCollapse = string.Join("", contents.Skip(collapseInfo.NextCharPosition.Column).Take(diff));

            return VisualTextLine.Create(textBeforeCollapse + textAfterCollapse, line.Index);
        }

        #endregion

    }
}