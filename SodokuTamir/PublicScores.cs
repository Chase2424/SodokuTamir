using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    [Activity(Label = "PublicScores")]
    public class PublicScores : AppCompatActivity
    {
        /// <summary>
        /// מחלקה בה מוצגים השיאים שנשמרו באופן גלובלי
        /// </summary>
        ListView lv;//רשימה של קוד לתצוגה
        PlayerAdapter adapter;//מתאם שחקן לרשימת קוד לתצוגה
        Android.Views.IMenu menu;//תפריט

        public static List<Player> listPublic = new List<Player>();//רשימת שחקנים
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PublicScores);
            // Create your application here
            this.lv = (ListView)FindViewById(Resource.Id.publicLview);
            listPublic.Clear();
            ReadFromFireBase();
            lv.ItemClick += Lv_ItemClick;
        }
        /// <summary>
        /// יצירת התפריט והגדרת הופעת אפשרויות בתפריט
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            this.menu = menu;
            MenuInflater.Inflate(Resource.Menu.MainMenu, this.menu);
            this.menu.GetItem(2).SetVisible(false);
            this.menu.GetItem(3).SetVisible(false);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            else if (MainActivity.SP.GetBoolean("IsMusicOn", false) == true)
            {
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);

            }
            return true;
        }
        /// <summary>
        /// פונקציה הנקראת כאשר לוחצים על אפשרות בתפריט ופועל בהתאם לאפשרות הנבחרה
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", true);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);
                StartService(MainActivity.backgroundMusic);

            }
            else
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", false);
                StopService(MainActivity.backgroundMusic);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            return true;

        }
        /// <summary>
        /// פונקציה הנקראת כאשר נלחץ אובייקט שיא במסך 
        /// הפונקציה מעבירה לתצוגת הסודוקו הפתור במחלקה אחרת
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(DisplayRecord));
            int pos = e.Position;
            i.PutExtra("Position", pos);
            i.PutExtra("Type","Global");
            StartActivity(i);
        }
        /// <summary>
        /// פונקציה הקוראת את המידע מהענן וממירה אותו לרשימת משתמשים
        /// לאחר מכן היא מתאמת בין ההצגה על המסך לרשימת המשתמשים
        /// </summary>
        public async void ReadFromFireBase()
        {
            try
            {
                List<Player> decoy = new List<Player>();
                decoy = await FirebaseUser.GetAll();
                for (int i = 0; i < decoy.Count(); i++)
                {

                    listPublic.Add(decoy[i]);
                }

                this.adapter = new PlayerAdapter(this, listPublic, "PublicScores");
                this.lv.Adapter = adapter;
            }
            catch
            {
                Toast.MakeText(this, "No Records Founded", ToastLength.Long).Show();
            }
        }
    }
}