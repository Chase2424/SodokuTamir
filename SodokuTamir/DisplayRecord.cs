using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
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
        String strboard;
        RelativeLayout board;
        bool first_time = true;
        static ArrayList buttons_to_remove = new ArrayList();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecordBoard);
            // Create your application here


            this.board = (RelativeLayout)FindViewById(Resource.Id.Board1);
           
            int place = Intent.GetIntExtra("Position", 0);
            this.cells  = new SudokuCell[9, 9];
            if (Intent.GetStringExtra("Type").Equals("Private"))
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

        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {


                    board.RemoveView(this.cells[i, j].getButton());
                }
            }

        }
        
        public void BuildBoard()
        {
            
            try
            {
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(this.cells[0,0].getWidth(), this.cells[0, 0].getLength());
                int x = 0, y = 0;
                
                for (int i = 0; i < 9; i++)
                {

                    y = i * ButtonWidth;
                    for (int j = 0; j < 9; j++)
                    {
                        x = j * ButtonHeight;
                        if (this.cells[i, j].getValue() != 0)
                        {
                            this.board.RemoveView(this.cells[i, j].getButton());
                            this.board.AddView(this.cells[i, j].getButton());

                        }
                        else
                        {
                            this.board.RemoveView(this.cells[i, j].getButton());
                            this.board.AddView(this.cells[i, j].getEmptyButton());
                        }
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