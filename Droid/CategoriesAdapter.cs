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

        private static void HandleViewHolderBound(RecyclerView.ViewHolder holder, ParentChildItemState state, Category category)
        {
            var viewHolder = (CategoriesAdapterViewHolder) holder;
            viewHolder.TitleLabel.Text = category.Name;
            viewHolder.State = state;
        }

        private static void HandleItemStateChanged(RecyclerView.ViewHolder holder, ParentChildItemState state)
        {
            var viewHolder = (CategoriesAdapterViewHolder) holder;
            viewHolder.State = state;
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

        public ParentChildItemState State {
            set => TitleLabel.SetBackgroundColor(GetColorForState(value));
        }
        
        private Color GetColorForState(ParentChildItemState state)
        {
            switch(state) {
                case ParentChildItemState.Root:
                    return Color.Blue;
                case ParentChildItemState.Parent:
                    return Color.Purple;
                case ParentChildItemState.Child:
                    return Color.Orange;
                case ParentChildItemState.Selected:
                    return Color.Red;
            }
            throw new ArgumentException($"Can't get color for unknown state {state}");
        }
    }
}