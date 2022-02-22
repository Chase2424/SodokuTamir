using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "android.intent.action.PHONE_STATE" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class MyPhoneReceiver : BroadcastReceiver
    {
       public MyPhoneReceiver()
        {

        }
         

        public override void OnReceive(Context context, Intent intent)
        {
            MyPhoneStateListener phoneStateListener = new MyPhoneStateListener(context);
            TelephonyManager telephonyManager = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
            telephonyManager.Listen(phoneStateListener, PhoneStateListenerFlags.CallState);
        }
    }
}