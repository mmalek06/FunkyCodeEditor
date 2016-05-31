using CodeEditor.Visuals;

namespace CodeEditor.Caching {
    internal class CachedSingleLine : CachedLine {

        #region public methods

        public override VisualTextLine ToVisualTextLine() =>
            VisualTextLine.Create(RenderedContents, Index);

        #endregion

    }
}
