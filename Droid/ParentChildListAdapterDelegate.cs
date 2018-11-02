using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;

namespace ParentChildListView.UI.Droid
{
    public sealed class ParentChildListAdapterDelegate<T> where T : ITreeNodeData
    {
        private readonly RecyclerView _recyclerView;
        private readonly Func<ViewGroup, int, RecyclerView.ViewHolder> _viewHolderSelector;
        private readonly Action<RecyclerView.ViewHolder, ItemRelation, T> _viewHolderBound;
        private readonly Action<RecyclerView.ViewHolder, ItemRelation> _itemStateChanged;
        private readonly ItemAnimator _itemAnimator;
        private TreeNode<T> _currentNode;

        public ParentChildListAdapterDelegate(
            RecyclerView recyclerView,
            Func<ViewGroup, int, RecyclerView.ViewHolder> viewHolderSelector,
            Action<RecyclerView.ViewHolder, ItemRelation, T> viewHolderBound,
            Action<RecyclerView.ViewHolder, ItemRelation> itemStateChanged)
        {
            _recyclerView = recyclerView;
            _viewHolderSelector = viewHolderSelector;
            _viewHolderBound = viewHolderBound;
            _itemStateChanged = itemStateChanged;
            _itemAnimator = new ItemAnimator();
            _recyclerView.SetItemAnimator(_itemAnimator);
        }

        public void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var relation = GetRelationForIndex(position);
            var data = GetDataForIndex(position, relation);
            _viewHolderBound(holder, relation, data);
        }

        private ItemRelation GetRelationForIndex(int index)
        {
            return index == 0 || index < _currentNode.ParentNodes.Count 
                ? ItemRelation.AsParent(_currentNode) 
                : ItemRelation.AsChild(_currentNode);
        }

        private T GetDataForIndex(int index, ItemRelation relation)
        {
            switch(relation.Type) {
                case ItemRelationType.Parent:
                    return relation.Level == 0 ? _currentNode.Data : _currentNode.ParentNodes[index].Data;
                case ItemRelationType.Child:
                    return _currentNode.ChildNodes[index - (_currentNode.ParentNodes.Count + 1)].Data;
            }
            throw new ArgumentException($"Couldn't get data for index {index}");
        }
        
        public RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _viewHolderSelector(parent, viewType);
        }

        public void OnItemSelected(int index)
        {
            var parentNodes = _currentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;

            if(index == 0) {
                SetCurrentNodeWithAnimation(index, _currentNode.ParentNodes.First());
            } else if(index < parentNodesCount) {
                SetCurrentNodeWithAnimation(index, parentNodes[index]);
            } else if(index > parentNodesCount) {
                SetCurrentNodeWithAnimation(index, _currentNode.ChildNodes[index - (parentNodesCount + 1)]);
            }
        }

        private void SetCurrentNodeWithAnimation(int index, TreeNode<T> selectedNode)
        {
            var diffResult = _currentNode.CalculateDiff(selectedNode);
            var previousNodeFlattened = _currentNode.Flatten().ToArray();
            var movingIndexes = diffResult.MovingIndexes.ToArray();

            foreach(var i in movingIndexes) {
                var viewHolder = _recyclerView.FindViewHolderForLayoutPosition(i);
                var relation = i == index && i > 0 ? ItemRelation.AsSelected(selectedNode) : GetStateForPreviousNode(previousNodeFlattened[i], selectedNode);
                _itemStateChanged(viewHolder, relation);
            }
            
            _currentNode = selectedNode;
            _itemAnimator.DiffResult = diffResult;
            AnimateDiffAsync(diffResult).Ignore();
        }
        
        private static ItemRelation GetStateForPreviousNode(TreeNode<T> previousNode, TreeNode<T> currentNode)
        {
            return currentNode.ChildNodes.Contains(previousNode)
                ? ItemRelation.AsChild(previousNode)
                : ItemRelation.AsParent(previousNode);
        }

        private async Task AnimateDiffAsync(DiffResult diffResult)
        {
            await DeleteItemsIfNeededAsync(diffResult.RemovedIndexes.ToArray());
            await InsertItemsIfNeededAsync(diffResult.AddedIndexes.ToArray());
        }

        private Task DeleteItemsIfNeededAsync(IReadOnlyList<int> indexes)
        {
            return indexes.Any() ? DeleteItemsAsync(indexes) : Task.CompletedTask;
        }

        private Task DeleteItemsAsync(IEnumerable<int> indexes)
        {
            // unfortunately removing the indexes the same way like they are added in InsertItemsAsync()
            // results in a crash for some selected indexes, so DiffUtil comes to the rescue
            var oldIndexes = Enumerable.Range(0, ItemCount).ToArray();
            var newIndexes = oldIndexes.Except(indexes).ToArray();
            var callback = new IndexDiffUtilCallback(oldIndexes, newIndexes);
            var adapterDiff = DiffUtil.CalculateDiff(callback);

            ItemCount = newIndexes.Length;

            adapterDiff.DispatchUpdatesTo(_recyclerView.GetAdapter());
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private Task InsertItemsIfNeededAsync(IReadOnlyCollection<int> indexes)
        {
            return indexes.Any() ? InsertItemsAsync(indexes) : Task.CompletedTask;
        }

        private Task InsertItemsAsync(IReadOnlyCollection<int> indexes)
        {
            ItemCount += indexes.Count;
            NotifyItemsInserted(indexes);
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private void NotifyItemsInserted(IEnumerable<int> indexes)
        {
            var consecutiveGroups = indexes.GroupConsecutive();
            foreach(var group in consecutiveGroups) {
                var array = group.ToArray();
                _recyclerView.GetAdapter().NotifyItemRangeInserted(array.First(), array.Count());
            }
        }
        
        public int ItemCount { get; private set; }

        public TreeNode<T> CurrentNode {
            set {
                _currentNode = value;
                ItemCount = value.ParentNodes.Count + value.ChildNodes.Count + 1;
            }
        }
    }
}