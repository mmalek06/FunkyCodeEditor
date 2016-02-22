using System;

namespace CodeEditor.DataStructures {
    public class TextPosition : IComparable {

        #region properties

        public static TextPosition Zero => new TextPosition(0, 0);

        public int Column { get; }

        public int Line { get; }

        #endregion

        #region constructors

        public TextPosition(int column, int line) {
            Column = column;
            Line = line;
        }

        #endregion

        #region public methods and operators

        public override bool Equals(object obj) {
            var other = (TextPosition)obj;

            return Column == other.Column && Line == other.Line;
        }

        public override int GetHashCode() {
            int hash = 23;

            hash = hash * 31 + Column.GetHashCode();
            hash = hash * 31 + Line.GetHashCode();

            return hash;
        }

        public override string ToString() {
            return string.Format("Column: {0}, Line: {1}", Column, Line);
        }

        public int CompareTo(object obj) {
            var otherPosition = (TextPosition)obj;

            if (this < otherPosition) {
                return -1;
            }
            if (this > otherPosition) {
                return 1;
            }

            return 0;
        }

        public static bool operator <(TextPosition @this, TextPosition other) => other == null ? false : @this.Line < other.Line || (@this.Line < other.Line && @this.Column < other.Column);

        public static bool operator >(TextPosition @this, TextPosition other) => other == null ? false : @this.Line > other.Line || (@this.Line > other.Line && @this.Column > other.Column);

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

        #endregion

    }
}
