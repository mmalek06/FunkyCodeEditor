namespace CodeEditor.DataStructures {
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public class TextPosition {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public int Column { get; set; }
        public int Line { get; set; }

        public TextPosition() { }

        public TextPosition(int column, int line) {
            Column = column;
            Line = line;
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public static bool operator <(TextPosition @this, TextPosition other) => other == null ? false : @this.Column < other.Column || @this.Line < other.Line;

        public static bool operator >(TextPosition @this, TextPosition other) => other == null ? false : @this.Column > other.Column || @this.Line > other.Line;

        public static bool operator <=(TextPosition @this, TextPosition other) => @this < other || @this == other;

        public static bool operator >=(TextPosition @this, TextPosition other) => @this > other || @this == other;

        public static bool operator ==(TextPosition @this, TextPosition other) {
            if (ReferenceEquals(@this, null) && ReferenceEquals(other, null)) {
                return true;
            }
            if (ReferenceEquals(other, null)) {
                return false;
            }

            return @this.Column == other.Column && @this.Line == other.Line;
        }

        public static bool operator !=(TextPosition @this, TextPosition other) {
            if ((ReferenceEquals(@this, null) && !ReferenceEquals(other, null))|| (!ReferenceEquals(@this, null) && ReferenceEquals(other, null))) {
                return true;
            }
            if (ReferenceEquals(@this, null) && ReferenceEquals(other, null)) {
                return false;
            }

            return @this.Column != other.Column || @this.Line != other.Line;
        }

    }
}
