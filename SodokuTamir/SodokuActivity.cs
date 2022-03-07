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
using Android.Icu.Text;
using System.ServiceProcess;
using System.Threading;
//TODO
// merge cells and getcells
// add numbers keyboard
// fix application close
// fix back button
// add clean file button
namespace SodokuTamir
{
    [Activity(Label = "Sodoku")]
    public class SodokuActivity : Activity
    {
        public static int reductionTime=0;
        static SodokuActivity _singleTone;
        Button Eraser;
        int toolType= 1;//0-eraser,1-pencil,2-pen
        Button Pen;
        Intent backgroundMusic;
        static int lives=3;
        Button Pencil;
        //ISharedPreferences sp;
        static int difficulty;
        static SudokuCell[,] cells;
        
        static DateTime startTime;
        static TimeSpan duration;
        TimerClass timerclass= new TimerClass();
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
        static bool gameStatus = false;
        String input;
        MyPhoneReceiver PhoneReceiver = new MyPhoneReceiver();
        LinearLayout l1;
        protected override void OnResume()
        {
            
            base.OnResume();
            //RegisterReceiver(PhoneReceiver, new IntentFilter(Intent.ActionCall));
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            
            SetContentView(Resource.Layout.SodokuLayout);
            // Create your application here
            finishedGenerating = false;
            /*
           if(gameStatus)
            {

                L1.RemoveAllViews();
                gameStatus = false;
            }*/

            ThreadStart ts = new ThreadStart(timerclass.Run);
            Thread thread = new Thread(ts);
            thread.Start();
            _singleTone = this;
            et = (EditText)FindViewById(Resource.Id.EditText);
            //et.Text="";
            L1 = null;
            L1 = (RelativeLayout)FindViewById(Resource.Id.Board);

            startTime = Convert.ToDateTime(DateTime.Now.ToString());
            
            cells = new SudokuCell[9, 9];
            Eraser = (Button)FindViewById(Resource.Id.Eraser);
            Pen = (Button)FindViewById(Resource.Id.Pen);
            Pencil = (Button)FindViewById(Resource.Id.Pencil);
            Eraser.Click += Eraser_Click;
            Pen.Click += Pen_Click;
            Pencil.Click += Pencil_Click;
            setPermissions();
            
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight,0, ButtonHeight, ButtonWidth, this);
                   
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

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MusicMenu, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Resource.Id.action_startMusic)
            {
               if (item.IsVisible)
                {
                    backgroundMusic = new Intent(this, typeof(MusicService));
                   
                    StartService(backgroundMusic);
                }
            }
            else if (item.ItemId == Resource.Id.action_stopMusic)
            {
                if (item.IsVisible)
                {
                    StopService(backgroundMusic);
                   
                }
            }
            return true;
        }
        private void Pencil_Click(object sender, EventArgs e)
        {
            this.toolType = 1;
        }

        private void Pen_Click(object sender, EventArgs e)
        {
            this.toolType = 2;
        }

        private void Eraser_Click(object sender, EventArgs e)
        {
            this.toolType = 0;
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
        public void SaveBoard()
        {
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
            string game_folder = root + "/saved_sodokus";
            int fCount = Directory.GetFiles(game_folder, "*", SearchOption.TopDirectoryOnly).Length;
            if ((int)fCount > 50)
            {
                Toast.MakeText(this, " Too many files, please clean some sodoku files  ", ToastLength.Short).Show();
                return;
            }
            Context context = this;
            ISharedPreferences sp = PreferenceManager.GetDefaultSharedPreferences(context);
            string PlayerName = Intent.GetStringExtra("PlayerName");
            var editor = sp.Edit();
            
            editor.PutInt("Number", sp.GetInt("Number", 0) + 1);
            String Sname = "Sodoku-" + sp.GetInt("Number", 0) + ".txt";
            editor.Commit();
            
            Java.IO.File myDir = new Java.IO.File(game_folder);
            myDir.Mkdir(); 
            
            Java.IO.File file = new Java.IO.File(myDir, Sname);
            if (file.Exists())
                file.Delete();
            
            
            string filename = Path.Combine(game_folder, Sname);
            FileStream fs = new FileStream(filename, FileMode.Create);

            byte[] bytes = Encoding.UTF8.GetBytes(PlayerName+","+duration + ","+startTime+"," + BoardToString(cells));

            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();/*
            for (int i=0;i<9;i++) {
                for (int j=0;j<9;j++)
                {
                    cells[i, j].getButton().Click += SodokuActivity_Click;
                }
            }*/
        }
        /*
        public void onBackPressed()
        {
            Toast.MakeText(this, " Press Back again to Exit ", ToastLength.Short).Show();
            return;
        }*/
        private void SodokuActivity_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int btnTag = (int)button.Tag;
            //התיוג של הכפתור משמש לדעת האם המשבצת נרשמה על ידי המשתמש או על ידינו
            if (et!=null && et.Text!=null && btnTag!=2 )
            {
                //בדיקת סוג הכלי אשר הוא מנסה לשנות איתו
                if (toolType == 0)
                {
                    button.Hint = "";
                }
                else if(toolType == 1)
                {
                    button.Hint = et.Text;
                }
                else
                {
                    try
                    {
                        int input = Int32.Parse(et.Text);
                        if(input >0 && input<10)
                        {
                            button.Text = et.Text;
                            int[,] arr = new int[9, 9];
                            arr = getBoard(GuessCells);
                            if (!checkBoard(arr))
                            {
                                button.Text = "";
                                lives = lives - 1;
                                if (lives == 0)
                                {
                                    L1.RemoveAllViewsInLayout();
                                    Toast.MakeText(this, "GAME-OVER", ToastLength.Long).Show();
                                    
                                    Intent i = new Intent(this, typeof(MainActivity));
                                    StartActivity(i);
                                }
                                else
                                {
                                    Toast.MakeText(this, "You have:"+lives+"lives left", ToastLength.Long).Show();
                                }
                                
                            }
                            else
                            {
                               if( !BoardToString(GuessCells).Contains('0'))
                                {
                                    Toast.MakeText(this, "Congratulations you have won", ToastLength.Long).Show();
                                    DateTime endtime = Convert.ToDateTime(DateTime.Now.ToString());
                                    timerclass.setStopped();
                                    duration = TimeSpan.FromSeconds(timerclass.getTime());
                                     
                                    Dialog d = new Dialog(this);
                                    d.SetContentView(Resource.Layout.SaveOutside);
                                    Button privately = (Button)d.FindViewById(Resource.Id.Private);
                                    Button publicly = (Button)d.FindViewById(Resource.Id.Public);
                                    d.Show();
                                    privately.Click += Privately_Click;
                                    publicly.Click += Publicly_Click;
                                    

                                }
                            }

                        }
                    }
                    catch
                    {
                        Toast.MakeText(this, "please enter numbers only", ToastLength.Long).Show() ;
                        

                    }
                }
            }
        }

        private async void Publicly_Click(object sender, EventArgs e)
        {
            //doFireWallStuff
            string PlayerName = Intent.GetStringExtra("PlayerName");
            Player p = new Player(PlayerName, ""+duration, ""+startTime, BoardToString(cells));
            await FirebaseUser.AddScore(p);
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        private void  Privately_Click(object sender, EventArgs e)
        {
            SaveBoard();
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
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

            difficulty = 0;
            //int[,] arr = getBoard(Original); 
            difficulty = Intent.GetIntExtra("difficulty", 0);
            string strBoard = BoardToString(Current).ToString();
            char[] board = strBoard.ToArray();
            if (difficulty == 1)
            {
                int i = 0;
                while(i< 3)
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
                while (i < 20)
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
                while (i < 30)
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
        public static void ShowBoard()
        {
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(GuessCells[0,0].getWidth(), GuessCells[0,0].getLength());
            L1.RemoveAllViews();
            gameStatus = true;
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    if (GuessCells[i, j].getValue() != 0)
                    {
                        if (j % 3 == 0 & j != 0)
                        {
                            layoutParams.SetMargins(GuessCells[i, j].getX()-10, GuessCells[i, j].getY(), 0, 0);
                            TextView tv = new TextView(_singleTone);
                            tv.Text = "|";
                            tv.LayoutParameters = layoutParams;
                            tv.TextSize = 20;
                            L1.AddView(tv);
                        }
                        L1.AddView(GuessCells[i, j].getButton());

                    }
                    else
                    {
                        L1.AddView(GuessCells[i, j].getEmptyButton());
                    }
                    
                }
                if (i % 3 == 0&i!=0)
                {
                    LinearLayout.LayoutParams layoutParams2 = new LinearLayout.LayoutParams(GuessCells[0, 0].getWidth() , 200);
                    for (int k = 0;k<9;k++)
                    {
                        TextView tv2 = new TextView(_singleTone);
                        layoutParams2.SetMargins(GuessCells[0, 0].getLength() * k, GuessCells[0, 0].getLength() * i-75 , 0, 0);
                        tv2.Text = "_";
                        tv2.LayoutParameters = layoutParams2;
                        tv2.TextSize = 30;
                        L1.AddView(tv2);
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
            //_singleTone.SaveBoard();

            //

            finishedGenerating = true;
            GuessCells = CreateClues(cells);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    GuessCells[i, j].getButton().Click += SodokuActivity_Click;
                }
            }
            ShowBoard();
            

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
                //Toast.MakeText(this, "we can read and write the media", ToastLength.Long).Show();
            }
            else if (Android.OS.Environment.MediaMountedReadOnly.Equals(state))
            {
                //we can only read the media
                mExternalStorageAvailable = true;
                mExternalStorageWriteable = false;
                //Toast.MakeText(this, "we can only read the media", ToastLength.Long).Show();

            }
            else
            {
                //something else is wrong. we can neither rad nor write
                mExternalStorageAvailable = mExternalStorageWriteable = false;
                //Toast.MakeText(this, "something else is wrong. we can neither read nor write", ToastLength.Long).Show();
            }
        }
    }
}
