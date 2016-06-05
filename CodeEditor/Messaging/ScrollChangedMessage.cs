namespace CodeEditor.Messaging {
    public class ScrollChangedMessage {

        #region properties

        public int LinesScrolled { get; set; }

        public int FirstVisibleLineIndex { get; set; }

        public int LastVisibleLineIndex { get; set; }

        #endregion

    }
}
