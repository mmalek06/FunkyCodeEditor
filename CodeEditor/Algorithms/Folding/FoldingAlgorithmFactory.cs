using System;
using CodeEditor.Enums;
using CodeEditor.Languages;

namespace CodeEditor.Algorithms.Folding {
    internal static class FoldingAlgorithmFactory {

        #region public methods

        public static IFoldingAlgorithm CreateAlgorithm(SupportedLanguages language) {
            var formattingType = LanguageFeatureInfo.GetFormattingType(language);

            switch (formattingType) {
                case FormattingType.BRACKETS:
                    return new BracketsFoldingAlgorithm();
                case FormattingType.MARKUP:
                    return new MarkupFoldingAlgorithm();
                case FormattingType.WHITESPACE:
                    return new WhitespaceFoldingAlgorithm();
            }

            throw new ArgumentException("Unsupported formatting type");
        }

        #endregion

    }
}
