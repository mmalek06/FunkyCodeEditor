using CodeEditor.Core.Enums;

namespace CodeEditor.Core.Messaging {
    public class EditorSettingsChangedMessage {

        #region properties

        public SupportedLanguages Language { get; set; }

        public FormattingType FormattingType { get; set; }

        #endregion

    }
}
