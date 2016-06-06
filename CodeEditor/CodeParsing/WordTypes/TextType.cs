namespace CodeEditor.CodeParsing.WordTypes {
    internal enum TextType {
        // eg "word", 1, 2.897
        STRING,
        NUMBER,
        COLLAPSE,
        KEYWORD,
        TYPE,
        // everything else
        STANDARD
    }
}
