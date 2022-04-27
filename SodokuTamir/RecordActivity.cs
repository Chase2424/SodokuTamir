using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    [Activity(Label = "RecordActivity")]
    public class RecordActivity : AppCompatActivity
    {
        ListView lv;
        PlayerAdapter adapter;        
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;
        Android.Views.IMenu menu;
        public static string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
        public static string game_folder = Path.Combine(root, "saved_sodokus");
        public static string record_file = Path.Combine(game_folder, "records.txt");

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
            if(System.IO.File.Exists(record_file))
                ReadRecordFiles();
            else
            {
                Toast.MakeText(this, "No records found", ToastLength.Long).Show();
            }
            lv.ItemClick += Lv_ItemClick;
            
        }

        

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
           this.menu = menu;
            MenuInflater.Inflate(Resource.Menu.MainMenu, this.menu);
            this.menu.GetItem(2).SetVisible(false);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            else if (MainActivity.SP.GetBoolean("IsMusicOn", false) == true)
            {
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);
                
            }
            return true;
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if(item.ItemId == Resource.Id.DeleteRecords)
            {
                if(System.IO.File.Exists(record_file))
                     System.IO.File.Delete(record_file);
                Finish();
            }
            else if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", true);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);
                StartService(MainActivity.backgroundMusic);

            }
            else 
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", false);
                StopService(MainActivity.backgroundMusic);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            return true;

        }
        
        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(DisplayRecord));
            int pos = e.Position;
            i.PutExtra("Position", pos);
            i.PutExtra("Type", "Private");
            StartActivity(i);
        }
        public void ReadRecordFiles()
        {
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
            //TODO check if file exists
            String[] lines = System.IO.File.ReadAllLines(record_file);
            foreach (string oCurrent in lines)
            {
                string PlayerName = oCurrent.Split(",")[0];
                string duration = oCurrent.Split(",")[1];
                string Date = oCurrent.Split(",")[2];
                string Board = oCurrent.Split(",")[3];
                MainActivity.list.Add(new Player(PlayerName, duration, Date, StringToBoard(Board)));
            }
            this.adapter = new PlayerAdapter(this, MainActivity.list,"RecordActivity");
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