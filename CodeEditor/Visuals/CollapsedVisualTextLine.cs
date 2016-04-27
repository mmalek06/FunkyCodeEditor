﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.TextProperties;

namespace CodeEditor.Visuals {
    internal class CollapsedVisualTextLine : VisualTextLine {

        #region fields

        private string collapseRepresentation;

        private string textBeforeCollapse;

        private string textAfterCollapse;

        private List<string> collapsedContent;

        #endregion

        #region properties

        public override string RenderedText => $"{textBeforeCollapse}{collapseRepresentation}{textAfterCollapse}";

        public override int Length => RenderedText.Length;

        public string CollapseRepresentation => collapseRepresentation;

        #endregion

        #region constructor

        public CollapsedVisualTextLine(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index, string collapseRepresentation) {
            collapsedContent = new List<string>(textSourcesToCollapse.Select(source => source.Text));
            textBeforeCollapse = precedingSource.Text;
            textAfterCollapse = followingSource.Text;
            Index = index;
            this.collapseRepresentation = collapseRepresentation;
        }

        #endregion

        #region public methods

        public override void Draw() {
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();
            double top = 0;

            using (var drawingContext = RenderOpen()) {
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(textBeforeCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    top = Index * textLine.Height;

                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(collapseRepresentation, runProperties), 0, 96 * 6, ParagraphProperties, null)) {    
                    textLine.Draw(drawingContext, new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length, top), InvertAxes.None);
                }
                using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource(textAfterCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    textLine.Draw(drawingContext, 
                        new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length + TextConfiguration.GetCharSize().Width * collapseRepresentation.Length, top), 
                        InvertAxes.None);
                }
            }
        }

        public override IReadOnlyList<string> GetStringContents() {
            var contents = new List<string>();

            contents.Add(textBeforeCollapse + collapsedContent[0]);
            contents.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 1)
                              select text);
            contents[contents.Count - 1] += textAfterCollapse;

            return contents;
        }

        public override IReadOnlyList<SimpleTextSource> GetTextSources() {
            var textSources = new List<SimpleTextSource>();
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();

            textSources.Add(new SimpleTextSource(textBeforeCollapse + collapsedContent[0], runProperties));
            textSources.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 1)
                                 select new SimpleTextSource(text, runProperties));
            textSources[textSources.Count - 1].Text += textAfterCollapse;

            return textSources;
        }

        public override CharInfo GetCharInfoAt(int column) {
            if (column < textBeforeCollapse.Length) {
                return new CharInfo {
                    IsCharacter = true,
                    Character = textBeforeCollapse[column],
                    PrevCharPosition = new Core.DataStructures.TextPosition(column: column, line: Index),
                    NextCharPosition = new Core.DataStructures.TextPosition(column: column, line: Index)
                };
            }
            if (column > textBeforeCollapse.Length && column >= $"{textBeforeCollapse}{collapseRepresentation}".Length) {
                return new CharInfo {
                    IsCharacter = true,
                    Character = RenderedText[column],
                    PrevCharPosition = new Core.DataStructures.TextPosition(column: column, line: Index),
                    NextCharPosition = new Core.DataStructures.TextPosition(column: column, line: Index)
                };
            }

            return new CharInfo {
                IsCharacter = false,
                PrevCharPosition = new Core.DataStructures.TextPosition(column: textBeforeCollapse.Length, line: Index),
                NextCharPosition = new Core.DataStructures.TextPosition(column: $"{textBeforeCollapse}{collapseRepresentation}".Length, line: Index)
            };
        }

        public override VisualTextLine CloneWithIndexChange(int index) => Create(collapsedContent, textBeforeCollapse, textAfterCollapse, index, collapseRepresentation);

        #endregion

    }
}
