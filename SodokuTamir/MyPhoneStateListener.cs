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
    public class MyPhoneStateListener: PhoneStateListener
    {
        /// <summary>
        /// מחלקה העוסקת בשינוי של מצב תקשורת טלפונית.
        /// </summary>

        Context context;
        //public static int countcheck;
        public MyPhoneStateListener(Context context)
        {
            this.context = context;
        }
        /// <summary>
        /// הפונקציה מקבלת מצב שיחה ואת טלפון המספר הנכנס 
        /// מפעיל הוראות בהתאם לסוג שינוי תקשורת אשר הגיע לפונקציה
        /// </summary>
        /// <param name="CS"></param>
        /// <param name="incomingNumber"></param>
        public override void OnCallStateChanged(CallState CS, string incomingNumber)
        {
            base.OnCallStateChanged(CS, incomingNumber);

            bool isThereACall = false;

            switch (CS)
            {
                case CallState.Ringing:

                    break;
                case CallState.Offhook:
                    Toast.MakeText(Application.Context, "Mid conversation", ToastLength.Short).Show();
                    SodokuActivity.timerclass.SomeoneIsCalling();


                    break;
                case CallState.Idle:
                    Toast.MakeText(Application.Context, "Stopped interaction", ToastLength.Short).Show();
                    SodokuActivity.timerclass.CallFinish();
                    break;
            }

        }
    }
}