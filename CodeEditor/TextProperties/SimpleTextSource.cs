using System;
using System.Windows.Media.TextFormatting;

namespace TextEditor.TextProperties {
    internal sealed class SimpleTextSource : TextSource {
        public string Text { get; set; }
        private readonly TextRunProperties properties;

        public SimpleTextSource(string text, TextRunProperties properties) {
            Text = text;
            this.properties = properties;
        }

        public override TextRun GetTextRun(int textSourceCharacterIndex) {
            if (textSourceCharacterIndex < Text.Length) {
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
    }
}
