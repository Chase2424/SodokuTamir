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
            this.cells = new SudokuCell[9, 9];

            input = "2 9 ? 7 4 3 8 6 1" +
                    "4 7 1 8 6 5 9 ? 7" +
                    "8 7 6 1 9 2 5 4 3" +
                    "3 8 7 4 5 9 2 1 6" +
                    "6 1 2 3 ? 7 4 7 5" +
                    "? 4 9 2 ? 6 7 3 8" +
                    "? ? 3 5 2 4 1 8 9" +
                    "9 2 8 6 7 1 ? 5 4" +
                    "1 5 4 9 3 ? 6 7 2";
            //String [] split = input.split(regex:" ");
            int ButtonHeight = 120, ButtonWidth = 120;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, 0, ButtonHeight, ButtonWidth, this);

                    board.AddView(this.cells[i, j].getButton());

                    //String s =split[i*9+j];
                    //  table[i, j] = new SudokuCell();

                }
            }
            RandomizeBoard();
        }
        public int RandomNumber()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 10);
            return number;
        }
        public void RandomizeBoard()
        {
            int counter = 0;
            int number = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    number = RandomNumber();
                    counter = 0;
                    do
                    {
                        number = RandomNumber();
                        counter++;
                    } while (IsValueTaken(i, j, number)&&counter!=10);
                    
                    this.cells[i, j].setValue(number);
                    this.cells[i, j].getButton().Text = ""+number;
                }
            }
        }
        public Boolean IsValueTaken(int x, int y, int number)
        {
            int squareX, squareY;
            for (int i = 0; i < 9; i++)
            {
                if (this.cells[i, y].getValue() == number)
                {
                    return true;
                }
                if (this.cells[x, i].getValue() == number)
                {
                    return true;
                }
            }
            if (x < 3)
            {
                squareX = 1;
            }
            else if (x > 5)
            {
                squareX = 3;
            }
            else
            {
                squareX = 2;



            }
            if (y < 3)
            {
                squareY = 1;
            }
            else if (y > 5)
            {
                squareY = 3;
            }
            else
            {
                squareY = 2;
            }
            for(int i = squareX; i<squareX+3;i++)
            {
                for(int j= squareY; j<squareY+3;j++)
                {
                    if(this.cells[i,j].getValue() == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}