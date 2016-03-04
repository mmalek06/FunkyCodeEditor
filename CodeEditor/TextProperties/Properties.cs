namespace CodeEditor.TextProperties {
    public static class Properties {

        #region constants

        public const string NEWLINE = "\r";
        public const string BACKSPACE = "\b";
        public const string TAB = "\t";

        #endregion

        #region public properties

        public static int TabSize { get; set; }

        #endregion

        #region constructor

        static Properties() {
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
