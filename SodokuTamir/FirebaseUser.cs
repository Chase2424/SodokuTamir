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
using System.Threading.Tasks;

namespace SodokuTamir
{
    public static class FirebaseUser
    {
        public static FirebaseClient firebase = new FirebaseClient("https://sodokutamir-default-rtdb.firebaseio.com/");
        public static string tableName = "sodokutamir";
        public static async Task<List<Player>> GetAll()
        {
            return (await firebase
                .Child(tableName)
                .OnceAsync<Player>()).Select(item => new Player
                {
                    name = item.Object.name,
                    Time = item.Object.Time,
                    Date = item.Object.Date,
                    SodokuBoard = item.Object.SodokuBoard

                }).ToList();
        }
        public static async Task AddScore(Player player)
        {

            await firebase
              .Child(tableName)
              .PostAsync(player);
        }

    }
}