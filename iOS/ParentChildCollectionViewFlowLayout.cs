using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ParentChildListView.UI.iOS
{
    public sealed class ParentChildCollectionViewFlowLayout : UICollectionViewFlowLayout
    {
        private readonly List<NSIndexPath> _insertingIndexPaths = new List<NSIndexPath>();
        private readonly List<NSIndexPath> _removedIndexPaths = new List<NSIndexPath>();
        
        public override void PrepareForCollectionViewUpdates(UICollectionViewUpdateItem[] updateItems)
        {
            base.PrepareForCollectionViewUpdates(updateItems);

            _insertingIndexPaths.Clear();
            _removedIndexPaths.Clear();
            
            foreach(var updateItem in updateItems) {
                if(updateItem.UpdateAction == UICollectionUpdateAction.Insert) {
                    _insertingIndexPaths.Add(updateItem.IndexPathAfterUpdate);
                } else if(updateItem.UpdateAction == UICollectionUpdateAction.Delete) {
                    _removedIndexPaths.Add(updateItem.IndexPathBeforeUpdate);
                }
            }
        }

        public override void FinalizeCollectionViewUpdates()
        {
            base.FinalizeCollectionViewUpdates();
            _insertingIndexPaths.Clear();
            _removedIndexPaths.Clear();
        }

        public override UICollectionViewLayoutAttributes InitialLayoutAttributesForAppearingItem(NSIndexPath itemIndexPath)
        {
            var attributes = base.InitialLayoutAttributesForAppearingItem(itemIndexPath);
            var index = _insertingIndexPaths.FindIndex(0, x => x.Equals(itemIndexPath));
            
            if(index >= 0) {
                var centerY = attributes.Frame.Y - (attributes.Bounds.Height / 2 - attributes.Frame.Height / 2);
                attributes.Center = new CGPoint(attributes.Bounds.GetMidX(), centerY);
                var transform = CGAffineTransform.MakeIdentity();
                
                if(index > 0) {
                    transform.Translate(0, -attributes.Bounds.Height * 10 * index);
                }
                
                transform.Scale(1f, 0.1f);
                attributes.Transform = transform;
                attributes.Alpha = 1f;
            }
            
            return attributes;
        }

        public override UICollectionViewLayoutAttributes FinalLayoutAttributesForDisappearingItem(NSIndexPath itemIndexPath)
        {
            var attributes =  base.FinalLayoutAttributesForDisappearingItem(itemIndexPath);
            var index = _removedIndexPaths.FindIndex(0, x => x.Equals(itemIndexPath));
            
            if(index >= 0) {
                var centerY = attributes.Frame.Y - (attributes.Bounds.Height / 2 - attributes.Frame.Height / 2);
                attributes.Transform = CGAffineTransform.MakeScale(1f, 0.1f);

                if(index == 0) {
                    attributes.Center = new CGPoint(attributes.Bounds.GetMidX(), centerY);
                } else {
                    attributes.Center = new CGPoint(attributes.Bounds.GetMidX(), centerY - attributes.Bounds.Height * index);
                }
                
                attributes.Alpha = 1f;
            }
            return attributes;
        }
    }
}