using System;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeEditor.Core.DataStructures;
using CodeEditor.Events;
using CodeEditor.Views.Text;

namespace CodeEditor.Tests.ScenarioTests.Views {
    [TestClass]
    public class TextViewRemovingScenarioTests {
        private TextView tv;

        [TestInitialize]
        public void InitializeTests() {
            tv = new TextView();
        }

        [TestMethod]
        public void ThreeLinesEnteredCursorMovedToSecondDeletePressed_LinesShouldBe2() {
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("asdf");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(2, tv.LinesCount);
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

            Assert.AreEqual(1, tv.LinesCount);
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

            Assert.AreEqual(1, tv.LinesCount);
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

            Assert.AreEqual(3, tv.LinesCount);
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

            Assert.AreEqual(2, tv.LinesCount);
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
            return new CaretMovedEventArgs { NewPosition = new TextPosition(column: col, line: line) };
        }
    }
}
