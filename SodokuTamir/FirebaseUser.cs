using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace SodokuTamir
{
    public static class FirebaseUser
    {
        /// <summary>
        /// במחלקה זו מוגדר ההתעסקות עם הFireBase.
        /// פונים למחלקה זו כאשר מוסיפים "שיא גלובלי" או כאשר אנו קוראים את השיאים
        /// </summary>
        public static FirebaseClient firebase = new FirebaseClient("https://sodokutamir-default-rtdb.firebaseio.com/");//יצירת משתמש וקישור לנתונים שלי בענן
        public static string tableName = "sodokutamir";//שם הטבלה ששמורים דרכה
       /// <summary>
       /// פונקציה זו לוקחת את כל המשתמשים אשר נשמרו במאגר המידע בענן וממירה אותם לרשימת שחקנים
       /// </summary>
       /// <returns>הפונקציה מחזיקה רשימת שחקנים</returns>
        public static async Task<List<Player>> GetAll()
        {
            return (await firebase
               .Child(tableName)
               .OnceAsync<JObject>()).Select(item => new Player
               {
                   Name = (string)item.Object.GetValue("Name"),
                   Time = (string)item.Object.GetValue("Time"),
                   Date = (string)item.Object.GetValue("Date"),
                   StrBoard = (string)item.Object.GetValue("StrBoard")
                   

               }).ToList();
        }
        /// <summary>
        /// הפונקציה מקבלת אובייקט שחקן ומוסיפה אותו למאגר מידע
        /// </summary>
        /// <param name="player"></param>
        /// <returns>הפונקציה לא מחזירה כלום</returns>
        public static async Task AddScore(Player player)
        {

            await firebase
              .Child(tableName)
              .PostAsync(player);
        }

    }
}