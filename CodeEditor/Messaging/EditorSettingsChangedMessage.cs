using CodeEditor.Core.Enums;

namespace CodeEditor.Messaging {
    internal class EditorSettingsChangedMessage {

        #region properties

        public SupportedLanguages Language { get; set; }

        public FormattingType FormattingType { get; set; }

        #endregion

    }
}
