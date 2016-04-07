using System.Collections.Generic;

namespace CodeEditor.Core.Extensions {
    public static class HashSetExtensions {

        #region public methods

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> elements) {
            foreach (var el in elements) {
                set.Add(el);
            }
        }

        #endregion

    }
}
