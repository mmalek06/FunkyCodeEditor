using System;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.TextProperties {
    internal sealed class SimpleTextSource : TextSource {

        #region fields

        private StringBuilder sb;
        private readonly TextRunProperties properties;

        #endregion

        #region properties

        public string Text => sb.ToString();

        #endregion

        #region constructor

        public SimpleTextSource(string text, TextRunProperties properties) {
            sb = new StringBuilder(300).Append(text);
            this.properties = properties;
        }

        #endregion

        #region public methods

        public override TextRun GetTextRun(int textSourceCharacterIndex) {
            string text = sb.ToString();

            if (textSourceCharacterIndex < text.Length && textSourceCharacterIndex >= 0) {
                return new TextCharacters(text, textSourceCharacterIndex, text.Length - textSourceCharacterIndex, properties);
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

        public void Append(string text) => sb.Append(text);

        public void Replace(string newText) => sb.Clear().Append(newText);

        #endregion

    }
}
