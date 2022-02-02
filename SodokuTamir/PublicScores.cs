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
    [Activity(Label = "PublicScores")]
    public class PublicScores : Activity
    {
        ListView lv;
        PlayerAdapter adapter;
       
        public static List<Player> listPublic = new List<Player>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PublicScores);
            // Create your application here
            this.lv = (ListView)FindViewById(Resource.Id.publicLview);
            doStuff();
            lv.ItemClick += Lv_ItemClick;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(DisplayRecord));
            int pos = e.Position;
            i.PutExtra("Position", pos);
            i.PutExtra("Type","Global");
            StartActivity(i);
        }

        public async void doStuff()
        {
            List<Player> decoy = new List<Player>();
            decoy = await FirebaseUser.GetAll();
            for (int i=0;i<decoy.Count();i++)
            {
               
                listPublic.Add(decoy[i]);   
            }
           
            this.adapter = new PlayerAdapter(this, listPublic,"PublicScores");
            this.lv.Adapter = adapter;
        }
    }
}