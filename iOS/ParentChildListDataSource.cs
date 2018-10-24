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
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return CurrentNode.ChildNodes.Count + CurrentNode.ParentNodes.Count + 1;
//            return Items.Count;
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
            
//            cell.SetupChildCell(Items[indexPath.Row]);
//            return cell;
        }

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var parentNodes = CurrentNode.ParentNodes;
            var parentNodesCount = parentNodes.Count;
            var index = indexPath.Row;

            if(index == 0) {
                CurrentNode = CurrentNode.ParentNodes.First();
            } else if(index < parentNodesCount) {
                CurrentNode = parentNodes[index];
            } else if(index > parentNodesCount) {
                CurrentNode = CurrentNode.ChildNodes[index - (parentNodesCount + 1)];
            }
            collectionView.ReloadData();
        }

//        private async Task AnimateStuff(UICollectionView collectionView, NSIndexPath indexPath)
//        {
//            await collectionView.PerformBatchUpdatesAsync(() => {
//                var removedIndexPaths = Enumerable.Range(indexPath.Row + 1, 3).Select(x => NSIndexPath.FromRowSection(x, 0)).ToArray();
//
//                Items.RemoveAt(removedIndexPaths[0].Row);
//                Items.RemoveAt(removedIndexPaths[1].Row);
//                Items.RemoveAt(removedIndexPaths[2].Row);
//                collectionView.DeleteItems(removedIndexPaths);
//            });
//
//            await collectionView.PerformBatchUpdatesAsync(() => {
//                var addedIndexes = Enumerable.Range(indexPath.Row + 1, 3).ToArray();
//                Items.InsertRange(indexPath.Row + 1, addedIndexes.Select(x => new Category(0, x, $"added #{x}")));
//                var addedIndexPaths = addedIndexes.Select(x => NSIndexPath.FromRowSection(x, 0)).ToArray();
//
//                collectionView.InsertItems(addedIndexPaths);
//            });
//        }
        
        public TreeNode<Category> CurrentNode { private get; set; }
//        public List<Category> Items { get; set; }
    }
}