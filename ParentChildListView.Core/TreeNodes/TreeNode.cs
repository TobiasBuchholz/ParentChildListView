using System.Collections.Generic;
using System.Linq;

namespace ParentChildListView.Core.TreeNodes
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
            var flattenedNodes = Flatten().ToList();
            var otherFlattenedNodes = other.Flatten().ToList();

            var removedNodes = flattenedNodes.Except(otherFlattenedNodes).ToList();
            var addedNodes = otherFlattenedNodes.Except(flattenedNodes);
            var movingNodes = flattenedNodes.Except(removedNodes);

            var removedIndexes = removedNodes.Select(x => flattenedNodes.FindIndex(0, y => x.Id == y.Id));
            var addedIndexes = addedNodes.Select(x => otherFlattenedNodes.FindIndex(0, y => x.Id == y.Id));
            var movingIndexes = movingNodes.Select(x => flattenedNodes.FindIndex(0, y => x.Id == y.Id));

            return new DiffResult(addedIndexes, removedIndexes, movingIndexes);
        }
        
        public IEnumerable<TreeNode<T>> Flatten()
        {
            return _parentNodes
                .Concat(new[] { this })
                .Concat(ChildNodes);
        }
        
        public override string ToString()
        {
            return $"[TreeNode: Id={Id} | ParentId={ParentId} | Data={Data}]";
        }
        
        public T Data { get; }
        public long Id => Data.Id;
        public long ParentId => Data.ParentId;
        public IReadOnlyList<TreeNode<T>> ChildNodes => _childNodes.AsReadOnly();
        public IReadOnlyList<TreeNode<T>> ParentNodes => _parentNodes.AsReadOnly();
    }
}