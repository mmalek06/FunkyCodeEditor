﻿using System.Linq;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Core.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.AlgorithmTests {
    [TestClass]
    public class BracketsCollapsingTextTests {
        private TextCollapser tc;

        [TestInitialize]
        public void InitializeTest() {
            tc = new TextCollapser();
        }

        [TestMethod]
        public void ShouldHaveTextBeforeAndAfterFold() {
            string text1 = "asdf ";
            string open = "{";
            string text2 = string.Empty;
            string close = "}";
            string text3 = " xzcv";

            var lines = new[] { text1 + open, text2, close + text3 };
            var line = tc.CollapseTextRange(new TextPositionsPair { StartPosition = new TextPosition(column: 5, line: 0), EndPosition = new TextPosition(column: 0, line: 2) }, lines, 0);

            Assert.AreEqual(text1, string.Join("", line.Text.Take(5)));
            Assert.AreEqual(text3, string.Join("", line.Text.Skip(10)));
        }
    }
}
