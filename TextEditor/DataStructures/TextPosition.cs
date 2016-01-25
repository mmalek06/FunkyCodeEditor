namespace TextEditor.DataStructures {
    public struct TextPosition {
        public int Column { get; set; }
        public int Line { get; set; }

        public TextPosition(int column, int line) {
            Column = column;
            Line = line;
        }
    }
}
