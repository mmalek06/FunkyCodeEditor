﻿using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Enums;
using CodeEditor.Core.Messaging;
using CodeEditor.Views.Caret;
using CodeEditor.Views.Text;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Views {
    [TestFixture]
    public class TextViewCollapsingTextWithBracketsUnitTests {
        private TextView tv;

        private CaretView cv;

        [SetUp]
        public void InitializeTest() {
            const int EditorCode = 1;

            Configuration.ConfigManager.AddEditorConfig(EditorCode, new Configuration.Config {
                Language = SupportedLanguages.JS,
                FormattingType = FormattingType.BRACKETS
            });

            cv = new CaretView();
            tv = new TextView(cv);

            cv.EditorCode = EditorCode;
            tv.EditorCode = EditorCode;
        }

        [Test]
        public void EnterOpeningAndClosingBracket_LineStateShouldNotChange() {
            string text1 = "{}";

            tv.EnterText(text1);
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.FOLDED));
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.EXPANDED));

            var renderedLines = tv.GetScreenLines();

            Assert.AreEqual(renderedLines[0], text1);
        }

        [Test]
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
                AreaBeforeFolding = new TextRange {
                    StartPosition = new TextPosition(startingCol, startingLine),
                    EndPosition = new TextPosition(endingCol, endingLine)
                },
                OpeningTag = "{",
                ClosingTag = "}"
            };
        }
    }
}
