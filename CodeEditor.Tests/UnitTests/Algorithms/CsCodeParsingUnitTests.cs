using System.Linq;
using CodeEditor.Algorithms.Parsing;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Enums;
using CodeEditor.Tests.Mocks;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Algorithms {
    [TestFixture]
    public class CsCodeParsingUnitTests {
        private TextParsingAlgorithm algorithm;

        [SetUp]
        public void Initialize() {
            algorithm = new TextParsingAlgorithm(SupportedLanguages.CS, new MockDefinitionLoader());
        }

        [TestCase("public")]
        [TestCase("void")]
        public void KeywordEntered_ShouldApplyKeywordParsing(string text) {
            var parsedContent = algorithm.Parse(new [] { text }).First().ToArray();

            CollectionAssert.AllItemsAreNotNull(parsedContent);
            Assert.That(parsedContent[0].Text, Is.EqualTo(text));
            Assert.That(parsedContent[0].Type, Is.EqualTo(TextType.KEYWORD));
        }

        [TestCase("public void")]
        [TestCase("internal static")]
        public void KeywordsEntered_ShouldApplyKeywordParsing(string text) {
            var parsedContent = algorithm.Parse(new[] { text }).First().ToArray();
            var rawWords = text.Split(' ');

            CollectionAssert.AllItemsAreNotNull(parsedContent);
            Assert.That(parsedContent[0].Text, Is.EqualTo(rawWords[0]));
            Assert.That(parsedContent[0].Type, Is.EqualTo(TextType.KEYWORD));
            Assert.That(parsedContent[1].Text, Is.EqualTo(rawWords[1]));
            Assert.That(parsedContent[1].Type, Is.EqualTo(TextType.KEYWORD));
        }
    }
}
