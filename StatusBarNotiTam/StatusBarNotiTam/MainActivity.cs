using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace StatusBarNotiTam
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button Start, Stop;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Start = (Button)FindViewById(Resource.Id.Start);
            Stop = (Button)FindViewById(Resource.Id.Stop);
            Start.Click += Start_Click;
            Stop.Click += Stop_Click;
        }

        private void Stop_Click(object sender, System.EventArgs e)
        {
           
        }

        private void Start_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(CheckActivity));
            i.PutExtra("key", "new message");
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, i, 0);
            Notification.Builder notificationBuilder = PendingIntent.GetActivity new Notification.Builder(this).SetSmallIcon(Resource.Drawable.baseline_done_black_48).SetContentTitle("title").SetContentText("text text");
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationBuilder.SetContentIntent(pendingIntent);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID,
                "NOTIFICATION_CHANNEL_NAME", NotificationImportance.High);
                notificationBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}