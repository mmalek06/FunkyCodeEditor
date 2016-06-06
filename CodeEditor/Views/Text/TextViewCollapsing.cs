using System.Linq;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Configuration;
using CodeEditor.Enums;
using CodeEditor.Extensions;
using CodeEditor.Messaging;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region event handlers

        public override void HandleTextFolding(FoldClickedMessage message) {
            if (message.State == FoldingStates.EXPANDED) {
                ExpandText(message);
            } else {
                CollapseText(message);
            }
        }

        #endregion

        #region public methods

        public void CollapseText(FoldClickedMessage message) {
            var algorithm = CollapseRepresentationAlgorithmFactory.GetAlgorithm(ConfigManager.GetConfig(EditorCode).FormattingType);
            var collapsedLine = collapsingAlgorithm.CollapseTextRange(message.AreaBeforeFolding, GetScreenLines(), algorithm);
            var linesToRedraw = collapsingAlgorithm.GetLinesToRedrawAfterCollapse(visuals.ToEnumerableOf<VisualTextLine>().ToList(), collapsedLine, message.AreaBeforeFolding);

            if (message.AreaBeforeFolding.StartPosition.Line != message.AreaBeforeFolding.EndPosition.Line) {
                visuals.RemoveRange(message.AreaBeforeFolding.StartPosition.Line, visuals.Count - (message.AreaBeforeFolding.StartPosition.Line + 1));
            }

            RedrawCollapsedLine(collapsedLine, message.AreaBeforeFolding.StartPosition.Line);
            AddLines(linesToRedraw);
            UpdateSize();
        }

        public void ExpandText(FoldClickedMessage message) {
            var collapseIndex = message.AreaBeforeFolding.StartPosition.Line;
            var collapsedLineContent = ((VisualTextLine)visuals[collapseIndex]).GetStringContents();
            var expandedLines = collapsedLineContent.Select((line, index) => VisualTextLine.Create(line, collapseIndex + index)).ToArray();
            var linesToRedraw = collapsingAlgorithm.GetLinesToRedrawAfterExpand(visuals.ToEnumerableOf<VisualTextLine>().Where(line => line.Index > message.AreaBeforeFolding.StartPosition.Line), expandedLines.Length - 1);

            visuals.RemoveRange(collapseIndex, LinesCount - collapseIndex);

            foreach (var line in expandedLines) {
                visuals.Insert(line.Index, line);
                line.Draw();
            }

            AddLines(linesToRedraw);
            UpdateSize();
        }

        #endregion

    }
}
