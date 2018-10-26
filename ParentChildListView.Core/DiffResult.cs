using System.Collections.Generic;

namespace ParentChildListView.UI
{
    public sealed class DiffResult
    {
        public DiffResult(
            IEnumerable<int> addedIndexes = null, 
            IEnumerable<int> removedIndexes = null,
            IEnumerable<int> movingIndexes = null)
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