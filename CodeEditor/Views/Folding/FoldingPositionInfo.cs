using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Folding {
    internal class FoldingPositionInfo {

        #region properties

        public TextPosition Position { get; set; }

        public FoldingStates State { get; set; }

        public bool Deleted { get; set; }

        #endregion

        #region public methods

        public override bool Equals(object obj) {
            var other = (FoldingPositionInfo)obj;

            if (other == null || other.Position == null) {
                return false;
            }

            return Position.Equals(other.Position);
        }

        public override int GetHashCode() {
            return Position.GetHashCode();
        }

        public override string ToString() {
            return string.Format("{0}, Deleted: {1}, State: {2}", Position.ToString(), Deleted.ToString(), State.ToString());
        }

        #endregion

    }
}
