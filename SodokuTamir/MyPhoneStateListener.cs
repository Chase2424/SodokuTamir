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
    class MyPhoneStateListener: PhoneStateListener
    {
        public override void OnCallStateChanged(CallState CS, string incomingNumber)
        {
            base.OnCallStateChanged(CS, incomingNumber);
            bool isThereACall=false;
            switch (CS)
            {
                case CallState.Ringing://כשמתקשרים אליך
                    Toast.MakeText(Application.Context, "SomeoneIsCalling", ToastLength.Short).Show();
                    break;
                case CallState.Offhook://כאשר ענית לשיחה 
                    Toast.MakeText(Application.Context, "Mid conversation", ToastLength.Short).Show();
                    break;
                case CallState.Idle://כששיחה הסתיימה או שאין שיחה בכלל
                    Toast.MakeText(Application.Context, "Stopped interaction", ToastLength.Short).Show();
                    break;
            }


        }
    }
}