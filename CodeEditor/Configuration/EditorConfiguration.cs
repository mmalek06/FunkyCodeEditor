﻿using System.Windows.Media;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Extensions;

namespace CodeEditor.Configuration {
    internal static class EditorConfiguration {

        public static int GetLinesColumnWidth() => ((int)StringExtensions.GetCharWidth()) * 4;

        public static int GetFoldingColumnWidth() => ((int)StringExtensions.GetCharWidth()) * 2;

        public static int GetTextAreaLeftMargin() => GetLinesColumnWidth() + GetFoldingColumnWidth();

        public static Brush GetLinesColumnBrush() {
            return Brushes.Black;
        }

        public static Brush GetLinesColumnFontColor() {
            return Brushes.White;
        }

        public static double GetFontHeight() => StringExtensions.GetCharSize().Height;

        public static Typeface GetTypeface() => 
            new Typeface(TextConfiguration.GetFontFamily(), TextConfiguration.GetFontStyle(), TextConfiguration.GetFontWeight(), TextConfiguration.GetFontStretch());

        public static IFoldingAlgorithm GetFoldingAlgorithm() => new BracketsFoldingAlgorithm();

    }
}
