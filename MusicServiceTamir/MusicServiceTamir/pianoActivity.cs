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
using Android.Media;
using Android.Content;

namespace MusicServiceTamir
{
    [Activity(Label = "pianoActivity")]
    public class pianoActivity : Activity
    {
        Button Do, re, mi, fa, so, la, ci;
        AudioManager am;
        MediaPlayer mp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.pianoLayout);

            // Create your application here
            Do = (Button)FindViewById(Resource.Id.Do);
            re = (Button)FindViewById(Resource.Id.Re);
            mi = (Button)FindViewById(Resource.Id.Mi);
            fa = (Button)FindViewById(Resource.Id.Fa);
            so = (Button)FindViewById(Resource.Id.Sol);
            la = (Button)FindViewById(Resource.Id.La);
            ci = (Button)FindViewById(Resource.Id.Ci);
            am = (AudioManager)GetSystemService(Context.AudioService);
            int max = am.GetStreamMaxVolume(Stream.Music);

            am.SetStreamVolume(Stream.Music, max / 2, 0);
            Do.Click += Do_Click;
            re.Click += Re_Click;
            mi.Click += Mi_Click;
            fa.Click += Fa_Click;
            so.Click += So_Click;
            la.Click += La_Click;
            ci.Click += Ci_Click;

        }

        private void Ci_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.ci);
            mp.Start();
        }

        private void La_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.la);
            mp.Start();
        }

        private void So_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.so);
            mp.Start();
        }

        private void Fa_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.fa);
            mp.Start();
        }

        private void Mi_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.mi);
            mp.Start();
        }

        private void Re_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.re);
            mp.Start();
        }

        private void Do_Click(object sender, EventArgs e)
        {
            mp = MediaPlayer.Create(this, Resource.Raw.do1);
            mp.Start();
        }
    }
}