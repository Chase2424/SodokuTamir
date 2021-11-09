using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodokuTamir
{
    public class Player
    {
        private string name;//Name of the player
        private int Time;//The time it took for the player to solve the sudoku
        private string Date;//The Date the game took place in


        public Player(string name, int Time, String Date)
        {
            this.name = name;
            this.Time = Time;
            this.Date = Date;

        }
        public string getName()
        {
            return this.name;
        }
        public int getTime()
        {
            return this.Time;
        }
        public String getDate()
        {
            return this.Date;
        }
    }
}