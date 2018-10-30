namespace ParentChildListView.Core.TreeNodes
{
    public interface ITreeNodeData
    {
        long ParentId { get; }
        long Id { get; }
    }
}