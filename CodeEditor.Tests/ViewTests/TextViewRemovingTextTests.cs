using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using CodeEditor.Events;

namespace CodeEditor.Tests {
    [TestClass]
    public class TextViewRemovingTextTests {
        private Views.TextView.View tv;
        private Views.TextView.TextInfo ti;

        [TestInitialize]
        public void InitializeTests() {
            tv = new Views.TextView.View();
            ti = new Views.TextView.TextInfo(tv);
        }

        [TestMethod]
        public void CharEnteredAndBackspaced_CursorShouldBeAt00() {
            tv.EnterText("s");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_CursorShouldBeAt00() {
            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_LinesShouldBe1() {
            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_CursorShouldBeAt00() {
            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_LinesShouldBe1() {
            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LinesShouldBe1() {
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_CursorShouldBeAt40() {
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LinesShouldBe1() {
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_CursorShouldBeAt40() {
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
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
            tv.RemoveText(new DataStructures.TextPositionsPair {
                StartPosition = new DataStructures.TextPosition(column: 0, line: 0),
                EndPosition = new DataStructures.TextPosition(column: 21, line: 2)
            });

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe3() {
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(3, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe2() {
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(2, ti.GetTextLinesCount());
        }

        private KeyEventArgs CreateKeyEventArgs(Key key) {
            return new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, key) {
                RoutedEvent = Keyboard.KeyDownEvent
            };
        }

        private TextCompositionEventArgs CreateTextCompositionEventArgs(string text) {
            return new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text));
        }

        private CaretMovedEventArgs CreateCaretMovedEventArgs(int col, int line) {
            return new CaretMovedEventArgs { NewPosition = new CodeEditor.DataStructures.TextPosition(column: col, line: line) };
        }
    }
}
