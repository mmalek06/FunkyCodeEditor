using CodeEditor.Visuals;

namespace CodeEditor.Caching {
    internal abstract class CachedLine {

        #region properties

        public int Index { get; set; }

        public string RenderedContents { get; set; }

        #endregion

        #region public methods

        public abstract VisualTextLine ToVisualTextLine();

        #endregion

    }
}
