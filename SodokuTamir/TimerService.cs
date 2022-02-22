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
using System.Threading;

namespace SodokuTamir
{
    [Service]
    public class TimerService:Service
    {
        int counter=0;
        bool timernotstopped = true;
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            
            Toast.MakeText(this, "service started " + counter, ToastLength.Long).Show();
            Thread t = new Thread(Run);
            t.Start();
            return base.OnStartCommand(intent, flags, startId);
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            timernotstopped = false;
            Toast.MakeText(this, "Service Stoped"+counter, ToastLength.Long).Show(); 
            counter = 0;
        }
        private void Run()
        {
            while (timernotstopped)
            {
                Thread.Sleep(1000);
               
               
                counter++;
            }
            StopSelf();
        }
    }
}