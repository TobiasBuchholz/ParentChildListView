using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using ParentChildListView.Core;
using ParentChildListView.Core.TreeNodes;

namespace ParentChildListView.UI.Droid
{
    [Activity(Label = "ParentChildListView", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);
            var layoutManager = new PredictiveLinearLayoutManager(this);
            var adapter = new CategoriesAdapter(recyclerView);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);
            adapter.CurrentNode = Category.CreateDummyCategories().ToRootTreeNodes()[0];
            adapter.NotifyDataSetChanged();
        }
    }
}

