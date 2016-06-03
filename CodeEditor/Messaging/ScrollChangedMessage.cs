namespace CodeEditor.Messaging {
    public class ScrollChangedMessage {

        #region properties

        public int ChangeInLines { get; set; }

        public int TopmostLine { get; set; }

        public int BottommostLine { get; set; }

        #endregion

    }
}
