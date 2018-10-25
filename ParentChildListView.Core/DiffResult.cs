using System.Collections.Generic;

namespace ParentChildListView.UI
{
    public sealed class DiffResult
    {
        public DiffResult(IEnumerable<int> addedIndexes = null, IEnumerable<int> removedIndexes = null)
        {
            AddedIndexes = addedIndexes;
            RemovedIndexes = removedIndexes;
        }

        public IEnumerable<int> AddedIndexes { get; }
        public IEnumerable<int> RemovedIndexes { get; }
    }
}