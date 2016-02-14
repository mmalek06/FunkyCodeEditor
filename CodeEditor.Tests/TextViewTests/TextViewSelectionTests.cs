using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CodeEditor.Tests.TextViewTests {
    [TestClass]
    public class TextViewSelectionTests {
        private Views.TextView.View tv;
        private Views.SelectionView.View sv;
        private Views.TextView.TextInfo ti;
        private Views.CaretView.View cv;

        [TestInitialize]
        public void InitializeTests() {
            tv = new Views.TextView.View();
            ti = new Views.TextView.TextInfo(tv);
            sv = new Views.SelectionView.View(ti);
            cv = new Views.CaretView.View(ti);
        }

        [TestMethod]
        public void SelectFourCharsInOneLineToTheRight_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var startingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = 4
            };
            var endingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = 8
            };

            tv.EnterText(text1 + text2 + text3);
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition);
            sv.Select(endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = ti.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }

        [TestMethod]
        public void SelectFourCharsInOneLineToTheLeft_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var startingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = 8
            };
            var endingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = 4
            };

            tv.EnterText(text1 + text2 + text3);
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition);
            sv.Select(endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = ti.GetTextPartsBetweenPositions(selectionArea.EndPosition, selectionArea.StartPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }

        [TestMethod]
        public void SelectMultipleCharsInTwoLines_TextRangeIsText2Text3() {
            string text1 = "I";
            string text2 = " saw Susie sitting in a shoe shine shop.";
            string text3 = "Where she ";
            string text4 = "sits she shines, and where she shines she sits.";
            var startingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = 1
            };
            var endingPosition = new DataStructures.TextPosition {
                Line = 1,
                Column = 10
            };

            tv.EnterText(text1 + text2);
            tv.EnterText("\r");
            tv.EnterText(text3 + text4);
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition);
            sv.Select(endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = ti.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
            Assert.AreEqual(text3, selectedParts.Last());
        }
              
        [TestMethod]
        public void SelectTwoOfFourLinesBottomToTop_CursorPositionIsAtTheEndOfText1() {
            string text1 = "shop";
            string text2 = "Where she sits she shines, and where she shines she sits.";
            var startingPosition = new DataStructures.TextPosition {
                Line = 1,
                Column = text2.Length
            };
            var endingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = text1.Length
            };

            cv.CaretMoved += tv.HandleCaretMove;

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            sv.Select(startingPosition);
            sv.Select(endingPosition);
            cv.MoveCursor(endingPosition);

            Assert.AreEqual(0, tv.ActivePosition.Line);
            Assert.AreEqual(text1.Length, tv.ActivePosition.Column);
        }

        [TestMethod]
        public void SelectTwoOfFourLinesTopToBottom_CursorPositionIsAtTheEndOfText2() {
            string text1 = "Where she sits she shines, and where she shines she sits.";
            string text2 = "shop";
            var startingPosition = new DataStructures.TextPosition {
                Line = 0,
                Column = text1.Length
            };
            var endingPosition = new DataStructures.TextPosition {
                Line = 1,
                Column = text2.Length
            };

            cv.CaretMoved += tv.HandleCaretMove;

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            sv.Select(startingPosition);
            sv.Select(endingPosition);
            cv.MoveCursor(endingPosition);

            Assert.AreEqual(1, tv.ActivePosition.Line);
            Assert.AreEqual(text2.Length, tv.ActivePosition.Column);
        }

    }
}
