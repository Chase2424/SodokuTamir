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
        public string name;//Name of the player
        public string Time;//The time it took for the player to solve the sudoku
        public string Date;//The Date the game took place in
        public SudokuCell[,] SodokuBoard;//The PlayersSodoku

        public Player()
        {

        }
        public Player(string name, string Time, string Date, SudokuCell[,] SodokuBoard)
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
        public SudokuCell[,] getBoard()
        {
            return this.SodokuBoard;
        }
    }
}