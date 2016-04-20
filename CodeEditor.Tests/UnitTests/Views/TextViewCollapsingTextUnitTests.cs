﻿using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.UnitTests.Views {
    [TestClass]
    public class TextViewCollapsingTextUnitTests {
        private TextView tv;

        private CaretView cv;

        [TestInitialize]
        public void InitializeTest() {
            cv = new CaretView();
            tv = new TextView(cv);
        }

        [TestMethod]
        public void EnterOpeningAndClosingBracket_LineStateShouldNotChange() {
            string text1 = "{}";

            tv.EnterText(text1);
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.FOLDED));
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.EXPANDED));

            var renderedLines = tv.GetScreenLines();

            Assert.AreEqual(renderedLines[0], text1);
        }

        [TestMethod]
        public void CreateTwoFolds_StateAfterCollapseAndExpandShouldNotChange() {
            string text1 = "asdf {";
            string text2 = "";
            string text3 = "} qwer";
            string text4 = "{";
            string text5 = "";
            string text6 = "}";
            string text7 = "xzcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.EnterText("\r");
            tv.EnterText(text4);
            tv.EnterText("\r");
            tv.EnterText(text5);
            tv.EnterText("\r");
            tv.EnterText(text6);
            tv.EnterText("\r");
            tv.EnterText(text7);
            tv.EnterText("\r");

            tv.HandleTextFolding(GetFoldClickedMessage(5, 0, 0, 2, FoldingStates.FOLDED));
            tv.HandleTextFolding(GetFoldClickedMessage(5, 0, 0, 2, FoldingStates.EXPANDED));

            var renderedLines = tv.GetScreenLines();
            var actualLines = tv.GetActualLines();

            Assert.IsTrue(Enumerable.SequenceEqual(renderedLines, actualLines));
        }

        private FoldClickedMessage GetFoldClickedMessage(int startingCol, int startingLine, int endingCol, int endingLine, FoldingStates state) {
            return new FoldClickedMessage {
                State = state,
                Area = new TextRange {
                    StartPosition = new TextPosition(startingCol, startingLine),
                    EndPosition = new TextPosition(endingCol, endingLine)
                },
                OpeningTag = "{",
                ClosingTag = "}"
            };
        }
    }
}
