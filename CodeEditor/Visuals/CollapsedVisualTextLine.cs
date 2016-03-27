﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;

namespace CodeEditor.Visuals {
    internal class CollapsedVisualTextLine : VisualTextLine {

        #region fields

        private string textBeforeCollapse;

        private string textAfterCollapse;

        private List<string> collapsedContent;

        #endregion

        #region properties

        public override string Text {
            get { return $"{textBeforeCollapse} {textAfterCollapse}"; }
            protected set { base.Text = value; }
        }

        public override int Length => Text.Length;

        #endregion

        #region constructor

        public CollapsedVisualTextLine(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index) {
            collapsedContent = new List<string>(textSourcesToCollapse.Select(source => source.Text));
            textBeforeCollapse = precedingSource.Text;
            textAfterCollapse = followingSource.Text;
            Index = index;

            Redraw();
        }

        #endregion

        #region public methods

        public override void UpdateText(string text) { }

        public override void Redraw() {
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();
            double top = 0;
            string collapse = "{...}";

            using (var drawingContext = RenderOpen()) {
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(textBeforeCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    top = Index * textLine.Height;

                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(collapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {    
                    textLine.Draw(drawingContext, new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length, top), InvertAxes.None);
                }
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(textAfterCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    textLine.Draw(drawingContext, 
                        new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length + TextConfiguration.GetCharSize().Width * collapse.Length, top), 
                        InvertAxes.None);
                }
            }
        }

        public override IEnumerable<string> GetStringContents() {
            var contents = new List<string>();

            contents.Add(textBeforeCollapse + collapsedContent[0]);
            contents.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 2)
                              select text);
            contents.Add(collapsedContent.Last() + textAfterCollapse);

            return contents;
        }

        public override IEnumerable<SimpleTextSource> GetTextSources() {
            var textSources = new List<SimpleTextSource>();
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();

            textSources.Add(new SimpleTextSource(textBeforeCollapse + collapsedContent[0], runProperties));
            textSources.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 2)
                                 select new SimpleTextSource(text, runProperties));
            textSources.Add(new SimpleTextSource(collapsedContent.Last() + textAfterCollapse, runProperties));

            return textSources;
        }

        #endregion

    }
}
