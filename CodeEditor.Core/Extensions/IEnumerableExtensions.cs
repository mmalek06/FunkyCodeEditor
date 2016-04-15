using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeEditor.Core.Extensions {
    public static class IEnumerableExtensions {

        #region public methods

        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T defaultValue) {
            var item = enumerable.FirstOrDefault(predicate);

            if (EqualityComparer<T>.Default.Equals(item, defaultValue)) {
                return defaultValue;
            }

            return item;
        }

        #endregion

    }
}
