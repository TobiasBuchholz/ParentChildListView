using System.Collections.Generic;
using System.Linq;
using PressMatrix.Utility.TreeNodes;

namespace ParentChildListView.UI.TreeNodes
{
    public sealed class TreeNode<T> where T : ITreeNodeData
    {
        public const long ParentIdNone = -1;

        private readonly List<TreeNode<T>> _parentNodes;
        private readonly List<TreeNode<T>> _childNodes;

        public TreeNode(T data)
        {
            Data = data;
            _parentNodes = new List<TreeNode<T>>();
            _childNodes = new List<TreeNode<T>>();
        }

        public void AddChildNode(TreeNode<T> node)
        {
            node._parentNodes.AddRange(_parentNodes);
            node._parentNodes.Add(this);
            _childNodes.Add(node);
        }

        public DiffResult CalculateDiff(TreeNode<T> other)
        {
            var levelDelta = ParentNodes.Count - other.ParentNodes.Count;
            if(levelDelta > 1) {
                return CalculateMoreLevelsDiff(other);
            } else if(levelDelta == 1) {
                return CalculateOneLevelDiffReversed(other);
            } else {
                return CalculateOneLevelDiff(other);
            }
        }

        private DiffResult CalculateMoreLevelsDiff(TreeNode<T> other)
        {
            var removedIndexes = Enumerable.Range(other.ParentNodes.Count + 1, CountParentsInBetween(other) + ChildNodes.Count + 1);
            var addedIndexes = Enumerable.Range(other.ParentNodes.Count + 1, other.ChildNodes.Count);
            return new DiffResult(addedIndexes, removedIndexes);
        }

        private int CountParentsInBetween(TreeNode<T> other)
        {
            return ParentNodes
                .Except(other.ParentNodes)
                .Count(x => x.Id != other.Id);
        }

        private DiffResult CalculateOneLevelDiffReversed(TreeNode<T> other)
        {
            var diff = other.CalculateOneLevelDiff(this);
            return new DiffResult(diff.RemovedIndexes, diff.AddedIndexes);
        }

        private DiffResult CalculateOneLevelDiff(TreeNode<T> other)
        {
            var index = _childNodes.FindIndex(0, x => x.Id == other.Id);
            var addedIndexes = Enumerable.Range(other.ParentNodes.Count + 1, other.ChildNodes.Count);
            var removedIndexes = Enumerable
                .Range(0, index)
                .Select(x => x + other.ParentNodes.Count)
                .Concat(Enumerable.Range(index + _parentNodes.Count + 2, _childNodes.Count - index - 1));
            return new DiffResult(addedIndexes, removedIndexes);
        }
        
        public override string ToString()
        {
            return $"[TreeNode: Id={Id} | ParentId={ParentId} | Data={Data}]";
        }
        
        public T Data { get; }
        public long Id => Data.Id;
        public long ParentId => Data.ParentId;
        public IReadOnlyList<TreeNode<T>> ChildNodes => _childNodes; //.AsReadOnly();
        public IReadOnlyList<TreeNode<T>> ParentNodes => _parentNodes; //.AsReadOnly();
    }
}