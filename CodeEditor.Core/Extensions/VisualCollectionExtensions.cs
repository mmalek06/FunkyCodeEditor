﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace CodeEditor.Core.Extensions {
    public static class VisualCollectionExtensions {

        #region public methods

        public static IEnumerable<T> ToEnumerableOf<T>(this VisualCollection collection) where T : class {
            var contents = new List<T>();

            foreach (var element in collection) {
                contents.AddIfNotNull(element as T);
            }

            return contents;
        }

        #endregion

    }
}