using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace ParentChildListView.UI.iOS
{
	public abstract class BaseCollectionViewSource<T> : UICollectionViewSource
	{
        private IList<T> _items;

		public BaseCollectionViewSource()
		{
            _items = new List<T>();
		}

		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			collectionView.CellForItem(indexPath).Alpha = 0.7f;
		}

		public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			collectionView.CellForItem(indexPath).Alpha = 1f;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return Items.Count;
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override void Scrolled(UIScrollView scrollView)
		{
            OnScrolled?.Invoke(scrollView, System.EventArgs.Empty);
		}

		protected virtual UICollectionReusableView GetViewForSupplementaryElementImpl(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			return null;
		}

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            var view = GetViewForSupplementaryElementImpl(collectionView, elementKind, indexPath);
            return view;
        }

		public IList<T> Items { 
            get => _items; 
            set {
                if(value != null) {
                    _items = value;
                } 
            }
        }

        public event EventHandler OnScrolled;
	}
}

