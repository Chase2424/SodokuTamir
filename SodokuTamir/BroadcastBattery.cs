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

namespace SodokuTamir
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class BroadcastBattery : BroadcastReceiver
    {
        bool once= true;
        Context context;
        public BroadcastBattery()
        {
            
        }
        public BroadcastBattery(Context context)
        {
            this.context = context;
        }
        /// <summary>
        /// פונקציה מאזינה לסוללה ומתריעה שהסוללה יורדת מ20 אחוז
        /// המחלקה הראשית מפעילה אותה 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            int battery = intent.GetIntExtra("level", 0);
            if(battery<20 &&once)
            {
                once = false;
                Toast.MakeText(context, "Your battery precentile is very low, beware", ToastLength.Long).Show();
            }
        }
    }
}