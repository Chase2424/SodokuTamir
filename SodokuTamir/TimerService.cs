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
        public int Callcounter=0, timeCounter=0;
        bool timernotstopped = true;
        int typeofTimer ;//1-call timer, 2- normalTimer

        public override void OnCreate()
        {
            base.OnCreate();
           
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            // Toast.MakeText(this, "service started " + counter, ToastLength.Long).Show();
            Thread t = new Thread(Run);
            typeofTimer = intent.GetIntExtra("type", 0);
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
            Toast.MakeText(this, "Service Stoped" + Callcounter, ToastLength.Long).Show();
            //SodokuActivity.reductionTime = Callcounter + SodokuActivity.reductionTime;
            SodokuActivity.TimeForGame = timeCounter + SodokuActivity.TimeForGame;
            //SodokuActivity.pleaseWork(Callcounter,timeCounter);
            Callcounter = 0;
        }
        
        private void Run()
        {
            while (timernotstopped)
            {
                Thread.Sleep(1000);

                if (typeofTimer == 1)
                    Callcounter++;
               // else if (typeofTimer == 2)
                    timeCounter++;
            }
            StopSelf();
        }
    }
}