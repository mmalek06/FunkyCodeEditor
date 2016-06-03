using System.Collections.Generic;
using CodeEditor.TextProperties;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Visuals {
    internal class CachedCollapsedVisualTextLine : CachedVisualTextLine {

        #region fields

        private string collapseRepresentation;

        private string textBeforeCollapse;

        private string textAfterCollapse;

        private List<string> collapsedContent;

        #endregion

        #region properties

        public override int Length => RenderedText.Length;

        public override string RenderedText => $"{textBeforeCollapse}{collapseRepresentation}{textAfterCollapse}";

        #endregion

        #region constructor

        public CachedCollapsedVisualTextLine(IEnumerable<string> contents, string precedingText, string followingText, int index, string collapseRepresentation) {
            collapsedContent = new List<string>(contents);
            textBeforeCollapse = precedingText;
            textAfterCollapse = followingText;
            Index = index;
            this.collapseRepresentation = collapseRepresentation;
        }

        #endregion

        #region public methods

        public override IReadOnlyList<string> GetStringContents() =>
            CollapseLineTraits.GetStringContents(textBeforeCollapse, textAfterCollapse, collapsedContent);

        public override CharInfo GetCharInfoAt(int column) =>
            CollapseLineTraits.GetCharInfoAt(column, textBeforeCollapse, textAfterCollapse, RenderedText, Index, collapseRepresentation);

        public override VisualTextLine CloneWithIndexChange(int index) => new CachedCollapsedVisualTextLine(collapsedContent, textBeforeCollapse, textAfterCollapse, Index, collapseRepresentation);

        public override VisualTextLine ToVisualTextLine() => Create(collapsedContent, textBeforeCollapse, textAfterCollapse, Index, collapseRepresentation);

        #endregion

    }
}
