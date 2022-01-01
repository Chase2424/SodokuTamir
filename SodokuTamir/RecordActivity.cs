using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    [Activity(Label = "RecordActivity")]
    public class RecordActivity : Activity
    {
        ListView lv;
        PlayerAdapter adapter;
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recordlayout);
            // Create your application here
            setPermissitios();
            

            this.lv = (ListView)FindViewById(Resource.Id.Lview);

            //MainActivity.list.Add(new Player("Guy1", 200, "4th july 2020"));
            //MainActivity.list.Add(new Player("Guy2", 20020, "4th july 2020"));
            //MainActivity.list.Add(new Player("Girl", 100, "4th july 2020"));
            MainActivity.list.Clear();
            ReadRecordFiles();
            lv.ItemClick += Lv_ItemClick;
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(DisplayRecord));
            int pos = e.Position;
            i.PutExtra("Position", pos);
            StartActivity(i);
        }
        public void ReadRecordFiles()
        {
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
            string game_folder = root + "/saved_sodokus";
            string[] files = Directory.GetFiles(game_folder);
            foreach (string oCurrent in files)
            {
                string text = System.IO.File.ReadAllText(oCurrent);
                string PlayerName = text.Split(",")[0];
                string duration = text.Split(",")[1];
                string Date = text.Split(",")[2];
                string Board = text.Split(",")[3];




                MainActivity.list.Add(new Player(PlayerName, duration, Date, StringToBoard(Board)));
            }
            this.adapter = new PlayerAdapter(this, MainActivity.list);
            this.lv.Adapter = adapter;
        }
        public SudokuCell[,] StringToBoard(string str)
        {

            SudokuCell[,] arr = new SudokuCell[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    arr[i, j] = new SudokuCell(j * 120, i * 120, (Char)str[i * 9 + j] - '0', 120, 120, this);
                }
            }
            return arr;
        }
        public void setPermissitios()
        {
            string state = Android.OS.Environment.ExternalStorageState;
            if (Android.OS.Environment.MediaMounted.Equals(state))
            {
                //We can read and write the media
                mExternalStorageAvailable = mExternalStorageWriteable = true;
                //Toast.MakeText(this, "We can read and write the media", ToastLength.Long).Show();
            }
            else if (Android.OS.Environment.MediaMountedReadOnly.Equals(state))
            {
                //We can only read the media
                mExternalStorageAvailable = true;
                mExternalStorageWriteable = false;
                //Toast.MakeText(this, "We can only read the media", ToastLength.Long).Show();
            }
            else
            {
                //Something else is wrong. we can neither read nor write
                mExternalStorageAvailable = mExternalStorageWriteable = false;
                //Toast.MakeText(this, "Something else is wrong. we can neither read nor write", ToastLength.Long).Show();
            }
        }
    }
}