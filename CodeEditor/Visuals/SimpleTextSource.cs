using System;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal sealed class SimpleTextSource : TextSource {

        #region fields

        private readonly TextRunProperties properties;

        #endregion

        #region properties

        public string Text { get; set; }

        #endregion

        #region constructor

        public SimpleTextSource(string text, TextRunProperties properties) {
            Text = text;
            this.properties = properties;
        }

        #endregion

        #region public methods

        public override TextRun GetTextRun(int textSourceCharacterIndex) {
            if (textSourceCharacterIndex < Text.Length && textSourceCharacterIndex >= 0) {
                return new TextCharacters(Text, textSourceCharacterIndex, Text.Length - textSourceCharacterIndex, properties);
            } else {
                return new TextEndOfParagraph(1);
            }
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex) {
            throw new NotImplementedException();
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
