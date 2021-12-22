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

            this.lv = (ListView)FindViewById(Resource.Id.Lview);
            MainActivity.list.Add(new Player("Guy1", 200, "4th july 2020"));
            MainActivity.list.Add(new Player("Guy2", 20020, "4th july 2020"));
            MainActivity.list.Add(new Player("Girl", 100, "4th july 2020"));
            this.adapter = new PlayerAdapter(this, MainActivity.list);
            this.lv.Adapter = adapter;
            lv.ItemClick += Lv_ItemClick;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(DisplayRecord));
            int pos = e.Position;
            i.PutExtra("Position", pos);
            StartActivity(i);
        }
    }
}