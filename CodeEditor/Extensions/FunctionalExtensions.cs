using System;

namespace TextEditor.Extensions {
    internal static class FunctionalExtensions {
        public static TResult Map<TSource, TResult>(this TSource @this, Func<TSource, TResult> fn) => fn(@this);

        public static T Tee<T>(this T @this, Action<T> act) {
            act(@this);

            return @this;
        }

        public static T Tee<T>(this T @this, Action act) {
            act();

            return @this;
        }
    }
}
