using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeEditor.DataStructures;
using CodeEditor.TextProperties;

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

        public IDictionary<int, string> UpdateLines(IList<SimpleTextSource> textSources, TextPosition startingTextPosition, string text) {
            var replacedText = SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextProperties.Properties.NEWLINE) {
                return LineAdded(textSources, text, startingTextPosition);
            } else if (text == TextProperties.Properties.TAB) {
                return TabPressed(textSources, text, startingTextPosition);
            } else if (replacedText.Length == 1) {
                return CharacterEntered(textSources, replacedText, startingTextPosition);
            } else {
                return TextPasted(textSources, text, startingTextPosition);
            }
        }

        #endregion

        #region methods

        private IDictionary<int, string> LineAdded(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) {
            string textBeforeCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Take(startingTextPosition.Column));
            string textAfterCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Skip(startingTextPosition.Column));
            var transformations = new Dictionary<int, string> {
                [startingTextPosition.Line] = textBeforeCursorPosition,
                [startingTextPosition.Line + 1] = textAfterCursorPosition
            };

            for (int i = startingTextPosition.Line + 1; i < textSources.Count; i++) {
                transformations[i + 1] = textSources[i].Text;
            }

            return transformations;
        }

        private IDictionary<int, string> TextPasted(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) =>
            text.Split(TextProperties.Properties.NEWLINE[0])
                .Select((line, index) => GetPositionWithText(line, index, startingTextPosition.Line, startingTextPosition.Column))
                .ToDictionary(pair => pair.Item1, kvp => kvp.Item2);

        private IDictionary<int, string> TabPressed(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) {
            string textBeforeCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Take(startingTextPosition.Column));
            string textAfterCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Skip(startingTextPosition.Column));

            return new Dictionary<int, string> {
                [startingTextPosition.Line] = textBeforeCursorPosition + new string(' ', TextProperties.Properties.TabSize) + textAfterCursorPosition
            };
        }

        private IDictionary<int, string> CharacterEntered(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) {
            var currentTextLine = new StringBuilder();

            if (textSources.Any()) {
                if (startingTextPosition.Column >= textSources[startingTextPosition.Line].Text.Length) {
                    currentTextLine.Append(textSources[startingTextPosition.Line].Text).Append(text);
                } else {
                    currentTextLine.Append(textSources[startingTextPosition.Line].Text).Insert(startingTextPosition.Column, text);
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