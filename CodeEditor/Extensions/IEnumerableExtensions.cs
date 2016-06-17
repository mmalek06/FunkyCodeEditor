using System;
using System.Collections.Generic;
using System.Linq;
using CodeEditor.Visuals;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Extensions {
    internal static class IEnumerableExtensions {

        #region public methods

        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T defaultValue) {
            var item = enumerable.FirstOrDefault(predicate);

            if (EqualityComparer<T>.Default.Equals(item, defaultValue)) {
                return defaultValue;
            }

            return item;
        }

        public static IEnumerable<VisualTextLine> GetVisualLines(this IEnumerable<CachedVisualTextLine> cachedLines) =>
            cachedLines.Select(cachedLine => {
                var collapsedLine = cachedLine as CachedCollapsedVisualTextLine;
                var singleLine = cachedLine as CachedSingleVisualTextLine;

                if (collapsedLine != null) {
                    return collapsedLine.ToVisualTextLine();
                }
                if (singleLine == null) {
                    throw new ArgumentException(nameof(singleLine));
                }

                return singleLine.ToVisualTextLine();
            });

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => source.DistinctBy(keySelector, null);
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (keySelector == null) {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return DistinctByImpl(source, keySelector, comparer);
        }

        #endregion

        #region methods

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            source.GroupBy(keySelector, comparer).Select(g => g.First());

        #endregion

    }
}
