using System.Collections.Generic;
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

        public int Index { get; set; }

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

        public abstract void Redraw();

        public static VisualTextLine Create(string text, int index) {
            return Create(new SimpleTextSource(text, TextConfiguration.GetGlobalTextRunProperties()), index);
        }

        public static VisualTextLine Create(SimpleTextSource textSource, int index) {
            return new SingleVisualTextLine(textSource, index);
        }

        public static VisualTextLine Create(IEnumerable<SimpleTextSource> textSourcesToCollapse, string precedingText, string followingText, int index) {
            var textSourceBeforeCollapse = new SimpleTextSource(precedingText, TextConfiguration.GetGlobalTextRunProperties());
            var textSourceAfterCollapse = new SimpleTextSource(followingText, TextConfiguration.GetGlobalTextRunProperties());

            return Create(textSourcesToCollapse, textSourceBeforeCollapse, textSourceAfterCollapse, index);
        }

        public static VisualTextLine Create(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index) {
            return new CollapsedVisualTextLine(textSourcesToCollapse, precedingSource, followingSource, index);
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
