using System;
using System.Collections.Generic;
using System.IO;
using CodeEditor.Enums;
using Newtonsoft.Json;

namespace CodeEditor.Languages {
    internal static class LanguageFeatureInfo {
        private const string LanguagefeaturesFilePath = @"Languages/LanguageFeatures.json";
        private const string DefaultLanguageFeaturesJson = "{'js':{'formatting':'brackets'}}";

        #region fields

        private static Dictionary<string, Dictionary<string, string>> languageFeatures = null;

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

        private static string GetLanguageFeaturesJson()
        {
            return File.Exists(LanguagefeaturesFilePath)
                ? File.ReadAllText(LanguagefeaturesFilePath)
                : DefaultLanguageFeaturesJson;
        }

        #endregion

    }
}
