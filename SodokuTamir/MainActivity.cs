using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;

namespace SodokuTamir
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        PlayerAdapter adapter;
       
        public static List<Player> list = new List<Player>();
        Button btnStart, btnRecord;
        RadioButton Easy, Medium, Hard;
        EditText PlayerName;
        
        MediaPlayer mp;
        BroadcastBattery broadCastBattery;
        AudioManager am;
        Intent backgroundMusic;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            mp = MediaPlayer.Create(this, Resource.Raw.Song);
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

       

        public interface IBackButtonListener
        {
            void OnBackPressed();
        }
        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadCastBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }
        protected override void OnPause()
        {
            UnregisterReceiver(broadCastBattery);
            base.OnPause();
        }
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MusicMenu, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Resource.Id.action_startMusic)
            {
                backgroundMusic = new Intent(this, typeof(MusicService));

                StartService(backgroundMusic);
            }
            else if (item.ItemId == Resource.Id.action_stopMusic)
            {
                StopService(backgroundMusic);
            }
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
                intent.PutExtra("PlayerName",PlayerName.Text);
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