namespace CodeEditor.TextProperties {
    public static class TextConfiguration {

        #region constants

        public const string NEWLINE = "\r";
        public const string BACKSPACE = "\b";
        public const string TAB = "\t";

        #endregion

        #region public properties

        public static int TabSize { get; set; }

        #endregion

        #region constructor

        static TextConfiguration() {
            TabSize = GetTabSize();
        }

        #endregion

        #region methods

        private static int GetTabSize() {
            return 4;
        }

        #endregion

    }
}
