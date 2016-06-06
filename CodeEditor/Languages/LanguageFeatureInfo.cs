using System;
using System.Collections.Generic;
using System.IO;
using CodeEditor.Enums;
using Newtonsoft.Json;

namespace CodeEditor.Languages {
    internal static class LanguageFeatureInfo {

        #region constants

        private const string LanguagefeaturesFilePath = @"Languages/LanguageFeatures.json";

        private const string DefaultLanguageFeaturesJson = "{'js':{'formatting':'brackets'}}";

        #endregion

        #region fields

        private static Dictionary<string, Dictionary<string, string>> languageFeatures;

        #endregion

        #region public methods

        public static FormattingType GetFormattingType(SupportedLanguages language) {
            if (languageFeatures == null) {
                languageFeatures = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(GetLanguageFeaturesJson());
            }

            // TODO: implement better defaults handling, decouple
            var formattingType = (FormattingType)Enum.Parse(typeof(FormattingType), languageFeatures[language.ToString().ToLower()]["formatting"].ToUpper());

            return formattingType;
        }

        private static string GetLanguageFeaturesJson() =>
            File.Exists(LanguagefeaturesFilePath) ? File.ReadAllText(LanguagefeaturesFilePath) : DefaultLanguageFeaturesJson;

        #endregion

    }
}
