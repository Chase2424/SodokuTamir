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
    [Activity(Label = "Sodoku")]
    public class SodokuActivity : Activity
    {
        SudokuCell[,] cells;
        RelativeLayout board;
        String input;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SodokuLayout);
            // Create your application here
            this.board = (RelativeLayout)FindViewById(Resource.Id.Board);
            this.cells = new SudokuCell[9,9];
            
            input = "2 9 ? 7 4 3 8 6 1"+
                    "4 7 1 8 6 5 9 ? 7"+
                    "8 7 6 1 9 2 5 4 3"+
                    "3 8 7 4 5 9 2 1 6"+
                    "6 1 2 3 ? 7 4 7 5"+
                    "? 4 9 2 ? 6 7 3 8"+
                    "? ? 3 5 2 4 1 8 9"+
                    "9 2 8 6 7 1 ? 5 4"+
                    "1 5 4 9 3 ? 6 7 2";
            //String [] split = input.split(regex:" ");
            int ButtonHeight=120,ButtonWidth = 120;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, i, ButtonHeight, ButtonWidth, this);
                    board.AddView(this.cells[i, j].getButton());
                    
                    //String s =split[i*9+j];
                    //  table[i, j] = new SudokuCell();

                }
            }
        }
    }
}