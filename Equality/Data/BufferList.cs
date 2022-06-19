using System;
using System.Collections;
using System.Collections.Generic;

namespace Equality.Data
{
    /// <summary>
    /// Buffer list which automatically remove items from collection when limit is exceeded.
    /// The item will be removed from the start of the list(by 0 index).
    /// </summary>
    internal class BufferList<T> : IEnumerable<T>
    {
        private List<T> _collection = new();

        public BufferList(int capacity)
        {
            if (capacity < 1) {
                throw new ArgumentOutOfRangeException();
            }

            Capacity = capacity;
        }

        public BufferList(int capacity, IEnumerable<T> collection) : this(capacity)
        {
            AddRange(collection);
        }

        /// <summary>
        /// The limit of the elements in the list.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Adds the given object to the end of this list.
        /// </summary>
        /// <param name="item">The object.</param>
        public void Add(T item)
        {
            _collection.Add(item);

            if (_collection.Count > Capacity) {
                _collection.RemoveAt(0);
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="items">The collection.</param>
        public void AddRange(IEnumerable<T> items)
        {
            _collection.AddRange(items);

            while (_collection.Count > Capacity) {
                _collection.RemoveAt(0);
            }
        }

        /// <summary>
        /// Adds the given object to the end of this list.
        /// Before adding element to the list it will be removed from the list using <see cref="List{T}.Remove(T)"/>.
        /// </summary>
        /// <param name="item">The object.</param>
        public void AddOrReplace(T item)
        {
            _collection.Remove(item);

            Add(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specified object from the list.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item) => _collection.Remove(item);

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_collection).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();

        public T this[int index]
        {
            get {
                return _collection[index];
            }

            set {
                _collection[index] = value;
            }
        }


    }
}
