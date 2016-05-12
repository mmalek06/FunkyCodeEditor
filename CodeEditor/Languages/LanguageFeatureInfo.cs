using System;
using System.Collections.Generic;
using System.IO;
using CodeEditor.Enums;
using Newtonsoft.Json;

namespace CodeEditor.Languages {
    internal static class LanguageFeatureInfo {

        #region fields

        private static Dictionary<string, Dictionary<string, string>> languageFeatures = null;

        #endregion

        #region public methods

        public static FormattingType GetFormattingType(SupportedLanguages language) {
            if (languageFeatures == null) {
                languageFeatures = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(@"Languages/LanguageFeatures.json"));
            }

            var formattingType = (FormattingType)Enum.Parse(typeof(FormattingType), languageFeatures[language.ToString().ToLower()]["formatting"].ToUpper());

            return formattingType;
        }

        #endregion

    }
}
