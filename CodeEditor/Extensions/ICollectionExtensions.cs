using System.Collections.Generic;

namespace CodeEditor.Extensions {
    public static class ICollectionExtensions {

        #region public methods

        public static bool AddIfNotNull<T>(this ICollection<T> collection, T element) {
            if (element == null) {
                return false;
            }

            collection.Add(element);

            return true;
        }

        #endregion

    }
}
