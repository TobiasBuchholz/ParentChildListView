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
        private readonly nfloat _cellHeight;
        private readonly Func<UICollectionView, NSIndexPath, ItemRelation, T, UICollectionViewCell> _cellSelector;
        private readonly Action<UICollectionViewCell, ItemRelation> _itemStateChanged;
        private TreeNode<T> _currentNode;

        public ParentChildListDataSourceDelegate(
            nfloat cellHeight,
            Func<UICollectionView, NSIndexPath, ItemRelation, T, UICollectionViewCell> cellSelector,
            Action<UICollectionViewCell, ItemRelation> itemStateChanged)
        {
            _cellHeight = cellHeight;
            _cellSelector = cellSelector;
            _itemStateChanged = itemStateChanged;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var relation = GetRelationForIndex(indexPath.Row);
            var data = GetDataForIndex(indexPath.Row, relation);
            return _cellSelector(collectionView, indexPath, relation, data);
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

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var parentNodes = _currentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;
            var index = indexPath.Row;

            if(index == 0 && _currentNode.ParentNodes.Any()) {
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
                var cell = collectionView.CellForItem(NSIndexPath.FromRowSection(i, 0));
                var relation = i == indexPath.Row && i > 0 ? ItemRelation.AsSelected(selectedNode) : GetStateForPreviousNode(previousNodeFlattened[i], selectedNode);
                _itemStateChanged(cell, relation);
            }
            
            _currentNode = selectedNode;
            AnimateDiffAsync(collectionView, diffResult).Ignore();
        }

        private static ItemRelation GetStateForPreviousNode(TreeNode<T> previousNode, TreeNode<T> currentNode)
        {
            return currentNode.ChildNodes.Contains(previousNode)
                ? ItemRelation.AsChild(previousNode)
                : ItemRelation.AsParent(previousNode);
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
                InvokeHeightWillChange();
                collectionView.DeleteItems(indexes.ToNSIndexPaths().ToArray());
            });
        }

        private void InvokeHeightWillChange()
        {
            HeightWillChange?.Invoke(this, ItemsCount * _cellHeight);
        }

        private async Task InsertItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> indexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                ItemsCount += indexes.Count;
                InvokeHeightWillChange();
                collectionView.InsertItems(indexes.ToNSIndexPaths().ToArray());
            });
        }
        
        public int ItemsCount { get; private set; }
        public int NumberOfSections => 1;
        
        public TreeNode<T> CurrentNode {
            set {
                _currentNode = value;
                ItemsCount = value.ParentNodes.Count + value.ChildNodes.Count + 1;
                InvokeHeightWillChange();
            }
        }

        public event EventHandler<nfloat> HeightWillChange;
    }
}