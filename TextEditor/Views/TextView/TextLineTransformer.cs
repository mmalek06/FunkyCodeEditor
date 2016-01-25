using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TextEditor.DataStructures;
using TextEditor.TextProperties;

namespace TextEditor.Views.TextView {
    internal class TextLineTransformer {

        #region fields

        private IList<SimpleTextSource> textSources;

        #endregion

        #region public properties

        public Regex SpecialCharsRegex { get; private set; }

        #endregion

        #region constructor

        public TextLineTransformer(IList<SimpleTextSource> textSources) {
            this.textSources = textSources;
            SpecialCharsRegex = new Regex("[\a|\b|\n|\r|\f|\t|\v]");
        }

        #endregion

        #region public methods

        public Dictionary<TextPosition, string> CreateLines(string text, int startingLineIdx, int startingColIdx) {
            var replacedText = SpecialCharsRegex.Replace(text, string.Empty);

            if (text == TextConfiguration.NEWLINE) {
                return LineAdded(text, startingLineIdx, startingColIdx);
            } else if (replacedText.Length == 1) {
                return CharacterEntered(replacedText, startingLineIdx, startingColIdx);
            } else {
                return TextPasted(text, startingLineIdx, startingColIdx);
            }
        }

        #endregion

        #region methods

        private Dictionary<TextPosition, string> LineAdded(string text, int startingLineIdx, int startingColIdx) {
            string textBeforeCursorPosition = string.Concat(textSources[startingLineIdx].Text.Take(startingColIdx));
            string textAfterCursorPosition = string.Concat(textSources[startingLineIdx].Text.Skip(startingColIdx));
            var transformations = new Dictionary<TextPosition, string> {
                [new TextPosition { Column = startingColIdx, Line = startingLineIdx }] = textBeforeCursorPosition,
                [new TextPosition { Column = 0, Line = startingLineIdx + 1 }] = textAfterCursorPosition
            };

            for (int i = startingLineIdx + 1; i < textSources.Count; i++) {
                transformations[new TextPosition { Column = 0, Line = i + 1 }] = textSources[i].Text;
            }

            return transformations;
        }

        private Dictionary<TextPosition, string> CharacterEntered(string text, int startingLineIdx, int startingColIdx) {
            string currentLineText = string.Empty;

            if (textSources.Any()) {
                currentLineText = textSources[startingLineIdx].Text;

                if (startingColIdx >= currentLineText.Length) {
                    currentLineText += text;
                } else {
                    currentLineText.Insert(startingColIdx, text);
                }
            } else {
                currentLineText = text;
            }

            return new Dictionary<TextPosition, string> {
                [new TextPosition { Column = startingColIdx + 1, Line = startingLineIdx }] = currentLineText
            };
        }

        private Dictionary<TextPosition, string> TextPasted(string text, int startingLineIdx, int startingColIdx) =>
            text.Split(TextConfiguration.NEWLINE[0])
                .Select((line, index) => GetPositionWithText(line, index, startingLineIdx, startingColIdx))
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
