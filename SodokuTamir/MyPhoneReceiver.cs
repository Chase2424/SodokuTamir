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
        /// <summary>
        /// מחלקה העוסקת בתקשורת טלפונית, מהותה להאזין ולזהות תקשורת טלפונית של ,תקשורת בין טלפונים
        /// </summary>
       public MyPhoneReceiver()
        {

        }
         
        /// <summary>
        /// פונקציה המופעלת כאשר יש שינוי במצב התקשורת טלפונית
        /// הפונקציה מעבירה מסר למחלקה MyPhoneStateListener כי התקיים שינוי במצב התקשורת
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            MyPhoneStateListener phoneStateListener = new MyPhoneStateListener(context);
            TelephonyManager telephonyManager = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
            telephonyManager.Listen(phoneStateListener, PhoneStateListenerFlags.CallState);
            
        }
    }
}