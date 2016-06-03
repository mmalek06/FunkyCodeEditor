using System.Collections.Generic;
using System.Linq;
using CodeEditor.TextProperties;

namespace CodeEditor.Visuals.Base {
    internal static class CollapseLineTraits {

        #region public methods

        public static IReadOnlyList<string> GetStringContents(string textBeforeCollapse, string textAfterCollapse, IReadOnlyList<string> collapsedContent) {
            var contents = new List<string> {
                textBeforeCollapse + (collapsedContent.Any() ? collapsedContent[0] : string.Empty)
            };

            contents.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 1)
                              select text);
            contents[contents.Count - 1] += textAfterCollapse;

            return contents;
        } 

        public static CharInfo GetCharInfoAt(int column, string textBeforeCollapse, string textAfterCollapse, string renderedText, int index, string collapseRepresentation) {
            if (column < textBeforeCollapse.Length) {
                return new CharInfo {
                    IsCharacter = true,
                    Text = textBeforeCollapse[column].ToString(),
                    PrevCharPosition = new DataStructures.TextPosition(column: column, line: index),
                    NextCharPosition = new DataStructures.TextPosition(column: column, line: index)
                };
            }
            if (column > textBeforeCollapse.Length && column >= $"{textBeforeCollapse}{collapseRepresentation}".Length && column < renderedText.Length) {
                return new CharInfo {
                    IsCharacter = true,
                    Text = renderedText[column].ToString(),
                    PrevCharPosition = new DataStructures.TextPosition(column: column, line: index),
                    NextCharPosition = new DataStructures.TextPosition(column: column, line: index)
                };
            }

            return new CharInfo {
                IsCharacter = false,
                Text = collapseRepresentation,
                PrevCharPosition = new DataStructures.TextPosition(column: textBeforeCollapse.Length, line: index),
                NextCharPosition = new DataStructures.TextPosition(column: $"{textBeforeCollapse}{collapseRepresentation}".Length, line: index)
            };
        }

        #endregion

    }
}
