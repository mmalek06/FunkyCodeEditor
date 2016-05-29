using System;
using CodeEditor.Enums;

namespace CodeEditor.Algorithms.TextManipulation {
    internal static class CollapseRepresentationAlgorithmFactory {

        #region public methods

        public static ICollapseRepresentation GetAlgorithm(FormattingType formattingType) {
            switch (formattingType) {
                case FormattingType.BRACKETS:
                    return new BracketsRepresentationAlgorithm();
                case FormattingType.MARKUP:
                    return new MarkupRepresentationAlgorithm();
                case FormattingType.WHITESPACE:
                    return new WhitespaceRepresentationAlgorithm();
            }

            throw new ArgumentException("Unsupported formatting type");
        }

        #endregion

    }
}
