using System.Collections.Generic;

namespace CodeEditor.Extensions {
    public static class HashSetExtensions {

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> elements) {
            foreach (var el in elements) {
                set.Add(el);
            }
        }

    }
}
