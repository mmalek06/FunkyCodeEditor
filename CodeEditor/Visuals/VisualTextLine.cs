﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.TextProperties;

namespace CodeEditor.Visuals {
    internal abstract class VisualTextLine : DrawingVisual {

        #region fields

        private static TextFormatter formatter;

        private static SimpleParagraphProperties paragraphProperties;

        #endregion

        #region properties

        public abstract int Length { get; }

        public abstract string RenderedText { get; }

        public int Index { get; protected set; }

        protected TextFormatter Formatter => formatter;

        protected SimpleParagraphProperties ParagraphProperties => paragraphProperties;

        #endregion

        #region constructors

        static VisualTextLine() {
            formatter = TextFormatter.Create();
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = TextConfiguration.GetGlobalTextRunProperties() };
        }

        #endregion

        #region public methods

        public abstract void Draw();

        public abstract IReadOnlyList<SimpleTextSource> GetTextSources();

        public abstract IReadOnlyList<string> GetStringContents();

        public abstract CharInfo GetCharInfoAt(int column);

        public abstract VisualTextLine CloneWithIndexChange(int index);

        public override string ToString() => $"{GetType().ToString()}, {RenderedText}, {Index}";

        public static VisualTextLine Create(string text, int index) {
            return new SingleVisualTextLine(new SimpleTextSource(text, TextConfiguration.GetGlobalTextRunProperties()), index);
        }

        public static VisualTextLine Create(IEnumerable<string> linesToCollapse, string precedingText, string followingText, int index, string collapseRepresentation) {
            var textSourceBeforeCollapse = new SimpleTextSource(precedingText, TextConfiguration.GetGlobalTextRunProperties());
            var textSourceAfterCollapse = new SimpleTextSource(followingText, TextConfiguration.GetGlobalTextRunProperties());

            return new CollapsedVisualTextLine(
                linesToCollapse.Select(line => new SimpleTextSource(line, TextConfiguration.GetGlobalTextRunProperties())), textSourceBeforeCollapse, textSourceAfterCollapse, index, collapseRepresentation);
        }

        #endregion

        #region methods

        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }

        #endregion

    }
}
