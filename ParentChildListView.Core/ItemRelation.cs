using ParentChildListView.Core.TreeNodes;

namespace ParentChildListView.Core
{
    public sealed class ItemRelation
    {
        public static ItemRelation AsParent<T>(TreeNode<T> node) where T : ITreeNodeData
        {
            return new ItemRelation(ItemRelationType.Parent, node.ParentNodes.Count);
        }

        public static ItemRelation AsChild<T>(TreeNode<T> node) where T : ITreeNodeData
        {
            return new ItemRelation(ItemRelationType.Child, node.ParentNodes.Count + 1);
        }

        public static ItemRelation AsSelected<T>(TreeNode<T> node) where T : ITreeNodeData
        {
            return new ItemRelation(ItemRelationType.Selected, node.ParentNodes.Count);
        }
        
        private ItemRelation(ItemRelationType type, int level)
        {
            Type = type;
            Level = level;
        }

        public override string ToString()
        {
            return $"[ItemRelation: Type={Type} | Level={Level}]";
        }

        public ItemRelationType Type { get; }
        public int Level { get; }
    }
}