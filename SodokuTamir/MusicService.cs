using Android.App;
using Android.Content;
using Android.Media;
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
    [Service]
    public class MusicService: Service
    {
        MediaPlayer mp;
        AudioManager am;
        public override void OnCreate()
        {
            base.OnCreate();
            mp = MediaPlayer.Create(this, Resource.Raw.Song);
            am = (AudioManager)GetSystemService(Context.AudioService);
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            // Toast.MakeText(this, "service started " + counter, ToastLength.Long).Show();
            PlayMusic();

            return base.OnStartCommand(intent, flags, startId);
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            mp.Pause();
        }
        private void PlayMusic()
        {
            mp.Start();
        }
    }
}