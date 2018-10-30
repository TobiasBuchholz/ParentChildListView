using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public class ParentChildListDataSourceDelegate<T> where T : ITreeNodeData
    {
        private readonly Func<UICollectionView, NSIndexPath, ParentChildItemState, T, UICollectionViewCell> _cellSelector;
        private TreeNode<T> _currentNode;

        public ParentChildListDataSourceDelegate(Func<UICollectionView, NSIndexPath, ParentChildItemState, T, UICollectionViewCell> cellSelector)
        {
            _cellSelector = cellSelector;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var state = GetStateForIndex(indexPath.Row);
            var data = GetDataForIndex(indexPath.Row, state);
            return _cellSelector(collectionView, indexPath, state, data);
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
        
        private T GetDataForIndex(int index, ParentChildItemState state)
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

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var parentNodes = _currentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;
            var index = indexPath.Row;

            if(index == 0) {
                SetCurrentNodeWithAnimation(collectionView, indexPath, _currentNode.ParentNodes.First());
            } else if(index < parentNodesCount) {
                SetCurrentNodeWithAnimation(collectionView, indexPath, parentNodes[index]);
            } else if(index > parentNodesCount) {
                SetCurrentNodeWithAnimation(collectionView, indexPath, _currentNode.ChildNodes[index - (parentNodesCount + 1)]);
            }
        }

        private void SetCurrentNodeWithAnimation(UICollectionView collectionView, NSIndexPath indexPath, TreeNode<T> selectedNode)
        {
            var diffResult = _currentNode.CalculateDiff(selectedNode);
            var previousNodeFlattened = _currentNode.Flatten().ToArray();
            var movingIndexes = diffResult.MovingIndexes.ToArray();
            
            foreach(var i in movingIndexes) {
                var previousNode = previousNodeFlattened[i];
                var state = GetStateForPreviousNode(previousNode, selectedNode);
                var cell = (CategoryCell) collectionView.CellForItem(NSIndexPath.FromRowSection(i, 0));
                cell.State = i == indexPath.Row && i > 0 ? ParentChildItemState.Selected : state;
            }
            
            _currentNode = selectedNode;
            AnimateDiffAsync(collectionView, diffResult).Ignore();
        }

        private static ParentChildItemState GetStateForPreviousNode(TreeNode<T> previousNode, TreeNode<T> currentNode)
        {
            if(previousNode.ParentNodes.Any()) {
                return currentNode.ChildNodes.Contains(previousNode) ? ParentChildItemState.Child : ParentChildItemState.Parent;
            } else {
                return ParentChildItemState.Root;
            }
        }

        private async Task AnimateDiffAsync(UICollectionView collectionView, DiffResult diffResult)
        {
            await DeleteItemsAsync(collectionView, diffResult.RemovedIndexes.ToArray());
            await InsertItemsAsync(collectionView, diffResult.AddedIndexes.ToArray());
        }

        private async Task DeleteItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> indexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                ItemsCount -= indexes.Count;
                collectionView.DeleteItems(indexes.ToNSIndexPaths().ToArray());
            });
        }
        
        private async Task InsertItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> indexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                ItemsCount += indexes.Count;
                collectionView.InsertItems(indexes.ToNSIndexPaths().ToArray());
            });
        }
        
        public int ItemsCount { get; private set; }
        public int NumberOfSections => 1;
        
        public TreeNode<T> CurrentNode {
            set {
                _currentNode = value;
                ItemsCount = value.ParentNodes.Count + value.ChildNodes.Count + 1;
            }
        }
    }
}