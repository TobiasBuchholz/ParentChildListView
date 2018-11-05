using System;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;

namespace ParentChildListView.UI.Droid
{
    public sealed class CategoriesAdapter : RecyclerView.Adapter
    {
        private readonly ParentChildListAdapterDelegate<Category> _delegate;
        
        public CategoriesAdapter(RecyclerView recyclerView)
        {
            _delegate = new ParentChildListAdapterDelegate<Category>(
                recyclerView.Context.Resources.GetDimensionPixelSize(Resource.Dimension.height_category_item),
                recyclerView,
                SelectViewHolder,
                HandleViewHolderBound,
                HandleItemStateChanged);
        }

        private RecyclerView.ViewHolder SelectViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item, parent, false);
            return new CategoriesAdapterViewHolder(itemView, OnItemSelected);
        }
        
        private void OnItemSelected(int index)
        {
            ItemSelected?.Invoke(this, index);
            _delegate.OnItemSelected(index);    
        }

        private static void HandleViewHolderBound(RecyclerView.ViewHolder holder, ItemRelation relation, Category category)
        {
            var viewHolder = (CategoriesAdapterViewHolder) holder;
            viewHolder.TitleLabel.Text = category.Name;
            viewHolder.Relation = relation;
        }

        private static void HandleItemStateChanged(RecyclerView.ViewHolder holder, ItemRelation relation)
        {
            var viewHolder = (CategoriesAdapterViewHolder) holder;
            viewHolder.Relation = relation;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _delegate.OnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _delegate.OnCreateViewHolder(parent, viewType);
        }

        public event EventHandler<int> ItemSelected;
        public override int ItemCount => _delegate.ItemCount;
        
        public TreeNode<Category> CurrentNode {
            set => _delegate.CurrentNode = value;
        }
        
        public event EventHandler<int> HeightWillChange {
            add => _delegate.HeightWillChange += value;
            remove => _delegate.HeightWillChange -= value;
        }
    }
    
    public sealed class CategoriesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public CategoriesAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public CategoriesAdapterViewHolder(View itemView, Action<int> itemSelectedAction = null) 
            : base(itemView)
        {
            TitleLabel = itemView.FindViewById<TextView>(Resource.Id.item_title_label);
            itemView.Click += (s, e) => itemSelectedAction?.Invoke(LayoutPosition);
        }

        public TextView TitleLabel { get; }

        public ItemRelation Relation {
            set => TitleLabel.SetBackgroundColor(GetColorForState(value));
        }
        
        private static Color GetColorForState(ItemRelation relation)
        {
            switch(relation.Type) {
                case ItemRelationType.Parent:
                    return relation.Level == 0 ? Color.Blue : Color.Purple;
                case ItemRelationType.Child:
                    return Color.Orange;
                case ItemRelationType.Selected:
                    return Color.Red;
            }
            throw new ArgumentException($"Can't get color for unknown state {relation}");
        } 
    }
}