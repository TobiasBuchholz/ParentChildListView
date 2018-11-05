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
        
        public CategoriesDataSource(nfloat cellHeight) 
        {
            _delegate = new ParentChildListDataSourceDelegate<Category>(cellHeight, GetCell, HandleItemStateChanged);
        }

        private static UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath, ItemRelation relation, Category category)
        {
            var cell = (CategoryCell) collectionView.DequeueReusableCell(CategoryCell.CellIdentifier, indexPath);
            cell.SetupCell(category);
            cell.Relation = relation;
            return cell;
        }
        
        private static void HandleItemStateChanged(UICollectionViewCell collectionViewCell, ItemRelation relation)
        {
            var cell = (CategoryCell) collectionViewCell;
            cell.Relation = relation;
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
        
        public event EventHandler<nfloat> HeightWillChange {
            add => _delegate.HeightWillChange += value;
            remove => _delegate.HeightWillChange -= value;
        }
    }
}