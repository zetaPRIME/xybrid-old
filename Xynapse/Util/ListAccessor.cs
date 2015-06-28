using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.Util {
    public class ListAccessor<T> : IList<T> {
        private IList<T> list;
        public ListAccessor(IList<T> list) { this.list = list; }

        public int IndexOf(T item) {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item) {
            throw new UnauthorizedAccessException();
        }

        public void RemoveAt(int index) {
            throw new UnauthorizedAccessException();
        }

        public T this[int index] {
            get {
                return list[index];
            }
            set {
                throw new UnauthorizedAccessException();
            }
        }

        public void Add(T item) {
            throw new UnauthorizedAccessException();
        }

        public void Clear() {
            throw new UnauthorizedAccessException();
        }

        public bool Contains(T item) {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            list.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(T item) {
            throw new UnauthorizedAccessException();
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }
    }
}
