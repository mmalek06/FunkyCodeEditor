using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using CodeEditor.Caching;
using CodeEditor.Visuals;

namespace CodeEditor.Extensions {
    internal static class VisualCollectionExtensions {

        #region public methods

        public static IEnumerable<T> ToEnumerableOf<T>(this VisualCollection collection) where T : class {
            var contents = new List<T>();

            foreach (var element in collection) {
                contents.AddIfNotNull(element as T);
            }

            return contents;
        }

        public static IList<CachedLine> ConvertToCachedLines(this VisualCollection collection, int omit, int count) {
            var result = new List<CachedLine>();
            var fragment = collection.ToEnumerableOf<VisualTextLine>().Skip(omit).Take(count);

            return fragment.Select(line => {
                var collapsedLine = line as CollapsedVisualTextLine;
                var singleLine = line as SingleVisualTextLine;

                if (collapsedLine != null) {
                    return GetCachedCollapsedLine(collapsedLine);
                } else {
                    return GetCachedSingleLine(singleLine);
                }
            }).ToList();
        }

        #endregion

        #region methods

        private static CachedLine GetCachedSingleLine(SingleVisualTextLine singleLine) =>
            new CachedSingleLine {
                Index = singleLine.Index,
                RenderedContents = singleLine.RenderedText
            };

        private static CachedLine GetCachedCollapsedLine(CollapsedVisualTextLine collapsedLine) =>
            new CachedCollapsedLine {
                Index = collapsedLine.Index,
                RenderedContents = collapsedLine.RenderedText,
                CollapsedContents = collapsedLine.CollapsedContent,
                CollapseRepresentation = collapsedLine.CollapseRepresentation,
                PrecedingText = GetPrecedingText(collapsedLine),
                FollowingText = GetFollowingText(collapsedLine)
            };

        private static string GetPrecedingText(CollapsedVisualTextLine line) {
            var text = line.RenderedText;
            var parts = text.Split(new[] { line.CollapseRepresentation }, StringSplitOptions.None);

            return parts[0];
        }

        private static string GetFollowingText(CollapsedVisualTextLine line) {
            var text = line.RenderedText;
            var parts = text.Split(new[] { line.CollapseRepresentation }, StringSplitOptions.None);

            return parts.Last();
        }

        #endregion

    }
}
