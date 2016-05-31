using System.Collections.Generic;
using System.Linq;
using CodeEditor.Caching;
using CodeEditor.Visuals;

namespace CodeEditor.Extensions {
    internal static class IListExtensions {

        #region public methods

        public static IEnumerable<VisualTextLine> GetVisualLines(this IList<CachedLine> cachedLines) =>
            cachedLines.Select(cachedLine => {
                var collapsedLine = cachedLine as CachedCollapsedLine;
                var singleLine = cachedLine as CachedSingleLine;

                if (collapsedLine != null) {
                    return GetCollapsedLine(collapsedLine);
                } else {
                    return GetSingleLine(singleLine);
                }
            });

        #endregion

        #region methods

        private static VisualTextLine GetSingleLine(CachedSingleLine line) =>
            VisualTextLine.Create(line.RenderedContents, line.Index);

        private static VisualTextLine GetCollapsedLine(CachedCollapsedLine line) =>
            VisualTextLine.Create(line.CollapsedContents, line.PrecedingText, line.FollowingText, line.Index, line.CollapseRepresentation);

        #endregion

    }
}
