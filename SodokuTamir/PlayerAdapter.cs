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
        /// <summary>
        /// מחלקה המתאמת בין שחקן לאובייקט רשימה בשם ListView
        /// מטרת הקוד לקשר בין מחלקת המשתמש לתצוגה על המסך של המשתמש
        /// </summary>

        Android.Content.Context context;// נתינת הקשר למקום ממנו הופעל ההתאמה
        List<Player> objects;//רשימת שחקנים
        Android.Views.LayoutInflater layoutInflater;//התצוגה של הקוד
        public string activity;
        
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
        /// <summary>
        /// פונקציה המתאמת בין מידע המשתמשים לתצוגה על אובייקט ה ListView
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
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