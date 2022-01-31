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
        List<Player> listPublic = new List<Player>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PublicScores);
            // Create your application here
            this.lv = (ListView)FindViewById(Resource.Id.publicLview);
            doStuff();
            
        }
       
        public async void doStuff()
        {
            List<Player> decoy = new List<Player>();
            decoy = await FirebaseUser.GetAll();
            for (int i=0;i<decoy.Count();i++)
            {
                listPublic.Add(decoy[i]);   
            }
           
            this.adapter = new PlayerAdapter(this, listPublic);
            this.lv.Adapter = adapter;
        }
    }
}