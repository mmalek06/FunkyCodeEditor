using System.Collections.Generic;
using CodeEditor.Commands;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace CodeEditor.Tests.ScenarioTests.Steps
{
    [Binding]
    public class BreakNonEmptyLineSteps {

        #region fields

        private List<string> textsToEnter;

        private TextView tv;

        private CaretView cv;

        private SelectionView sv;

        private TextView.TextViewInfo ti;

        private EnterTextCommand etc;

        private CaretMoveCommand cmc;

        #endregion

        #region constructor

        public BreakNonEmptyLineSteps() {
            textsToEnter = new List<string>();
            tv = new TextView();
            ti = TextView.TextViewInfo.GetInstance(tv);
            cv = new CaretView();
            sv = new SelectionView(ti);
            etc = new EnterTextCommand(tv, sv, ti);
            cmc = new CaretMoveCommand(cv, ti);
        }

        #endregion

        #region steps

        [Given(@"Text to enter is '(.*)'")]
        public void GivenTextToEnterIs(string textToEnter) {
            textsToEnter.Add(textToEnter);
        }
        
        [When(@"I enter text")]
        public void WhenIEnterText() {
            foreach (string text in textsToEnter) {
                var evtArgs = EventGenerator.CreateTextCompositionEventArgs(text);

                if (etc.CanExecute(evtArgs)) {
                    etc.Execute(evtArgs);
                }
            }
        }
        
        [When(@"I move caret to column number '(.*)' in line '(.*)'")]
        public void WhenIMoveCaretToColumnNumberInLine(int column, int line) {
            var evtArgs = EventGenerator.CreateCaretMovedEventArgs(column, line);

            if (cmc.CanExecute(evtArgs)) {
                cmc.Execute(evtArgs);
            }
        }
        
        [When(@"I hit enter key")]
        public void WhenIHitEnterKey() {
            var evtArgs = EventGenerator.CreateTextCompositionEventArgs("\r");

            if (etc.CanExecute(evtArgs)) {
                etc.Execute(evtArgs);
            }
        }
        
        [Then(@"I should see '(.*)' lines")]
        public void ThenIShouldSeeLines(int numberOfVisibleLines) {
            var actualLinesCount = ti.GetScreenLines().Count;

            Assert.AreEqual(numberOfVisibleLines, actualLinesCount);
        }
        
        [Then(@"The '(.*)' line should be equal to '(.*)'")]
        public void ThenTheLineShouldBeEqualTo(int lineNo, string text) {
            var actualLines = ti.GetScreenLines();

            Assert.AreEqual(textsToEnter[lineNo], text);
        }

        #endregion

    }
}
