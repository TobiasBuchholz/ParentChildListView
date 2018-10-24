using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using ParentChildListView.UI.TreeNodes;
using PressMatrix.Utility.TreeNodes;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class ParentChildListDataSource : UICollectionViewDataSource
    {
        private TreeNode<Category> _currentNode;
        private int _itemsCount;
        
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _itemsCount;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CategoryCell) collectionView.DequeueReusableCell(CategoryCell.CellIdentifier, indexPath);
            var parentNodes = CurrentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;
            var index = indexPath.Row;

            if(index == 0) {
                cell.SetupRootCell(parentNodes.Any() ? parentNodes.First().Data : CurrentNode.Data);
            } else if(index == parentNodesCount) {
                cell.SetupSelectedCell(CurrentNode.Data);
            } else if(index < parentNodesCount) {
                cell.SetupParentCell(parentNodes[index].Data);
            } else {
                cell.SetupChildCell(CurrentNode.ChildNodes[index - (parentNodesCount + 1)].Data);
            }
            return cell;
        }

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CategoryCell) collectionView.CellForItem(indexPath);
            var parentNodes = CurrentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;
            var index = indexPath.Row;

            if(index == 0) {
                var previousNode = CurrentNode;
                CurrentNode = CurrentNode.ParentNodes.First();
                AnimateSelectionAsync(collectionView, previousNode.CalculateDiff(CurrentNode));
            } else if(index < parentNodesCount) {
                var previousNode = CurrentNode;
                CurrentNode = parentNodes[index];
                AnimateSelectionAsync(collectionView, previousNode.CalculateDiff(CurrentNode));
            } else if(index > parentNodesCount) {
                var previousNode = CurrentNode;
                CurrentNode = CurrentNode.ChildNodes[index - (parentNodesCount + 1)];
                AnimateSelectionAsync(collectionView, previousNode.CalculateDiff(CurrentNode));
            }

            if(index > 0) {
                cell.SetupSelectedCell(CurrentNode.Data);
            }
        }

        private async Task AnimateSelectionAsync(UICollectionView collectionView, DiffResult diffResult)
        {
            await DeleteItemsAsync(collectionView, diffResult.RemovedIndexes.ToArray());
            await InsertItemsAsync(collectionView, diffResult.AddedIndexes.ToArray());
        }

        private async Task DeleteItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> removedIndexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                _itemsCount -= removedIndexes.Count;
                collectionView.DeleteItems(removedIndexes.ToNSIndexPaths().ToArray());
            });
        }
        
        private async Task InsertItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> addedIndexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                _itemsCount += addedIndexes.Count;
                collectionView.InsertItems(addedIndexes.ToNSIndexPaths().ToArray());
            });
        }

        public TreeNode<Category> CurrentNode {
            private get => _currentNode;
            set {
                if(_currentNode == null) {
                    _itemsCount = value.ChildNodes.Count + value.ParentNodes.Count + 1;
                }
                _currentNode = value;
            }
        }
    }
}