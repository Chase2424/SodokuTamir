using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Java.IO;
using System;

namespace SodokuTamir
{
    [Activity(Label = "Sodoku")]
    public class SodokuActivity : Activity
    {
        static SodokuActivity _singleTone;
        Button Eraser;
        Button Pen;
            Button Pencil;
        //ISharedPreferences sp;
         static int difficulty; 
        static SudokuCell[,] cells;
         static RelativeLayout L1;
        static int ButtonHeight = 120, ButtonWidth = 120;
        static SudokuCell[,] GuessCells;
        static List<int[,]> Solutions;
        static int result_num = 0;
        ISharedPreferences shared;
        static int guess;
        static bool finishedGenerating = false;
        public static EditText et;
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;
        String input;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _singleTone = this;
            
            SetContentView(Resource.Layout.SodokuLayout);
            // Create your application here
            et = (EditText)FindViewById(Resource.Id.EditText);
            //et.Text="";
            L1 = (RelativeLayout)FindViewById(Resource.Id.Board);
            
            cells = new SudokuCell[9, 9];
            Eraser = (Button)FindViewById(Resource.Id.Eraser);
            Pen = (Button)FindViewById(Resource.Id.Pen);
            Pencil = (Button)FindViewById(Resource.Id.Pencil);
            
            setPermissions();
            L1.RemoveAllViews();
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, 0, ButtonHeight, ButtonWidth, this);
                }
            }
            //Console.WriteLine("Hello World!");
            Solutions = new List<int[,]>();
            int[,] board = new int[9, 9];
            //מקרה 1
            //int[] allow_to_use1 = new int[9] {1,2,3,4,5,6,7,8,9 };
            //generate_array(board, 0, 0, allow_to_use1);

            //מקרה 2 שיש ערך של 2 במקום 0,0
            //int[] allow_to_use1 = new int[8] { 1, 3, 4, 5, 6, 7, 8, 9 };
            int[] allow_to_use1 = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int rndNum = RandomNumber(allow_to_use1.Length);
            board[0, 0] = allow_to_use1[rndNum];
            generate_array(board, 1, 0, allow_to_use1);


            //  print_arr(board,0,0);




        }
        //פונקציה המעבירה את ערכי הלוח למחרוזת אחת
        public static string BoardToString(SudokuCell[,] arr)
        {
            string Str="";
            for(int i =0;i<9;i++)
            {
                for(int j=0;j<9;j++)
                {
                    Str = Str + arr[i, j].getValue();
                }
            }
            return Str;
        }
        public  void SaveBoard()
        {

            
            Context context = this;
            ISharedPreferences sp = PreferenceManager.GetDefaultSharedPreferences(context);

            var editor = sp.Edit();
            
            editor.PutInt("Number", sp.GetInt("Number", 0) + 1);
            String Sname = "Sodoku-" + sp.GetInt("Number", 0) + ".txt";
            editor.Commit();
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
            string game_folder = root + "/saved_sodokus";
            Java.IO.File myDir = new Java.IO.File(game_folder);

            
            Java.IO.File file = new Java.IO.File(myDir, Sname);
            if (file.Exists())
                file.Delete();
            
            myDir.Mkdir(); 
            string filename = Path.Combine(game_folder, Sname);
            FileStream fs = new FileStream(filename, FileMode.Create);

            byte[] bytes = Encoding.UTF8.GetBytes(BoardToString(cells));

            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
            for (int i=0;i<9;i++) {
                for (int j=0;j<9;j++)
                {
                    cells[i, j].getButton().Click += SodokuActivity_Click;
                }
            }
        }

        private void SodokuActivity_Click(object sender, EventArgs e)
        {
            // תנאי לסיום משחק
            // לבדוק אם הכל שונה מ-0
            // לבדוק שהכל כתוב עם עט
            //
            // למנוע מחיקה או שינוי של תאי מקור
            // להוסיף כפתור לאיפוס הלוח
            // להפריד צבעים עט ועפרון
            //so.checkBoard();
            
            if(et!=null && et.Text!=null)
            {
                var button = (Button)sender;
                
            }
        }

        public void NumberEntry()
        {
            var editor = shared.Edit();
            editor.PutString("Board",BoardToString(GuessCells));
            editor.Commit();
        }
        public SudokuCell[,] CreateClues(SudokuCell[,] Original)
        {
            Random rnd = new Random();
            SudokuCell[,] Current = Original;
            int difficulty = 0;
            //int[,] arr = getBoard(Original); 
            difficulty = Intent.GetIntExtra("difficulty", 0);
            string strBoard = BoardToString(Current).ToString();
            char[] board = strBoard.ToArray();
            if (difficulty == 1)
            {
                int i = 0;
                while(i< 20)
                {
                    int number = rnd.Next(0, 81);
                    if (board[number] != '0')
                    {
                        i++;
                        board[number] = '0';
                    }

                }/*
                while (board.Count(f => (f == 0)) < 20)
                {
                    int number = rnd.Next(0, 81);
                    board[number] = '0';
                }*/
            }
            else if (difficulty == 2)
            {
                int i = 0;
                while (i < 30)
                {
                    int number = rnd.Next(0, 81);
                    if (board[number] != '0')
                    {
                        i++;
                        board[number] = '0';
                    }

                }/*
                while (board.Count(f => (f == 0)) < 30)
                {
                    int number = rnd.Next(0, 81);
                    board[number] = '0';
                }*/
            }
            else if (difficulty == 3)
            {
                int i = 0;
                while (i < 40)
                {
                    int number = rnd.Next(0, 81);
                    if (board[number] != '0')
                    {
                        i++;
                        board[number] = '0';
                    }

                }/*
                while (board.Count(f => (f == 0)) < 40)
                {
                    int number = rnd.Next(0, 81);
                    board[number] = '0';
                }*/
            }
            else
            {
                //לא אמור לקרות, רמת משחק לא מוכרת
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
            }
            
            strBoard = new string(board);
            SudokuCell[,]arr2 = new SudokuCell[9, 9];
            arr2 = StringToBoard(strBoard);
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Current[i, j].setValue(arr2[i, j].getValue());
                }
            }
            
            return Current;
        }
        public int[,] getBoard(SudokuCell[,] suduko)
        {
            int[,] arr = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    arr[i, j] = suduko[i, j].getValue();
                }
            }

                    return arr;
        }//הופך את המערך של מחלקה למערך של מספרים שלמים
        public SudokuCell[,] StringToBoard(string str)
        {
            
            SudokuCell[,] arr = new SudokuCell[9,9];
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                  
                     arr[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, (Char)str[i * 9 + j]-'0', ButtonHeight, ButtonWidth, this);
                }
            }
            return arr;
        }
        public static void ShowBoard(SudokuCell[,]board)
        {
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j].getValue() != 0)
                    {
                        L1.AddView(board[i, j].getButton());
                    }
                    else
                    {
                        L1.AddView(board[i, j].getEmptyButton());
                    }
                }
            }
        }
        public static int RandomNumber(int end)
        {
            Random rnd = new Random();
            int number = rnd.Next(0, end);
            return number;
        }
        protected override void OnResume()
        {
            base.OnResume();
            
            //ShowBoard(GuessCells);
        }
        public static bool generate_array(int[,] arr1, int x, int y, int[] allow_to_use1)
        {
           
            //הפונקציה לא עבדה כאשר עבדנו על אוותו מערך בלי לשכפל אותו
            int[,] arr = arr1.Clone() as int[,];

            // אם לא תשמש ב סינון של מה שכבר השתמשת, יקח פי עמה זמן לפונקציה 
            int[] allow_to_use = allow_to_use1.Clone() as int[];
            //שהמספר יבחר מהמערך באופן רנדומלי
            int rndNum;
            rndNum = RandomNumber(allow_to_use.Length);

            //  print_arr(arr, x, y);
            bool next = false;
            for (int i = 0; i < allow_to_use.Length; i++)
            {
                rndNum = RandomNumber(allow_to_use.Length);
                int current_number = allow_to_use[rndNum];
                next = false;
                // אם המספר קיים בשורה, תעבור למספר הבא
                for (int x1 = 0; x1 < x; x1++)
                {
                    if (arr[x1, y] == current_number)
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    //Remove number from Arr
                    allow_to_use = allow_to_use.Where(val => val != current_number).ToArray();
                    continue;
                }
                /*
                for (int y1 = 0; y1 < y; y1++)
                {
                    if (arr[x, y1] == current_number)
                    {
                        next = true;
                        break;
                    }
                }

                if (next)
                {
                    
                    continue;
                }*/


                // אם הגענו לפה, המספר בסדר
                arr[x, y] = current_number;

                if (checkBoard(arr))
                {

                    if (x == 8)
                    {
                        if (y == 8)
                        {
                            //הגענו לסוף הלוח, הצלחה אמיתית GREAT SUCCESS
                            print_arr(arr, x, y);
                            _singleTone.BoardReady(arr);
                            finishedGenerating = true;
                            return true;

                        }
                        x = 0;
                        y++;
                        // אם שורה חדשה, תן לו אפוציה לכל המספרים
                        allow_to_use = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                    else
                    {
                        x++;
                    }
                    if(!finishedGenerating)
                         generate_array(arr, x, y, allow_to_use);
                }

            }


            return true;
        }

        public void BoardReady(int[,]board)
        {
            for (int k = 0; k < 9; k++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[k, j].setValue(board[k, j]);

                }
            }
            _singleTone.SaveBoard();

            //L1.RemoveAllViewsInLayout();

            finishedGenerating = true;
            GuessCells = CreateClues(cells);
            ShowBoard(GuessCells); 
            

            //_singleTone.on_board_ready(arr);
        }
        public static bool checkBoard(int[,] arr)
        {
            // הסתמש במיון דליים בכדי לספור מופעים של כל ספרה בכל ציר
            int[] _bucket = new int[10];
            //[0,0,0,0,0,0,0]
            //    1-9 מטםיע יותר מפעם אחת
            //בדוק את השורות
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (arr[x, y] == 0)
                        continue;
                    if (_bucket[arr[x, y]] > 0)
                        return false;

                    _bucket[arr[x, y]]++;
                }

                _bucket = new int[10];
            }


            //check all y
            for (int x = 0; x < 9; x++)

            {
                for (int y = 0; y < 9; y++)
                {
                    if (arr[x, y] == 0)
                        continue;
                    if (_bucket[arr[x, y]] > 0)
                        return false;

                    _bucket[arr[x, y]]++;
                }

                _bucket = new int[10];
            }


            //Check sub-array

            if (
               // היה קושי לחלק את המערך לבדיקות לפי אזורים בלי לכתוב פונקציית עזר
               check_sub_arr(arr, 0, 0) &&
               check_sub_arr(arr, 3, 0) &&
               check_sub_arr(arr, 6, 0) &&

               check_sub_arr(arr, 0, 3) &&
               check_sub_arr(arr, 3, 3) &&
               check_sub_arr(arr, 6, 3) &&

               check_sub_arr(arr, 0, 6) &&
               check_sub_arr(arr, 3, 6) &&
               check_sub_arr(arr, 6, 6)
               )
                return true;

            else
                return false;
        }

       
        public static bool check_sub_arr(int[,] arr, int start_pos_x, int start_pos_y)
        {

            int[] _bucket = new int[10];


            for (int y = start_pos_y; y < start_pos_y + 3; y++)
            {
                for (int x = start_pos_x; x < start_pos_x + 3; x++)
                {
                    if (arr[x, y] == 0)
                        continue;
                    if (_bucket[arr[x, y]] > 0)
                        return false;

                    _bucket[arr[x, y]]++;
                }


            }


            return true;

        }

        // הדפס את הלוח 
        public static void print_arr(int[,] arr, int pos_x, int pos_y)
        {
            int[,] arr1 = arr.Clone() as int[,];
            result_num++;
            System.Console.WriteLine("*****************************\n");
            System.Console.WriteLine("result_num:" + result_num + " x:" + pos_x + " ,y:" + pos_y);
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    //Console.Write(arr1[x, y] + " ");
                }
                System.Console.WriteLine("\n");
            }
        }
    

       


        public void setPermissions()
        {
            string state = Android.OS.Environment.ExternalStorageState;
            if (Android.OS.Environment.MediaMounted.Equals(state))
            {
                //we can read and write the media
                mExternalStorageAvailable = mExternalStorageWriteable = true;
                Toast.MakeText(this, "we can read and write the media", ToastLength.Long).Show();
            }
            else if (Android.OS.Environment.MediaMountedReadOnly.Equals(state))
            {
                //we can only read the media
                mExternalStorageAvailable = true;
                mExternalStorageWriteable = false;
                Toast.MakeText(this, "we can only read the media", ToastLength.Long).Show();

            }
            else
            {
                //something else is wrong. we can neither rad nor write
                mExternalStorageAvailable = mExternalStorageWriteable = false;
                Toast.MakeText(this, "something else is wrong. we can neither read nor write", ToastLength.Long).Show();
            }
        }
    }
}
