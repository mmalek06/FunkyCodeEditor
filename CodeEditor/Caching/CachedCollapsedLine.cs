using System.Collections.Generic;
using CodeEditor.Visuals;

namespace CodeEditor.Caching {
    internal class CachedCollapsedLine : CachedLine {

        #region properties

        public string CollapseRepresentation { get; set; }

        public string PrecedingText { get; set; }

        public string FollowingText { get; set; }

        public IEnumerable<string> CollapsedContents { get; set; }

        #endregion

        #region public methods

        public override VisualTextLine ToVisualTextLine() =>
            VisualTextLine.Create(CollapsedContents, PrecedingText, FollowingText, Index, CollapseRepresentation);

        #endregion

    }
}
