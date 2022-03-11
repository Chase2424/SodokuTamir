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
    public class TimerClass
    {
        bool InACall = false;
        public int timeCounter = 0;
        bool timernotstopped = true;
        public TimerClass()
        {

        }
        public void SomeoneIsCalling()
        {
            InACall = true;

        }
        public void CallFinish()
        {
            InACall = false;
        }
        public void setStopped()
        {
            timernotstopped = false;
        }
        public int getTime()
        {
            return timeCounter;
        }
        public void Run()
        {
            while (timernotstopped)
            {
                if (InACall != true)
                {
                    Thread.Sleep(1000);
                    timeCounter++;
                }
            }

        }
    }
}