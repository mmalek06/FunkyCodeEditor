using System.Collections.Generic;
using CodeEditor.TextProperties;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Visuals {
    internal class CachedSingleVisualTextLine : CachedVisualTextLine {

        #region properties

        public override int Length => RenderedText.Length;

        public override string RenderedText { get; }

        #endregion

        #region constructor

        public CachedSingleVisualTextLine(string text, int index) {
            RenderedText = text;
            Index = index;
        }

        #endregion

        #region public methods

        public override IReadOnlyList<string> GetStringContents() => new[] { RenderedText };

        public override CharInfo GetCharInfoAt(int column) => new CharInfo { IsCharacter = true, Text = RenderedText[column].ToString() };

        public override VisualTextLine CloneWithIndexChange(int index) => new CachedSingleVisualTextLine(RenderedText, index);

        public override VisualTextLine ToVisualTextLine() => Create(RenderedText, Index);

        #endregion

    }
}
