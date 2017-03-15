using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SocksCore.Utils
{

    public class ConcurrentList<T> : IEnumerable<T>
    {
        private List<T> _collection;

        public ConcurrentList()
        {
            _collection = new List<T>();
        }

        public void Add(T element)
        {
            lock (_collection)
            {
                _collection.Add(element);
            }
        }

        public bool Remove(T element)
        {

            lock (_collection)
            {
                return _collection.Remove(element);
            }
        }

        public int RemoveAll(Predicate<T> predicate)
        {
            lock (_collection)
            {
                return _collection.RemoveAll(predicate);
            }
        }

        public List<T> ToList()
        {
            lock (_collection)
            {
                return _collection.ToList();
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            lock (_collection)
            {
                foreach (var element in _collection)
                {
                    yield return element;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}