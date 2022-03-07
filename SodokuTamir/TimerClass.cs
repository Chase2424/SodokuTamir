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
        public int timeCounter = 0;
        bool timernotstopped = true;
        public TimerClass()
        {
            
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
                Thread.Sleep(1000);
                timeCounter++;
            }
            
        }
    }
}