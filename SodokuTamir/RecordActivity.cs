using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    [Activity(Label = "RecordActivity")]
    public class RecordActivity : Activity
    {
        ListView lv;
        PlayerAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recordlayout);
            // Create your application here

            lv = (ListView)FindViewById(Resource.Id.Lview);
            adapter = new PlayerAdapter(this, MainActivity.list);
            lv.Adapter = adapter;
        }
    }
}