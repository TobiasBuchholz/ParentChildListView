using System;
using UIKit;

namespace UIKit
{
    public static class UICollectionViewExtensions
    {
        public static void AddItemLongClickListener(this UICollectionView collectionView, Action<UICollectionViewCell> action)
        {
            var recognizerToAdd = new UILongPressGestureRecognizer((recognizer) => {
                if(recognizer.State != UIGestureRecognizerState.Ended) {
                    var point = recognizer.LocationInView(collectionView);
                    var indexPath = collectionView.IndexPathForItemAtPoint(point);
                    if(indexPath != null) {
                        action?.Invoke(collectionView.CellForItem(indexPath));
                    }
                }
            });
            recognizerToAdd.DelaysTouchesBegan = true;
            collectionView.AddGestureRecognizer(recognizerToAdd);
        } 
        
        public static int NumberOfItems(this UICollectionView collectionView)
        {
            var childCount = 0;
            for(nint i = 0, numberOfSections = collectionView.NumberOfSections(); i < numberOfSections; i++) {
                childCount += (int) collectionView.NumberOfItemsInSection(i);
            }
            return childCount;
        }
    }
}