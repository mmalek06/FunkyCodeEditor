using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TextEditorTests.TextViewTests {
    [TestClass]
    public class TextViewSelectionTests {

        [TestMethod]
        public void SelectFourCharsInOneLineToTheRight_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var tv = new TextEditor.Views.TextView.View();
            var textInfo = new TextEditor.Views.TextView.TextInfo(tv);
            var sv = new TextEditor.Views.SelectionView.View(textInfo);
            var startingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 0,
                Column = 4
            };
            var endingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 0,
                Column = 8
            };

            tv.EnterText(text1 + text2 + text3);
            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition, endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = textInfo.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }

        [TestMethod]
        public void SelectFourCharsInOneLineToTheLeft_TextRangeIsText2() {
            string text1 = "asdf";
            string text2 = "qwer";
            string text3 = "zxcv";
            var tv = new TextEditor.Views.TextView.View();
            var textInfo = new TextEditor.Views.TextView.TextInfo(tv);
            var sv = new TextEditor.Views.SelectionView.View(textInfo);
            var startingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 0,
                Column = 8
            };
            var endingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 0,
                Column = 4
            };

            tv.EnterText(text1 + text2 + text3);
            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition, endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = textInfo.GetTextPartsBetweenPositions(selectionArea.EndPosition, selectionArea.StartPosition);

            Assert.AreEqual(text2, selectedParts.First());
        }

        [TestMethod]
        public void SelectMultipleCharsInTwoLines_TextRangeIsText2Text3() {
            string text1 = "I";
            string text2 = " saw Susie sitting in a shoe shine shop.";
            string text3 = "Where she ";
            string text4 = "sits she shines, and where she shines she sits.";
            var tv = new TextEditor.Views.TextView.View();
            var textInfo = new TextEditor.Views.TextView.TextInfo(tv);
            var sv = new TextEditor.Views.SelectionView.View(textInfo);
            var startingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 0,
                Column = 1
            };
            var endingPosition = new TextEditor.DataStructures.TextPosition {
                Line = 1,
                Column = 10
            };

            tv.EnterText(text1 + text2);
            tv.EnterText("\r");
            tv.EnterText(text3 + text4);
            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs {
                NewPosition = startingPosition
            });
            sv.Select(startingPosition, endingPosition);

            var selectionArea = sv.GetCurrentSelectionArea();
            var selectedParts = textInfo.GetTextPartsBetweenPositions(selectionArea.StartPosition, selectionArea.EndPosition);

            Assert.AreEqual(text2, selectedParts.First());
            Assert.AreEqual(text3, selectedParts.Last());
        }
                
    }
}
