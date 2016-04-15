using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using CodeEditor.Core.DataStructures;
using CodeEditor.Events;
using CodeEditor.Views.Text;

namespace CodeEditor.Tests.UnitTests.Views {
    [TestClass]
    public class TextViewRemovingTextUnitTests {
        private TextView tv;
        private TextView.TextViewInfo ti;

        [TestInitialize]
        public void InitializeTests() {
            tv = new TextView();
            ti = TextView.TextViewInfo.GetInstance(tv);
        }

        [TestMethod]
        public void CharEnteredAndBackspaced_CursorShouldBeAt00() {
            tv.EnterText("s");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, ti.ActivePosition.Column);
            Assert.AreEqual(0, ti.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_CursorShouldBeAt00() {
            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, ti.ActivePosition.Column);
            Assert.AreEqual(0, ti.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_LinesShouldBe1() {
            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, ti.LinesCount);
        }

        [TestMethod]
        public void ThreeEmptyLinesEnteredAndDelPressed_LinesShouldBe3() {
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(3, ti.LinesCount);
        }

        [TestMethod]
        public void ThreeEmptyLinesEnteredAndDelPressed_CursorShouldBeAt20() {
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(2, ti.ActivePosition.Line);
            Assert.AreEqual(0, ti.ActivePosition.Column);
        }

        [TestMethod]
        public void ThreeNonEmptyLinesEnteredBackspacePressedAtCharOneBeforeTheLastOneInTheLastLine_LinesShouldBe1() {
            string text1 = "some text";
            string text2 = "";
            string text3 = "totally unimportant text";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.RemoveText(new TextRange {
                StartPosition = new TextPosition(column: 0, line: 0),
                EndPosition = new TextPosition(column: 21, line: 2)
            });

            Assert.AreEqual(1, ti.LinesCount);
        }
    }
}
