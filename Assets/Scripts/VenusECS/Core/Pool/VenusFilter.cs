using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VenusECS.Core.Pool
{
    public class VenusFilter : IEnumerable<VenusEntity>, IEnumerator<VenusEntity>
    {
        private IExcludeVenusFilter _exclude;
        private IIncludeVenusFilter _include;
        private int _minEntityId;
        private int _maxEntityId;
        private int _bitsPerInt;
        private int _nextEntityId;
        private int _currentChunkIndex;
        private int _currentMask;
        private int _currentExcludeMask;
        private VenusEntity _currentEntity;
        private bool _isClone;
        private bool _isIterating;

        public VenusEntity Current => _currentEntity;

        object IEnumerator.Current => Current;

        public VenusFilter(IIncludeVenusFilter includeFilter) : this(includeFilter, null)
        {
        }

        public VenusFilter(IIncludeVenusFilter includeFilter, IExcludeVenusFilter excludeVenusFilter)
        {
            _include = includeFilter;
            _exclude = excludeVenusFilter;
            _currentEntity = new VenusEntity { Id = -1 };
            CalculateEntityIdRange();
            Reset();
        }

        // Private constructor for cloning
        private VenusFilter(VenusFilter source, bool isClone)
        {
            _include = source._include;
            _exclude = source._exclude;
            _minEntityId = source._minEntityId;
            _maxEntityId = source._maxEntityId;
            _bitsPerInt = source._bitsPerInt;
            _currentEntity = new VenusEntity { Id = -1 };
            _isClone = isClone;
            Reset();
        }

        // Create a clone that shares the same filters but has its own iteration state
        public VenusFilter Clone()
        {
            return new VenusFilter(this, true);
        }

        private void CalculateEntityIdRange()
        {
            if (_include.PoolsToInclude.Count == 0) return;
            
            // Get the range of entity IDs we need to check
            _minEntityId = int.MaxValue;
            _maxEntityId = int.MinValue;
            _bitsPerInt = 0;
            
            // Calculate min/max entity IDs from include pools
            foreach (var pool in _include.PoolsToInclude)
            {
                _minEntityId = Math.Min(_minEntityId, pool.MinEntityId);
                _maxEntityId = Math.Max(_maxEntityId, pool.MaxEntityId);
                if (_bitsPerInt == 0) _bitsPerInt = pool.BitsPerInt;
            }
            
            // Also consider exclude pools for min/max calculation
            if (_exclude != null && _exclude.PoolsToExclude.Count > 0)
            {
                foreach (var pool in _exclude.PoolsToExclude)
                {
                    _minEntityId = Math.Min(_minEntityId, pool.MinEntityId);
                    _maxEntityId = Math.Max(_maxEntityId, pool.MaxEntityId);
                    if (_bitsPerInt == 0) _bitsPerInt = pool.BitsPerInt;
                }
            }
        }

        private void UpdateCurrentChunkMasks()
        {
            if (_include.PoolsToInclude.Count == 0) return;
            
            // Start with all bits set
            _currentMask = ~0;
            
            // For include, we need all bits to be set (AND)
            foreach (var pool in _include.PoolsToInclude)
            {
                int poolOffset = (pool.MinEntityId - _minEntityId) / _bitsPerInt;
                int poolChunkIndex = _currentChunkIndex - poolOffset;
                
                // If this chunk is outside pool's range, result is 0
                if (poolChunkIndex < 0 || poolChunkIndex >= pool.Bitmask.Length)
                {
                    _currentMask = 0;
                    return;
                }
                
                _currentMask &= pool.Bitmask[poolChunkIndex];
                
                // Early exit if mask becomes 0
                if (_currentMask == 0) return;
            }
            
            // For exclude pools, we need to ensure bits are NOT set
            _currentExcludeMask = 0;
            if (_exclude != null && _exclude.PoolsToExclude.Count > 0)
            {
                foreach (var pool in _exclude.PoolsToExclude)
                {
                    int poolOffset = (pool.MinEntityId - _minEntityId) / _bitsPerInt;
                    int poolChunkIndex = _currentChunkIndex - poolOffset;
                    
                    // If this chunk is outside pool's range, skip it
                    if (poolChunkIndex < 0 || poolChunkIndex >= pool.Bitmask.Length) continue;
                    
                    _currentExcludeMask |= pool.Bitmask[poolChunkIndex];
                }
            }
            
            // Apply exclude mask (remove excluded bits)
            _currentMask &= ~_currentExcludeMask;
        }

        public IEnumerator<VenusEntity> GetEnumerator()
        {
            // Before iteration, recalculate entity range to ensure we have the latest
            CalculateEntityIdRange();
            
            // If we're already being iterated, create a new enumerator
            // Otherwise, just reset ourselves and return this instance
            if (_isIterating)
            {
                return Clone();
            }
            
            Reset();
            _isIterating = true;
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            if (_include.PoolsToInclude.Count == 0 || _minEntityId > _maxEntityId || _bitsPerInt == 0) 
                return false;

            // Calculate how many chunks (ints) are in the range
            int totalChunks = ((_maxEntityId - _minEntityId) / _bitsPerInt) + 1;

            // Start with current entity position
            int bitPosition = (_nextEntityId - _minEntityId) % _bitsPerInt;
            
            while (_currentChunkIndex < totalChunks)
            {
                // If we're at the start of a chunk, update the masks
                if (bitPosition == 0)
                {
                    UpdateCurrentChunkMasks();
                }
                
                // If current chunk has no matching entities, skip to next chunk
                if (_currentMask == 0)
                {
                    _currentChunkIndex++;
                    bitPosition = 0;
                    continue;
                }
                
                // Find next bit that is set in the mask
                while (bitPosition < _bitsPerInt)
                {
                    if ((_currentMask & (1 << bitPosition)) != 0)
                    {
                        // Found a matching entity
                        int entityId = _minEntityId + (_currentChunkIndex * _bitsPerInt) + bitPosition;
                        _currentEntity.Id = entityId;
                        
                        // Prepare for next search
                        _nextEntityId = entityId + 1;
                        bitPosition++;

                        if (bitPosition  > 0 && bitPosition % _bitsPerInt == 0) 
                        {
                            _currentChunkIndex++;
                        }
                        
                        return true;
                    }
                    
                    bitPosition++;
                }
                
                // Move to next chunk
                _currentChunkIndex++;
                bitPosition = 0;
            }
            
            // We've completed the iteration
            _isIterating = false;
            return false;
        }

        public void Reset()
        {
            _nextEntityId = _minEntityId;
            _currentChunkIndex = 0;
            _currentMask = 0;
            _currentExcludeMask = 0;
            _currentEntity.Id = -1;
            _isIterating = false;
        }

        public void Dispose()
        {
            // No need to dispose any resources
        }
    }
}