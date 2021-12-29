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
    [Activity(Label = "DisplayRecord")]
    public class DisplayRecord : Activity
    {
        int[,] SavedArray = new int[9, 9];
        int ButtonHeight = 120, ButtonWidth = 120;
        SudokuCell[,] cells;
        RelativeLayout board;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecordBoard);
            // Create your application here
            this.board = (RelativeLayout)FindViewById(Resource.Id.Board1);
            BuildBoard();
        }
        public void DefineCurrentArray()
        {
            int j = Intent.GetIntExtra("Position", 0);

        }
        public SudokuCell[,] StringToBoard(string str)
        {
            SudokuCell[,] arr = new SudokuCell[9,9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    arr[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, str[i * j], ButtonHeight, ButtonWidth, this);
                }
            }
            return arr;
        }
        public void BuildBoard()
        {
            try
            {


                
                for (int i = 0; i < 9; i++)
                {

                    for (int j = 0; j < 9; j++)
                    {
                        this.cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, SavedArray[i,j], ButtonHeight, ButtonWidth, this);

                        board.AddView(this.cells[i, j].getButton());
                        //
                        //String s =split[i*9+j];
                        //  table[i, j] = new SudokuCell();

                    }
                }
            }
            catch
            {
                this.cells = new SudokuCell[9, 9];

                for (int i = 0; i < 9; i++)
                {

                    for (int j = 0; j < 9; j++)
                    {
                        this.cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, 0, ButtonHeight, ButtonWidth, this);

                        board.AddView(this.cells[i, j].getButton());
                    }
                }
             }
        }
    }
}