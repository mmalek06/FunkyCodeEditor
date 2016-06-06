﻿using System.Collections.Generic;
using System.Windows.Media;
using CodeEditor.CodeParsing;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Enums;

namespace CodeEditor.Configuration {
    internal static class SharedEditorConfiguration {

        #region properties

        public static int GetLinesColumnWidth() => ((int)TextConfiguration.GetCharSize().Width) * 5;

        public static int GetFoldingColumnWidth() => ((int)TextConfiguration.GetCharSize().Width) * 3;

        public static int GetTextAreaLeftMargin() => GetLinesColumnWidth() + GetFoldingColumnWidth();

        public static Brush GetLinesColumnBrush() {
            return Brushes.Black;
        }

        public static Brush GetLinesColumnFontColor() {
            return Brushes.White;
        }

        public static Brush GetFoldingColumnBrush() {
            return GetEditorBrush();
        }

        public static IEnumerable<IWordType> GetWordParsers(SupportedLanguages language, IDefinitionLoader definitionLoader) =>
            new List<IWordType> {
                new CollapseWordType { Language = language },
                new TypeWordType { Language = language },
                new KeywordWordType { Language = language },
                new StdWordType { Language = language },
                new StringWordType { Language = language }
            };

        public static Brush GetFoldingColumnFontColor() {
            return Brushes.LightBlue;
        }

        public static Brush GetEditorBrush() {
            return Brushes.LightGray;
        }

        public static Typeface GetTypeface() =>
            new Typeface(TextConfiguration.GetFontFamily(), TextConfiguration.GetFontStyle(), TextConfiguration.GetFontWeight(), TextConfiguration.GetFontStretch());

        #endregion

    }
}
