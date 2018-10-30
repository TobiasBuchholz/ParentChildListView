using System.Collections.Generic;

namespace ParentChildListView.Core
{
    public sealed class DiffResult
    {
        public DiffResult(
            IEnumerable<int> addedIndexes, 
            IEnumerable<int> removedIndexes,
            IEnumerable<int> movingIndexes)
        {
            AddedIndexes = addedIndexes;
            RemovedIndexes = removedIndexes;
            MovingIndexes = movingIndexes;
        }

        public IEnumerable<int> AddedIndexes { get; }
        public IEnumerable<int> RemovedIndexes { get; }
        public IEnumerable<int> MovingIndexes { get; }
    }
}