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
        public void KeywordsEntered_ShouldApplyKeywordParsing(string text) {
            var parsedContent = algorithm.Parse(new [] { text }).First().ToArray();

            CollectionAssert.AllItemsAreNotNull(parsedContent);
            Assert.That(parsedContent[0].Text, Is.EqualTo(text));
            Assert.That(parsedContent[0].Type, Is.EqualTo(TextType.KEYWORD));
        }
    }
}
