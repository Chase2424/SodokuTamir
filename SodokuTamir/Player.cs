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
        /// <summary>
        /// מחלקת משתמש
        /// </summary>
        public string Name;//שם השחקן
        public string Time;//הזמן שלקח לשחקן לפתור את הסודוקו
        public string Date;//תאריך המשחק
        public SudokuCell[,] SodokuBoard;//הסודוקו אשר פתר
        
        public string StrBoard= "";//הסודוקו אשר פתר בצורת מחרוזת


        //יש לי שני סוגים של פעולה בונה  
        public Player()
        {

        }
        /// <summary>
        ///  פעולה בונה של משתמש גלובלי כך כשקוראים את המידע מהפיירבייס או מכניסים אותו אפשר להכניס את הלוח בתור מחרוזת
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Time"></param>
        /// <param name="Date"></param>
        /// <param name="board"></param>
        public Player(string Name, string Time, string Date, string board)
        {
            this.Name = Name;
            this.Time = Time;
            this.Date = Date;
            this.StrBoard = board;



        }
        /// <summary>
        /// פעולה בונה של משתמש אשר אפשר להכניס לוח סודוקו 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Time"></param>
        /// <param name="Date"></param>
        /// <param name="SodokuBoard"></param>
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