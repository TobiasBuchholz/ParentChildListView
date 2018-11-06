using System;
using Android.Animation;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
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

            var flatRecyclerView = FindViewById<RecyclerView>(Resource.Id.flat_recyclerview);
            var flatLayoutManager = new PredictiveLinearLayoutManager(this);
            var flatAdapter = new CategoriesAdapter(flatRecyclerView);
            flatRecyclerView.SetLayoutManager(flatLayoutManager);
            flatRecyclerView.SetAdapter(flatAdapter);
            flatAdapter.CurrentNode = Category.CreateFlatDummyCategories().ToRootTreeNodes()[0];
            flatAdapter.NotifyDataSetChanged();
           
            
            var recyclerView = new RecyclerView(this);
            var adapter = new CategoriesAdapter(recyclerView);
            recyclerView.SetLayoutManager(new PredictiveLinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);
            adapter.CurrentNode = Category.CreateDummyCategories().ToRootTreeNodes()[0];
            adapter.NotifyDataSetChanged();

            var flapView = FindViewById<FlapView>(Resource.Id.flap_view);
            flapView.Title = "Filter";
            flapView.ContentView = recyclerView;
            recyclerView.AddOnItemTouchListener(new RecyclerViewOutSideTouchListener(flapView.Close));

//            adapter.HeightWillChange += (s, e) => {
//                var animation = new ResizeAnimation(recyclerView, recyclerView.Height, e);
//                animation.Duration = 250;
//                recyclerView.StartAnimation(animation);
//            };
        }
    }
}

