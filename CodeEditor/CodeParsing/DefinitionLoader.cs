using System.Collections.Generic;
using System.IO;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Enums;

namespace CodeEditor.CodeParsing {
    internal class DefinitionLoader : IDefinitionLoader {

        #region constants

        private const string FilePathPart = @"CodeParsing/Files/";

        private static readonly Dictionary<string, IEnumerable<string>> Words;

        #endregion

        #region constructor

        static DefinitionLoader() {
            Words = new Dictionary<string, IEnumerable<string>>();
        }

        #endregion

        #region public methods

        public IEnumerable<string> GetWords(TextType type, SupportedLanguages language) {
            var name = @"" + FilePathPart + language.ToString().ToLower() + "_" + type.ToString().ToLower() + ".txt";

            if (Words.ContainsKey(name)) {
                return Words[name];
            }
            
            if (!File.Exists(name)) {
                throw new FileNotFoundException(name);
            }

            Words[name] = File.ReadAllText(name).Split(',');

            return Words[name];
        } 

        #endregion

    }
}
