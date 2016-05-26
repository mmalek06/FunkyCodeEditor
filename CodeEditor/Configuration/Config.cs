using System.Collections.Generic;
using CodeEditor.Controls;
using CodeEditor.Enums;

namespace CodeEditor.Configuration {
    internal static class ConfigManager {

        #region fields

        private static Dictionary<int, Config> configMap = new Dictionary<int, Config>();

        #endregion

        #region public methods

        public static void AddEditorConfig(int editorCode, Config config) => configMap[editorCode] = config;

        public static Config GetConfig(int editorCode) => configMap[editorCode];

        #endregion

    }

    internal class Config {

        #region properties

        public SupportedLanguages Language { get; set; }

        public FormattingType FormattingType { get; set; }

        public InputViewsWrapper InputControl { get; set; }

        #endregion

    }
}
