using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Selection;
using CodeEditor.Views.Text;

namespace CodeEditor.Tests.ScenarioTests.Views {
    [TestClass]
    public class TextViewSelectionScenarioTests {
        private TextView tv;
        private SelectionView sv;
        private CaretView cv;

        [TestInitialize]
        public void InitializeTests() {
            cv = new CaretView();
            tv = new TextView(cv);
            sv = new SelectionView(tv, cv);
        }

        [TestMethod]
        public void EnterOneLineSelectFourCharsInOneLineToTheRight_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var startingPosition = new TextPosition(line: 0, column: 4);
            var endingPosition = new TextPosition(line: 0, column: 8);

            tv.EnterText(text1 + text2 + text3);
            sv.Select(startingPosition);
            sv.Select(endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = tv.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }

        [TestMethod]
        public void EnterOneLineSelectFourCharsInOneLineToTheLeft_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var startingPosition = new TextPosition(line: 0, column: 8);
            var endingPosition = new TextPosition(line: 0, column: 4);

            tv.EnterText(text1 + text2 + text3);
            sv.Select(startingPosition);
            sv.Select(endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = tv.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }
              
        [TestMethod]
        public void EnterTwoLinesSelectTwoOfFourLinesBottomToTop_CursorPositionIsAtTheEndOfText1() {
            string text1 = "shop";
            string text2 = "Where she sits she shines, and where she shines she sits.";
            var startingPosition = new TextPosition(line: 1, column: text2.Length);
            var endingPosition = new TextPosition(line: 0, column: text1.Length);

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            sv.Select(startingPosition);
            sv.Select(endingPosition);
            cv.MoveCursor(endingPosition);

            Assert.AreEqual(0, cv.CaretPosition.Line);
            Assert.AreEqual(text1.Length, cv.CaretPosition.Column);
        }

        [TestMethod]
        public void EnterTwoLinesSelectTwoOfFourLinesTopToBottom_CursorPositionIsAtTheEndOfText2() {
            string text1 = "Where she sits she shines, and where she shines she sits.";
            string text2 = "shop";
            var startingPosition = new TextPosition(line: 0, column: text1.Length);
            var endingPosition = new TextPosition(line: 1, column: text2.Length);

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            sv.Select(startingPosition);
            sv.Select(endingPosition);
            cv.MoveCursor(endingPosition);

            Assert.AreEqual(1, cv.CaretPosition.Line);
            Assert.AreEqual(text2.Length, cv.CaretPosition.Column);
        }

    }
}
