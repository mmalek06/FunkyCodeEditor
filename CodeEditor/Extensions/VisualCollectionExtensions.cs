using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using CodeEditor.Visuals;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Extensions {
    internal static class VisualCollectionExtensions {

        #region public methods

        public static void InsertRange(this VisualCollection collection, IEnumerable<Visual> visuals, int index) {
            foreach (var visual in visuals) {
                collection.Insert(index, visual);

                index++;
            }
        }

        public static void AddRange(this VisualCollection collection, IEnumerable<Visual> visuals) {
            foreach (var visual in visuals) {
                collection.Add(visual);
            }
        }

        public static IEnumerable<T> ToEnumerableOf<T>(this VisualCollection collection) where T : class {
            var contents = new List<T>();

            foreach (var element in collection) {
                contents.AddIfNotNull(element as T);
            }

            return contents;
        }

        public static IList<CachedVisualTextLine> ConvertToCachedLines(this VisualCollection collection, int count, int omit = 0) {
            var fragment = collection.ToEnumerableOf<VisualTextLine>().Skip(omit).Take(count);

            return fragment.Select(line => {
                var collapsedLine = line as CollapsedVisualTextLine;
                var singleLine = line as SingleVisualTextLine;

                if (collapsedLine != null) {
                    return collapsedLine.ToCachedLine();
                }
                if (singleLine == null) {
                    throw new ArgumentException(nameof(line));
                }

                return singleLine.ToCachedLine();
            }).ToList();
        }

        #endregion

    }
}
