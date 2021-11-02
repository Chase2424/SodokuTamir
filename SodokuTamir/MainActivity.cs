﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;

namespace SodokuTamir
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        PlayerAdapter adapter;
        ListView lv;
        public static List<Player> list = new List<Player>();
        Button btnStart, btnRecord;
        RadioButton Easy, Medium, Hard;
        EditText PlayerName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            list.Add(new Player("Guy1", 200,"4th july 2020"));
            list.Add(new Player("Guy2", 20020, "4th july 2020"));
            list.Add(new Player("Girl", 100, "4th july 2020"));
            btnStart = (Button)FindViewById(Resource.Id.Start);
            btnRecord = (Button)FindViewById(Resource.Id.Record);
            Easy = (RadioButton)FindViewById(Resource.Id.Easy);
            Medium = (RadioButton)FindViewById(Resource.Id.Medium);
            Hard = (RadioButton)FindViewById(Resource.Id.Hard);
            PlayerName = (EditText)FindViewById(Resource.Id.PlayerName);
            btnStart.Click += BtnStart_Click;
            btnRecord.Click += BtnRecord_Click;
        }

        private void BtnRecord_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RecordActivity));

            StartActivity(intent);
        }

        private void BtnStart_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SodokuActivity));

            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}