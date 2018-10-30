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
using ParentChildListView.UI.TreeNodes;

namespace ParentChildListView.UI.Droid
{
    public class ParentChildListAdapter : RecyclerView.Adapter
    {
        private readonly RecyclerView _recyclerView;
        private readonly ItemAnimator _itemAnimator;
        private TreeNode<Category> _currentNode;
        private int _itemsCount;

        public ParentChildListAdapter(
            RecyclerView recyclerView,
            ItemAnimator itemAnimator)
        {
            _recyclerView = recyclerView;
            _itemAnimator = itemAnimator;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = (ParentChildListAdapterViewHolder) holder;
            var state = GetStateForIndex(position);
            var data = GetDataForIndex(position, state);
            viewHolder.TitleLabel.Text = data.Name;
            viewHolder.State = state;
        }

        private ParentChildItemState GetStateForIndex(int index)
        {
            if(index == 0) {
                return ParentChildItemState.Root;
            } else if(index < _currentNode.ParentNodes.Count) {
                return ParentChildItemState.Parent;
            } else {
                return ParentChildItemState.Child;
            }
        }
        
        private Category GetDataForIndex(int index, ParentChildItemState state)
        {
            switch(state) {
                case ParentChildItemState.Root:
                    return _currentNode.ParentNodes.Any() ? _currentNode.ParentNodes.First().Data : _currentNode.Data;
                case ParentChildItemState.Parent:
                    return _currentNode.ParentNodes[index].Data;
                case ParentChildItemState.Child:
                    return _currentNode.ChildNodes[index - (_currentNode.ParentNodes.Count + 1)].Data;
            }
            throw new ArgumentException($"Couldn't get data for index {index}");
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item, parent, false);
            return new ParentChildListAdapterViewHolder(itemView, OnItemSelected);
        }

        private void OnItemSelected(int index)
        {
            ItemSelected?.Invoke(this, index);
            
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

        private void SetCurrentNodeWithAnimation(int index, TreeNode<Category> selectedNode)
        {
            var diffResult = _currentNode.CalculateDiff(selectedNode);
            var previousNodeFlattened = _currentNode.Flatten().ToArray();
            var movingIndexes = diffResult.MovingIndexes.ToArray();

            foreach(var i in movingIndexes) {
                var previousNode = previousNodeFlattened[i];
                var state = GetStateForPreviousNode(previousNode, selectedNode);
                var viewHolder = (ParentChildListAdapterViewHolder) _recyclerView.FindViewHolderForLayoutPosition(i);
                viewHolder.State = i == index && i > 0 ? ParentChildItemState.Selected : state;
            }
            
            _currentNode = selectedNode;
            _itemAnimator.DiffResult = diffResult;
            AnimateDiffAsync(diffResult).Ignore();
        }
        
        private static ParentChildItemState GetStateForPreviousNode(TreeNode<Category> previousNode, TreeNode<Category> currentNode)
        {
            if(previousNode.ParentNodes.Any()) {
                return currentNode.ChildNodes.Contains(previousNode) ? ParentChildItemState.Child : ParentChildItemState.Parent;
            } else {
                return ParentChildItemState.Root;
            }
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
            var oldIndexes = Enumerable.Range(0, _itemsCount).ToArray();
            var newIndexes = oldIndexes.Except(indexes).ToArray();
            var callback = new IndexDiffUtilCallback(oldIndexes, newIndexes);
            var adapterDiff = DiffUtil.CalculateDiff(callback);

            _itemsCount = newIndexes.Length;

            adapterDiff.DispatchUpdatesTo(this);
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private Task InsertItemsIfNeededAsync(IReadOnlyCollection<int> indexes)
        {
            return indexes.Any() ? InsertItemsAsync(indexes) : Task.CompletedTask;
        }

        private Task InsertItemsAsync(IReadOnlyCollection<int> indexes)
        {
            _itemsCount += indexes.Count;
            NotifyItemsInserted(indexes);
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private void NotifyItemsInserted(IEnumerable<int> indexes)
        {
            var consecutiveGroups = indexes.GroupConsecutive();
            foreach(var group in consecutiveGroups) {
                var array = group.ToArray();
                NotifyItemRangeInserted(array.First(), array.Count());
            }
        }
        
        public event EventHandler<int> ItemSelected;
        public override int ItemCount => _itemsCount;

        public TreeNode<Category> CurrentNode {
            set {
                _currentNode = value;
                _itemsCount = value.ParentNodes.Count + value.ChildNodes.Count + 1;
            }
        }
    }

    public class ParentChildListAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ParentChildListAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public ParentChildListAdapterViewHolder(View itemView, Action<int> itemSelectedAction = null) 
            : base(itemView)
        {
            TitleLabel = itemView.FindViewById<TextView>(Resource.Id.item_title_label);
            itemView.Click += (s, e) => itemSelectedAction?.Invoke(LayoutPosition);
        }

        public TextView TitleLabel { get; }

        public ParentChildItemState State {
            set => TitleLabel.SetBackgroundColor(GetColorForState(value));
        }
        
        private Color GetColorForState(ParentChildItemState state)
        {
            switch(state) {
                case ParentChildItemState.Root:
                    return Color.Blue;
                case ParentChildItemState.Parent:
                    return Color.Purple;
                case ParentChildItemState.Child:
                    return Color.Orange;
                case ParentChildItemState.Selected:
                    return Color.Red;
            }
            throw new ArgumentException($"Can't get color for unknown state {state}");
        }
        
    }
}