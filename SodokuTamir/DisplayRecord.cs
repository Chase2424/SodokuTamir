using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
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
            if(board!=null)
                this.board.RemoveAllViews();
            this.board = (RelativeLayout)FindViewById(Resource.Id.Board1);
            int place = Intent.GetIntExtra("Position", 0);
            if (Intent.GetStringExtra("Type").Equals("private"))
            {
                this.cells = MainActivity.list[place].getBoard();
            }
            else
            {
                PublicScores.listPublic[place].StringToBoard(PublicScores.listPublic[place].GetStrBoard(),this);
                this.cells = PublicScores.listPublic[place].getBoard();
            }
            BuildBoard();
        }
       
       
        
       
        public void BuildBoard()
        {
            
            try
            {



                for (int i = 0; i < 9; i++)
                {

                    for (int j = 0; j < 9; j++)
                    {
                        

                        board.AddView(this.cells[i, j].getButton());
                        //
                        //String s =split[i*9+j];
                        //  table[i, j] = new SudokuCell();

                    }
                }
            }
            catch(Exception e)
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