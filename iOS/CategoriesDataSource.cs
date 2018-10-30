using System;
using Foundation;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class CategoriesDataSource : UICollectionViewDataSource
    {
        private readonly ParentChildListDataSourceDelegate<Category> _delegate;
        
        public CategoriesDataSource() 
        {
            _delegate = new ParentChildListDataSourceDelegate<Category>(GetCell);
        }

        private static UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath, ParentChildItemState state, Category category)
        {
            var cell = (CategoryCell) collectionView.DequeueReusableCell(CategoryCell.CellIdentifier, indexPath);
            cell.SetupCell(category);
            cell.State = state;
            return cell;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return _delegate.GetCell(collectionView, indexPath);
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _delegate.ItemsCount;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return _delegate.NumberOfSections;
        }

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _delegate.ItemSelected(collectionView, indexPath);
        }

        public TreeNode<Category> CurrentNode {
            set => _delegate.CurrentNode = value;
        }
    }
}