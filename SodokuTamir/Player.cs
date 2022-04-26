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
        public string Name;//Name of the player
        public string Time;//The time it took for the player to solve the sudoku
        public string Date;//The Date the game took place in
        public SudokuCell[,] SodokuBoard;//The PlayersSodoku
        
        public string StrBoard="";//Board as string, I use it in the public players


        public Player()
        {

        }
        public Player(string Name, string Time, string Date, string board)
        {
            this.Name = Name;
            this.Time = Time;
            this.Date = Date;
            this.StrBoard = board;



        }
        public Player(string Name, string Time, string Date, SudokuCell[,] SodokuBoard)
        {
            this.Name = Name;
            this.Time = Time;
            this.Date = Date;
            this.SodokuBoard = SodokuBoard;
        }
        public string GetStrBoard()
        {
            return this.StrBoard;
        }
        public void StringToBoard(string str, Context context)
        {

            SudokuCell[,] arr = new SudokuCell[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    arr[i, j] = new SudokuCell(j * 120, i * 120, (Char)str[i * 9 + j] - '0', 120, 120, context);
                }
            }
            this.SodokuBoard = arr;
        }
        
        public string BoardToString(SudokuCell[,] arr)
        {
            string Str = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Str = Str + arr[i, j].getValue();
                }
            }
            return Str;
        }
        
        public string getName()
        {
            return this.Name;
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