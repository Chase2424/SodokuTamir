﻿using Android.App;
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
        public override void OnCallStateChanged(CallState CS, string incomingNumber)
        {
            base.OnCallStateChanged(CS, incomingNumber);
            bool isThereACall=false;
            switch (CS)
            {
                case CallState.Ringing:
                    Toast.MakeText(Application.Context, "SomeoneIsCalling", ToastLength.Short).Show();
                    break;
                case CallState.Offhook: 
                    Toast.MakeText(Application.Context, "Mid conversation", ToastLength.Short).Show();
                    break;
                case CallState.Idle:
                    Toast.MakeText(Application.Context, "Stopped interaction", ToastLength.Short).Show();
                    break;
            }


        }
    }
}