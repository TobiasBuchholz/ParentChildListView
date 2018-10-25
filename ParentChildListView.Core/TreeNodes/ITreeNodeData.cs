namespace PressMatrix.Utility.TreeNodes
{
    public interface ITreeNodeData
    {
        long ParentId { get; }
        long Id { get; }
    }
}