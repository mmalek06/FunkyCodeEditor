namespace CodeEditor.Messaging {
    public class ScrollChangedMessage {

        #region properties

        public int TopmostLine { get; set; }

        public int BottommostLine { get; set; }

        public int ChangeInLines { get; set; }

        #endregion

    }
}
