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
        /// <summary>
        /// מחלקת מוזיקה אשר רצה ברקע האפליקציה
        /// </summary>
        MediaPlayer mp;//נגן מוזיקה
        AudioManager am;//מנהל מוזיקה
        public override void OnCreate()
        {
            base.OnCreate();
            mp = MainActivity.mp;
            am = (AudioManager)GetSystemService(Context.AudioService);
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            PlayMusic();
            return base.OnStartCommand(intent, flags, startId);
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            mp.Looping = false;
            mp.Pause();
        }
        private void PlayMusic()
        {
            mp.Start();
            mp.Looping = true;
        }
    }
}