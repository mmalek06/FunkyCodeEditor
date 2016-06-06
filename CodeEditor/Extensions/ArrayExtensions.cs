namespace CodeEditor.Extensions {
    public static class ArrayExtensions {

        #region public methods

        public static void Populate<T>(this T[] arr, T value) {
            for (var i = 0; i < arr.Length; i++) {
                arr[i] = value;
            }
        }

        #endregion

    }
}
