using System.Collections.Generic;
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

        public T Data { get; }
        public long Id => Data.Id;
        public long ParentId => Data.ParentId;
        public IReadOnlyList<TreeNode<T>> ChildNodes => _childNodes; //.AsReadOnly();

        public IReadOnlyList<TreeNode<T>> ParentNodes => _parentNodes; //.AsReadOnly();

        public override string ToString()
        {
            return $"[TreeNode: Id={Id} | ParentId={ParentId} | Data={Data}]";
        }
    }
}