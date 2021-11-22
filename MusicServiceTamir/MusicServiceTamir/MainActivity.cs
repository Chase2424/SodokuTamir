using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Media;
using Android.Content;


namespace MusicServiceTamir
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        SeekBar s1;
        AudioManager am;
        Button start, stop,pause,Piano;
        MediaPlayer mp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            start = (Button)FindViewById(Resource.Id.Start);
            stop = (Button)FindViewById(Resource.Id.Stop);
            s1 = (SeekBar)FindViewById(Resource.Id.seekBar1);
            pause = (Button)FindViewById(Resource.Id.Pause);
            mp = MediaPlayer.Create(this, Resource.Raw.CRHCP);
            Piano = (Button)FindViewById(Resource.Id.Piano);
            start.Click += Start_Click;
            stop.Click += Stop_Click;
            s1.ProgressChanged += S1_ProgressChanged;
            am = (AudioManager)GetSystemService(Context.AudioService);
            int max = am.GetStreamMaxVolume(Stream.Music);
            s1.Max = max;
            am.SetStreamVolume(Stream.Music, max / 2, 0);
            pause.Click += Pause_Click;
            Piano.Click += Piano_Click;
        }

        private void Piano_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(pianoActivity));
            StartActivity(i);

        }

        private void Pause_Click(object sender, System.EventArgs e)
        {
            mp.Pause();
        }

        private void S1_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            am.SetStreamVolume(Stream.Music, e.Progress, VolumeNotificationFlags.PlaySound);
        }

        private void Stop_Click(object sender, System.EventArgs e)
        {
            mp.Stop();
        }

        private void Start_Click(object sender, System.EventArgs e)
        {
            mp.Start();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}