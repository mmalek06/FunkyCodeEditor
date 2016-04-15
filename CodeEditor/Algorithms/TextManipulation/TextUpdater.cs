using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextUpdater {

        #region public properties

        public Regex SpecialCharsRegex { get; private set; }

        #endregion

        #region constructor

        public TextUpdater() {
            SpecialCharsRegex = new Regex("[\a|\b|\n|\r|\f|\t|\v]");
        }

        #endregion

        #region public methods

        public IReadOnlyDictionary<int, string> GetChangeInLines(IReadOnlyList<string> lines, TextPosition startingTextPosition, string text) {
            var replacedText = SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextProperties.Properties.NEWLINE) {
                return LineAdded(lines, text, startingTextPosition);
            } else if (text == TextProperties.Properties.TAB) {
                return TabPressed(lines, text, startingTextPosition);
            } else if (replacedText.Length == 1) {
                return CharacterEntered(lines, replacedText, startingTextPosition);
            } else {
                return TextPasted(lines, text, startingTextPosition);
            }
        }

        #endregion

        #region methods

        private IReadOnlyDictionary<int, string> LineAdded(IReadOnlyList<string> lines, string text, TextPosition startingTextPosition) {
            string textBeforeCursorPosition = string.Concat(lines[startingTextPosition.Line].Take(startingTextPosition.Column));
            string textAfterCursorPosition = string.Concat(lines[startingTextPosition.Line].Skip(startingTextPosition.Column));
            var transformations = new Dictionary<int, string> {
                [startingTextPosition.Line] = textBeforeCursorPosition,
                [startingTextPosition.Line + 1] = textAfterCursorPosition
            };

            for (int i = startingTextPosition.Line + 1; i < lines.Count; i++) {
                transformations[i + 1] = lines[i];
            }

            return transformations;
        }

        private IReadOnlyDictionary<int, string> TextPasted(IReadOnlyList<string> lines, string text, TextPosition startingTextPosition) =>
            text.Split(TextProperties.Properties.NEWLINE[0])
                .Select((line, index) => GetPositionWithText(line, index, startingTextPosition.Line, startingTextPosition.Column))
                .ToDictionary(pair => pair.Item1, kvp => kvp.Item2);

        private IReadOnlyDictionary<int, string> TabPressed(IReadOnlyList<string> lines, string text, TextPosition startingTextPosition) {
            string textBeforeCursorPosition = string.Concat(lines[startingTextPosition.Line].Take(startingTextPosition.Column));
            string textAfterCursorPosition = string.Concat(lines[startingTextPosition.Line].Skip(startingTextPosition.Column));

            return new Dictionary<int, string> {
                [startingTextPosition.Line] = textBeforeCursorPosition + new string(' ', TextProperties.Properties.TabSize) + textAfterCursorPosition
            };
        }

        private IReadOnlyDictionary<int, string> CharacterEntered(IReadOnlyList<string> lines, string text, TextPosition startingTextPosition) {
            var currentTextLine = new StringBuilder();

            if (lines.Any()) {
                if (startingTextPosition.Column >= lines[startingTextPosition.Line].Length) {
                    currentTextLine.Append(lines[startingTextPosition.Line]).Append(text);
                } else {
                    currentTextLine.Append(lines[startingTextPosition.Line]).Insert(startingTextPosition.Column, text);
                }
            } else {
                currentTextLine.Append(text);
            }

            return new Dictionary<int, string> {
                [startingTextPosition.Line] = currentTextLine.ToString()
            };
        }

        private Tuple<int, string> GetPositionWithText(string line, int loopIndex, int startingLineIdx, int startingColIdx) {
            string normalizedText = NormalizeText(line);
            int colIdx = loopIndex == 0 ? startingColIdx : 0;

            return new Tuple<int, string>(startingLineIdx + loopIndex, normalizedText);
        }

        private string NormalizeText(string text) {
            string normalizedText = text;

            if (text == TextProperties.Properties.NEWLINE) {
                normalizedText = string.Empty;
            } else if (text == TextProperties.Properties.TAB) {
                normalizedText = new string(' ', TextProperties.Properties.TabSize);
            }

            return normalizedText;
        }

        #endregion

    }
}