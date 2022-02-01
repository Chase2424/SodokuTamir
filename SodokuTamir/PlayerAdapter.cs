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
    class PlayerAdapter : BaseAdapter<Player>
    {
        Android.Content.Context context;
        List<Player> objects;
        Android.Views.LayoutInflater layoutInflater;
        public string activity;
        //see if it works
        public PlayerAdapter(Android.Content.Context context, System.Collections.Generic.List<Player> objects,string activity)
        {
            this.context = context;
            this.objects = objects;
            this.activity = activity;
        }
        public List<Player> GetList()
        {
            return this.objects;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get
            {
                return this.objects.Count;
            }
        }
        public override Player this[int position]
        {
            get
            {
                return this.objects[position];
            }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (activity.Equals("RecordActivity"))
            {
                layoutInflater = ((RecordActivity)context).LayoutInflater;
            }
            else
            {
                layoutInflater = ((PublicScores)context).LayoutInflater;
            }
            Android.Views.View view = layoutInflater.Inflate(Resource.Layout.PlayerLayout, parent, false);
            TextView Name = view.FindViewById<TextView>(Resource.Id.name);
            TextView Time = view.FindViewById<TextView>(Resource.Id.time);
            // ImageView Array = view.FindViewById<ImageView>(Resource.Id.Array);
            TextView Date = view.FindViewById<TextView>(Resource.Id.date);
            Player temp = objects[position];
            if (temp != null)
            {
                Name.Text = "" + temp.getName();
                Time.Text = "Time:" + temp.getTime();
                //Array.SetImageBitmap(temp.getArray());
                Date.Text = "Date:" + temp.getDate();

            }
            return view;
        }
        
    }
}