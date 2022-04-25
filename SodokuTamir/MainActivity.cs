using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SodokuTamir
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity 
    {
        PlayerAdapter adapter;
        public static ISharedPreferences SP;
        public static List<Player> list = new List<Player>();
        Button btnStart, btnRecord;
        RadioButton Easy, Medium, Hard;
        EditText PlayerName;
        
        public static MediaPlayer mp;
        BroadcastBattery broadCastBattery;
        AudioManager am;
        public static Intent backgroundMusic;
        Android.Views.IMenu menu;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SP = PreferenceManager.GetDefaultSharedPreferences(this);
            backgroundMusic = new Intent(this, typeof(MusicService));
            try
            {
                if (SP.GetString("Pathmp3", "nothing").Equals("nothing"))
                {
                    mp = MediaPlayer.Create(this, Resource.Raw.Song);
                }
                else
                {
                    mp = MediaPlayer.Create(this, Android.Net.Uri.Parse(SP.GetString("Pathmp3", "nothing")));
                }
            }
            catch//יקרה מתי שהקובץ היה קיים אך נמחק
            {
                mp = MediaPlayer.Create(this, Resource.Raw.Song);
            }
                am = (AudioManager)GetSystemService(Context.AudioService);
            broadCastBattery = new BroadcastBattery(this);
            btnStart = (Button)FindViewById(Resource.Id.start);
            btnRecord = (Button)FindViewById(Resource.Id.Record);
            Easy = (RadioButton)FindViewById(Resource.Id.Easy);
            Medium = (RadioButton)FindViewById(Resource.Id.Medium);
            Hard = (RadioButton)FindViewById(Resource.Id.Hard);
            PlayerName = (EditText)FindViewById(Resource.Id.PlayerName);
            btnStart.Click += BtnStart_Click;
            btnRecord.Click += BtnRecord_Click;
        }




        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadCastBattery, new IntentFilter(Intent.ActionBatteryChanged));
            if (this.menu != null)
            {
                if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
                {
                    this.menu.GetItem(0).SetVisible(true);
                    this.menu.GetItem(1).SetVisible(false);
                }
                else
                {
                    this.menu.GetItem(0).SetVisible(false);
                    this.menu.GetItem(1).SetVisible(true);

                }
            }

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopService(MainActivity.backgroundMusic);
        }
        protected override void OnPause()
        {
            UnregisterReceiver(broadCastBattery);
            base.OnPause();
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            this.menu = menu;
            MenuInflater.Inflate(Resource.Menu.MusicMenu, this.menu);
            this.menu.GetItem(2).SetVisible(true);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            else if (MainActivity.SP.GetBoolean("IsMusicOn", false) == true)
            {
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);
                StartService(MainActivity.backgroundMusic);
            }
            return true;
        }
        public async void PickFile()
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    String Text = $"File Name: {result.FileName}";
                    if (result.FileName.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        //Toast.MakeText(this, ""+ result.FullPath, ToastLength.Long).Show();
                        mp = MediaPlayer.Create(this, Android.Net.Uri.Parse(result.FullPath));
                        var editor = SP.Edit();
                        editor.PutString("Pathmp3", result.FullPath);
                        editor.Commit();

                    }
                    else
                    {
                        Toast.MakeText(this, "Please select a mp3 file", ToastLength.Long).Show();
                    }
                }

                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ""+ex, ToastLength.Long).Show();
            }

            
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Resource.Id.MusicFile)
            {
                PickFile();
               
            }           
            else if (SP.GetBoolean("IsMusicOn", false) == false)
            {
                var editor = SP.Edit();
                editor.PutBoolean("IsMusicOn", true);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);           
                StartService(MainActivity.backgroundMusic);
                
            }
            else
            {
                var editor = SP.Edit();
                editor.PutBoolean("IsMusicOn", false);
                StopService(MainActivity.backgroundMusic);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }

            /*
            if (item.ItemId == Resource.Id.action_startMusic)
            {
                if (MainActivity.menu.GetItem(0).IsVisible)
                {

                    MainActivity.menu.GetItem(0).SetVisible(false);
                    MainActivity.menu.GetItem(1).SetVisible(true);
                    StartService(MainActivity.backgroundMusic);
                }
            }
            else if (item.ItemId == Resource.Id.action_stopMusic)
            {
                if (MainActivity.menu.GetItem(1).IsVisible)
                {
                    MainActivity.menu.GetItem(1).SetVisible(false);
                    MainActivity.menu.GetItem(0).SetVisible(true);
                    StopService(MainActivity.backgroundMusic);
                }
            }*/
            return true;
        
         }
        private void BtnRecord_Click(object sender, System.EventArgs e)
        {


            Dialog d = new Dialog(this);
            d.SetContentView(Resource.Layout.OutsideInside);
            Button privately = (Button)d.FindViewById(Resource.Id.privateresult);
            Button globally = (Button)d.FindViewById(Resource.Id.globalresult);
            d.Show();
            privately.Click += Privately_Click;

            globally.Click += Globally_Click;
           
        }

        private void Globally_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PublicScores));

            StartActivity(intent);
        }

        private void Privately_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RecordActivity));

            StartActivity(intent);
        }

        private void BtnStart_Click(object sender, System.EventArgs e)
        {

            if (PlayerName.Text != "" && !(PlayerName.Text.Contains(",")))
            {

                Intent intent = new Intent(this, typeof(SodokuActivity));
                if (Hard.Checked)
                {
                    intent.PutExtra("difficulty", 3);
                }
                else if (Medium.Checked)
                {
                    intent.PutExtra("difficulty", 2);
                }
                else
                {
                    intent.PutExtra("difficulty", 1);
                }
                intent.PutExtra("PlayerName", PlayerName.Text);
                
               
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Please enter your name only with the a-z and numbers", ToastLength.Long).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}