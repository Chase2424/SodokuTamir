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
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecordBoard);
            // Create your application here
            this.board = (RelativeLayout)FindViewById(Resource.Id.Board1);
            
            BuildBoard();
        }
        public void getSpecificFile(int num)
        {
            //Java.IO.File path = Environment.GetExternalFilesDir(System.Environment.DirectoryPictures);
            //Java.IO.File myDir = new Java.IO.File(path, "/saved_images");
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();

            Java.IO.File myDir = new Java.IO.File(root + "/saved_sodokus");
            

            var fileName = @"suduku-"+num;
            using FileStream fs = File.OpenRead(fileName);
            byte[] buf = new byte[1024];
            int c;
            string sodoku_text="";
            while ((c = fs.Read(buf, 0, buf.Length)) > 0)
            {
                sodoku_text+=(Encoding.UTF8.GetString(buf, 0, c));
            }

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
                    arr[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, str[i * 9+j], ButtonHeight, ButtonWidth, this);
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
                        this.cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, SavedArray[i, j], ButtonHeight, ButtonWidth, this);

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
        public void setPermissitios()
        {
            string state = Android.OS.Environment.ExternalStorageState;
            if (Android.OS.Environment.MediaMounted.Equals(state))
            {
                //We can read and write the media
                mExternalStorageAvailable = mExternalStorageWriteable = true;
                Toast.MakeText(this, "We can read and write the media", ToastLength.Long).Show();
            }
            else if (Android.OS.Environment.MediaMountedReadOnly.Equals(state))
            {
                //We can only read the media
                mExternalStorageAvailable = true;
                mExternalStorageWriteable = false;
                Toast.MakeText(this, "We can only read the media", ToastLength.Long).Show();
            }
            else
            {
                //Something else is wrong. we can neither read nor write
                mExternalStorageAvailable = mExternalStorageWriteable = false;
                Toast.MakeText(this, "Something else is wrong. we can neither read nor write", ToastLength.Long).Show();
            }
        }
    }
}