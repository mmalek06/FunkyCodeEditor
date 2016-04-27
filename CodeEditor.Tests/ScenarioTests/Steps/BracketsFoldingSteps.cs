using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps {
    [Binding]
    public class BracketsFoldingSteps {

        #region steps

        [When(@"I request folding for position starting at '(.*)' '(.*)' and ending at '(.*)' '(.*)'")]
        public void WhenIRequestFoldingForPositionStartingAtAndEndingAt(int columnStart, int lineStart, int columnEnd, int lineEnd) {
            var message = GetBracketFoldClickedMessage(columnStart, lineStart, columnEnd, lineEnd, FoldingStates.FOLDED);

            Common.Context.TextView.HandleTextFolding(message);
            Postbox.Instance.Send(message);
        }

        [When(@"I request unfolding for position starting at '(.*)' '(.*)' and ending at '(.*)' '(.*)'")]
        public void WhenIRequestUnfoldingForPositionStartingAtAndEndingAt(int columnStart, int lineStart, int columnEnd, int lineEnd) {
            var message = GetBracketFoldClickedMessage(columnStart, lineStart, columnEnd, lineEnd, FoldingStates.EXPANDED);

            Common.Context.TextView.HandleTextFolding(message);
            Postbox.Instance.Send(message);
        }

        #endregion

        #region helpers

        private FoldClickedMessage GetBracketFoldClickedMessage(int startingCol, int startingLine, int endingCol, int endingLine, FoldingStates state) {
            return new FoldClickedMessage {
                State = state,
                AreaBeforeFolding = new TextRange {
                    StartPosition = new TextPosition(startingCol, startingLine),
                    EndPosition = new TextPosition(endingCol, endingLine)
                },
                OpeningTag = "{",
                ClosingTag = "}"
            };
        }

        #endregion

    }
}
