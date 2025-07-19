using System;
using System.Collections;
using System.Collections.Generic;

namespace VenusECS.Core
{
    /// <summary>
    /// HashMap.
    /// Supports:
    /// Out of range checks, throws exceptions
    /// Does not supports:
    /// Keys duplicates check by performance reasons
    /// </summary>
    /// <typeparam name="TK">Key type</typeparam>
    /// <typeparam name="TD">Value Type. Must be struct.</typeparam>
    public class VenusHashMap<TK, TD> : IEnumerable<TK> where TK : IEquatable<TK> where TD : struct
    {
        private struct DataContainer
        {
            public int KeyHash;
            public TK Key;
            public int DataIndex;
        }

        public class Enumerator : IEnumerator<TK>
        {
            private TK _current;
            private int _currentIndex;
            private VenusHashMap<TK, TD> _hashmap;

            public Enumerator(VenusHashMap<TK, TD> hashmap)
            {
                _hashmap = hashmap;
            }

            public bool MoveNext()
            {
                bool isInBorder = false;
                do
                {
                    var notEmpty = _hashmap._dataContainers[_currentIndex].KeyHash != 0;
                    if (notEmpty)
                    {
                        _current = _hashmap._dataContainers[_currentIndex].Key;
                        return true;
                    }
                    _currentIndex++;
                    isInBorder = _hashmap._currentCapacity > _currentIndex;
                } while (isInBorder);

                return false;
            }

            public void Reset()
            {
                _currentIndex = 0;
            }

            public TK Current => _current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _currentIndex = 0;
            }
        }

        private const int _defaultSizePowerOfTwo = 10;
        private int _currentCapacity;
        private int _initialCapacity;
        private int _count;
        private int _freeCount;
        private int[] _freeIndices;

        private int[] _hashesToIndices;
        private DataContainer[] _dataContainers;
        private TD[] _dataList;

        public VenusHashMap() : this(1 << _defaultSizePowerOfTwo)
        {
        }

        public VenusHashMap(int capacity)
        {
            _currentCapacity = capacity;
            _initialCapacity = capacity;
            _hashesToIndices = new int[_currentCapacity];
            Array.Fill(_hashesToIndices, -1);
            _dataContainers = new DataContainer[_currentCapacity];
            _freeIndices = new int[_currentCapacity];
            _dataList = new TD[_currentCapacity];
            _count = 0;
            _freeCount = 0;
        }

        public int Count() => _count;

        public IEnumerator<TK> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public ref TD Add(TK key)
        {
            var hash = key.GetHashCode();
            var normalizedHash = Math.Abs(hash) % _initialCapacity;
            int offset = 0;
            while (_hashesToIndices[normalizedHash + _initialCapacity * offset] >= 0)
            {
                offset++;
                if (offset * _initialCapacity + normalizedHash >= _currentCapacity)
                {
                    Rehash();
                    break;
                }
            }

            if (_freeCount > 0)
            {
                var baseHashIndex = normalizedHash + _initialCapacity * offset;
                var dataIndex = _freeIndices[_freeCount - 1];
                _hashesToIndices[baseHashIndex] = dataIndex;
                _dataList[dataIndex] = new TD();
                ref var dataContainer = ref _dataContainers[dataIndex];
                dataContainer.DataIndex = dataIndex;
                dataContainer.Key = key;
                dataContainer.KeyHash = hash;
                _freeCount--;
                _count++;
                return ref _dataList[dataIndex];
            }
            else
            {
                var i = normalizedHash + _initialCapacity * offset;
                _hashesToIndices[i] = _count;
                if (_dataContainers.Length <= _count)
                {
                    var newCapacity = _dataContainers.Length + _initialCapacity;
                    Array.Resize(ref _dataContainers, newCapacity);
                    Array.Resize(ref _dataList, newCapacity);
                }
                _dataList[_count] = new TD();
                ref var dataContainer = ref _dataContainers[_count];
                dataContainer.DataIndex = _count;
                dataContainer.Key = key;
                dataContainer.KeyHash = hash;
                return ref _dataList[_count++];
            }
        }

        private void Rehash()
        {
            _currentCapacity += _initialCapacity;
            Array.Resize(ref _hashesToIndices, _currentCapacity);
            Array.Fill(_hashesToIndices, -1, _currentCapacity - _initialCapacity , _initialCapacity);
        }

        public ref TD Get(TK key)
        {
            var hash = key.GetHashCode();
            var normalizedHash = Math.Abs(hash) % _initialCapacity;
            int offset = 0;
            do
            {
                var index = normalizedHash + _initialCapacity * offset;
                var dataContainer = _dataContainers[_hashesToIndices[index]];
                var indexFromContainer = dataContainer.DataIndex;
                if (indexFromContainer == -1)
                {
                    break;
                }
                if (dataContainer.Key.Equals(key))
                {
                    return ref _dataList[_hashesToIndices[index]];
                }
                offset++;
            } while (normalizedHash + _initialCapacity * offset < _currentCapacity);

            throw new ArgumentException("Cant found value by key hash");
        }

        public void Remove(TK key)
        {
            _freeCount++;
            var hash = key.GetHashCode();
            var normalizedHash = Math.Abs(hash) % _initialCapacity;
            int offset = 0;
            do
            {
                var index = normalizedHash + _initialCapacity * offset;
                var dataContainer = _dataContainers[_hashesToIndices[index]];
                var indexFromContainer = dataContainer.DataIndex;
                if (indexFromContainer == -1)
                {
                    break;
                }
                if (dataContainer.Key.Equals(key))
                {
                    _freeIndices[_freeCount - 1] = _hashesToIndices[index];
                    _dataContainers[_hashesToIndices[index]].DataIndex = -1;
                    _hashesToIndices[index] = -1;
                    return;
                }
                offset++;
            } while (normalizedHash + _initialCapacity * offset < _currentCapacity);

            throw new ArgumentException("Cant found value by key hash");
        }

        public bool Has(TK key)
        {
            var hash = key.GetHashCode();
            var normalizedHash = Math.Abs(hash) % _initialCapacity;
            int offset = 0;

            var index = 0;
            while (_hashesToIndices.Length > (index = normalizedHash + _initialCapacity * offset)
                   && _hashesToIndices[index] != -1)
            {
                if (_dataContainers[_hashesToIndices[index]].Key.Equals(key))
                {
                    return true;
                }

                offset++;
            }

            return false;
        }

        public void Clear()
        {
            _count = 0;
            _freeCount = 0;
            Array.Fill(_hashesToIndices, -1);
        }
    }
}