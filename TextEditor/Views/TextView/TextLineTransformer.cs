using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TextEditor.DataStructures;
using TextEditor.TextProperties;

namespace TextEditor.Views.TextView {
    internal class TextLineTransformer {

        #region public properties

        public Regex SpecialCharsRegex { get; private set; }

        #endregion

        #region constructor

        public TextLineTransformer() {
            SpecialCharsRegex = new Regex("[\a|\b|\n|\r|\f|\t|\v]");
        }

        #endregion

        #region public methods

        public IDictionary<TextPosition, string> TransformLines(IList<SimpleTextSource> textSources, TextPosition startingTextPosition, string text) {
            var replacedText = SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextConfiguration.NEWLINE) {
                return LineAdded(textSources, text, startingTextPosition);
            } else if (replacedText.Length == 1) {
                return CharacterEntered(textSources, replacedText, startingTextPosition);
            } else {
                return TextPasted(textSources, text, startingTextPosition);
            }
        }

        #endregion

        #region methods

        private IDictionary<TextPosition, string> LineAdded(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) {
            string textBeforeCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Take(startingTextPosition.Column));
            string textAfterCursorPosition = string.Concat(textSources[startingTextPosition.Line].Text.Skip(startingTextPosition.Column));
            var transformations = new Dictionary<TextPosition, string> {
                [new TextPosition { Column = startingTextPosition.Column, Line = startingTextPosition.Line }] = textBeforeCursorPosition,
                [new TextPosition { Column = 0, Line = startingTextPosition.Line + 1 }] = textAfterCursorPosition
            };

            for (int i = startingTextPosition.Line + 1; i < textSources.Count; i++) {
                transformations[new TextPosition { Column = 0, Line = i + 1 }] = textSources[i].Text;
            }

            return transformations;
        }

        private IDictionary<TextPosition, string> CharacterEntered(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) {
            string currentLineText = string.Empty;

            if (textSources.Any()) {
                currentLineText = textSources[startingTextPosition.Line].Text;

                if (startingTextPosition.Column >= currentLineText.Length) {
                    currentLineText += text;
                } else {
                    currentLineText.Insert(startingTextPosition.Column, text);
                }
            } else {
                currentLineText = text;
            }

            return new Dictionary<TextPosition, string> {
                [new TextPosition { Column = startingTextPosition.Column + 1, Line = startingTextPosition.Line }] = currentLineText
            };
        }

        private IDictionary<TextPosition, string> TextPasted(IList<SimpleTextSource> textSources, string text, TextPosition startingTextPosition) =>
            text.Split(TextConfiguration.NEWLINE[0])
                .Select((line, index) => GetPositionWithText(line, index, startingTextPosition.Line, startingTextPosition.Column))
                .ToDictionary(pair => pair.Item1, kvp => kvp.Item2);

        private Tuple<TextPosition, string> GetPositionWithText(string line, int loopIndex, int startingLineIdx, int startingColIdx) {
            string normalizedText = NormalizeText(line);
            int colIdx = loopIndex == 0 ? startingColIdx : 0;

            return new Tuple<TextPosition, string>(new TextPosition {
                Line = startingLineIdx + loopIndex,
                Column = colIdx + normalizedText.Length
            }, normalizedText);
        }

        private string NormalizeText(string text) {
            string normalizedText = text;

            if (text == TextConfiguration.NEWLINE) {
                normalizedText = string.Empty;
            } else if (text == TextConfiguration.TAB) {
                normalizedText = new string(' ', TextConfiguration.TabSize);
            }

            return normalizedText;
        }

        #endregion

    }
}
