﻿using Android.App;
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
        private string Time;//The time it took for the player to solve the sudoku
        private string Date;//The Date the game took place in
        private string SodokuBoard;//The PlayersSodoku

        public Player(string name, string Time, string Date,string SodokuBoard)
        {
            this.name = name;
            this.Time = Time;
            this.Date = Date;
            this.SodokuBoard = SodokuBoard;
        }
        public string getName()
        {
            return this.name;
        }
        public string getTime()
        {
            return this.Time;
        }
        public string getDate()
        {
            return this.Date;
        }
        public string getBoard()
        {
            return this.SodokuBoard;
        }
    }
}