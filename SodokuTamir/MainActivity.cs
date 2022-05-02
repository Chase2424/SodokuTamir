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
        /// <summary>
        /// המחלקה הראשית המגשרת בין כל אבני הפרויקט 
        /// </summary>
        public static ISharedPreferences SP;     
        Button btnStart, btnRecord;// כפתור להתחלת המשחק וכפתור ללראות שיאים
        RadioButton Easy, Medium, Hard;// כפתורי רדיו לבחירת רמת הקושי
        EditText PlayerName;// שם השחקן
        public static MediaPlayer mp;// נגן מוזיקה 
        BroadcastBattery broadCastBattery;//מאזין למצב הסוללה
        AudioManager am;//מנהל מוזיקה
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
            try//מנגן את המוזיקה לפי המצב השמור מראש 
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
            RegisterReceiver(broadCastBattery, new IntentFilter(Intent.ActionBatteryChanged));//רושם את המאזין סוללה למערכת
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
            StopService(MainActivity.backgroundMusic);//עוצר את מוזיקת הרקע
            UnregisterReceiver(broadCastBattery);// קוטע את התיאום בין המאזין לסוללה למערכת
        }
        
        /// <summary>
        /// יצירת התפריט והגדרת הופעת אפשרויות בתפריט
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            this.menu = menu;
            MenuInflater.Inflate(Resource.Menu.MainMenu, this.menu);
            this.menu.GetItem(2).SetVisible(true);
            this.menu.GetItem(3).SetVisible(false);
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

        /// <summary>
        /// בחירת קובץ מוזיקה מהמערכת
        /// </summary>
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
        /// <summary>
        /// פונקציה הנקראת כאשר לוחצים על אפשרות בתפריט ופועל בהתאם לאפשרות הנבחרה
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
            return true;
        
         }
        /// <summary>
        /// לחיצה על כפתור הראה שיאים אשר פותח דיאלוג השואל איזה שיא להראות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// לחיצה על אופצית לראות שיאים גלובליים לפי הדיאלוג שנפתח כתוצאה מכפתור להראות שיא
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Globally_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PublicScores));

            StartActivity(intent);
        }
        /// <summary>
        /// לחיצה על אופצית לראות שיאים מקומיים לפי הדיאלוג שנפתח כתוצאה מכפתור להראות שיא
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Privately_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RecordActivity));

            StartActivity(intent);
        }
        /// <summary>
        /// כפתור התחלת משחק
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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