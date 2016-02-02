using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TextEditor.DataStructures {
    internal class AutoTrimmingStack<T> : IEnumerable<T>, IEnumerable, ICollection {

        #region fields

        private readonly int capacity;
        private List<T> stack;

        #endregion

        #region properties

        public int Count => stack.Count;

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        #endregion

        #region constructor

        public AutoTrimmingStack(int capacity) {
            stack = new List<T>(capacity);
            this.capacity = capacity;
        }

        #endregion

        #region public methods

        public void Push(T item) {
            if (Count == capacity) {
                stack.RemoveAt(0);
            }

            stack.Add(item);
        }

        public T Pop() {
            T item = stack.Last();

            stack.Remove(item);

            return item;
        }

        public T Peek() => stack.Last();

        public void CopyTo(Array array, int index) => stack.CopyTo((T[])array, index);

        public IEnumerator<T> GetEnumerator() => stack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => stack.GetEnumerator();

        #endregion

    }
}
