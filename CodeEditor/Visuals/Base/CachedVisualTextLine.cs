namespace CodeEditor.Visuals.Base {
    internal abstract class CachedVisualTextLine : VisualTextLine {

        #region public methods

        public override void Draw() { }

        public override CachedVisualTextLine ToCachedLine() => this;

        public abstract VisualTextLine ToVisualTextLine();

        #endregion

    }
}
