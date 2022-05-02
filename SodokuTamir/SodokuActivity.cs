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
using System.Collections;
using AndroidX.AppCompat.App;
using System.IO;

//TODO
// merge cells and getcells
// fix application close
// fix back button
// add clean file button
namespace SodokuTamir
{
    [Activity(Label = "Sodoku",  Theme = "@style/AppTheme")]
    public class SodokuActivity : AppCompatActivity
    {
        /// <summary>
        /// המחלקה המרכזית של המשחק בו רץ הסודוקו עצמו
        /// </summary>
        
        static SodokuActivity _singleTone;
        static ArrayList buttons_to_remove = new ArrayList();//שומרת כפתורים שצריך להסיר לפני שנסגר הפעילות בקוד כדי שלא יהיה כפילויות בשיוך הכפתורים לפעילות
        Android.Views.IMenu menu;//תפריט
        ImageButton Eraser,Pen,Pencil;//כפתורי בחירה
        int toolType = 1;//0-eraser,1-pencil,2-pen
        static int lives = 3;//כמות החיים הנותרים למשתמש
       
        static int difficulty;// קושי הסודוקו
        static SudokuCell[,] cells;//מערך דו ממדי של תאי סודוקו
        static DateTime startTime;//תאריך ההתחלה
        static TimeSpan duration;//הזמן שלקח למשתמש לפתור את הסודוקו
        public static TimerClass timerclass = new TimerClass();//מחלקת שעון
        static RelativeLayout L1;//המסך בו נציג את הכפתורים
        static int ButtonHeight = 120, ButtonWidth = 120;
        static SudokuCell[,] GuessCells;//הלוח אחרי יצירת ניחושים
        static string SaveClueBoard;//הלוח אחרי יצירת ה ניחושים בצורת מחרוזת
        
        //static int result_num = 0;//משתנה ששומש בתהליך הפיתוח לבדיקת לוחות סודוקו 
        public static LinearLayout.LayoutParams layoutParamsClicked = new LinearLayout.LayoutParams(300, 300);//עיצוב לתצוגת הכלים בשימוש 
        public static LinearLayout.LayoutParams layoutParamsNotClicked = new LinearLayout.LayoutParams(300, 300);//עיצוב לתצוגת הכלים שלא בשימוש
        static bool finishedGenerating = false;// משתנה הקובע האם הלוח סיים ליצור לוח אחד במהלך יצירת הלוח
        public static EditText et;//מקום להזנת ניחוש השחקן
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;
        
        String input;
        MyPhoneReceiver PhoneReceiver = new MyPhoneReceiver();
        LinearLayout l1;
        protected override void OnDestroy()
        {
            base.OnDestroy();
            timerclass.setStopped();
            try
            {
                UnregisterReceiver(PhoneReceiver);
            }
            catch
            {
                //הטלפון לא האזין כיוון שלא נכנסה שיחה
            }
        }
        /// <summary>
        /// פונקציה זו יוצרת את התפריט בצורה אוטומטית 
        /// הפונקציה מגדירה איזה פריט בתפריט צריך להראות לפי הSharedPreference
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            this.menu = menu;
            MenuInflater.Inflate(Resource.Menu.MainMenu, this.menu);
            this.menu.GetItem(2).SetVisible(false);
            this.menu.GetItem(3).SetVisible(false);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            else if (MainActivity.SP.GetBoolean("IsMusicOn", false) == true)
            {
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);

            }
            return true;
        }
        /// <summary>
        /// הפונקציה נקראת כאשר לוחצים על פריט בתפריט
        /// הפונקציה פועלת לפי איזה פריט שנלחץ 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (MainActivity.SP.GetBoolean("IsMusicOn", false) == false)
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", true);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(false);
                this.menu.GetItem(1).SetVisible(true);
                StartService(MainActivity.backgroundMusic);

            }
            else
            {
                var editor = MainActivity.SP.Edit();
                editor.PutBoolean("IsMusicOn", false);
                StopService(MainActivity.backgroundMusic);
                editor.Commit();
                this.menu.GetItem(0).SetVisible(true);
                this.menu.GetItem(1).SetVisible(false);
            }
            return true;

        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SodokuLayout);           
            // Create your application here
            finishedGenerating = false;
            lives = 3;
           ThreadStart ts = new ThreadStart(timerclass.Run);
            Thread thread = new Thread(ts);
            thread.Start();
            _singleTone = this;
            et = (EditText)FindViewById(Resource.Id.EditText);
            //et.Text="";
            L1 = null;
            L1 = (RelativeLayout)FindViewById(Resource.Id.Board);
            
            startTime = Convert.ToDateTime(DateTime.Now.ToString());
           
            layoutParamsClicked.SetMargins(5, 5, 5, 5);
            layoutParamsNotClicked.SetMargins(0, 0, 0, 0);
            cells = new SudokuCell[9, 9];
            Eraser = (ImageButton)FindViewById(Resource.Id.Eraser);
            Pen = (ImageButton)FindViewById(Resource.Id.Pen);
            Pencil = (ImageButton)FindViewById(Resource.Id.Pencil);
            Eraser.Click += Eraser_Click;
            Pen.Click += Pen_Click;
            Pencil.Click += Pencil_Click;
            setPermissions();
             
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, 0, ButtonHeight, ButtonWidth, this);

                }
            }
            //Console.WriteLine("Hello World!");
            
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

       

        private void Pencil_Click(object sender, EventArgs e)
        {
            this.toolType = 1;
            Pencil.LayoutParameters = layoutParamsClicked;
            Pen.LayoutParameters = layoutParamsNotClicked;
            Eraser.LayoutParameters = layoutParamsNotClicked;
        }

        private void Pen_Click(object sender, EventArgs e)
        {
            this.toolType = 2;
            Pen.LayoutParameters = layoutParamsClicked;
            Eraser.LayoutParameters = layoutParamsNotClicked;
            Pencil.LayoutParameters = layoutParamsNotClicked;
        }

        private void Eraser_Click(object sender, EventArgs e)
        {
            this.toolType = 0;
            Eraser.LayoutParameters = layoutParamsClicked;
            Pencil.LayoutParameters = layoutParamsNotClicked;
            Pen.LayoutParameters = layoutParamsNotClicked;

        }

        //פונקציה המעבירה את ערכי הלוח למחרוזת אחת
        public static string BoardToString(SudokuCell[,] arr)
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
        /// <summary>
        /// פעולה הנקראת כאשר השחקן פתר את הסודוקו ורוצה לשמור את השיא באופן מקומי
        /// הפונקציה שומרת את הפרטים של המשתמש בקובץ טקסט בתיקיית סודוקו
        /// </summary>
        public void SaveBoard()
        {
            
            if (!System.IO.File.Exists(RecordActivity.game_folder))
            {
                Directory.CreateDirectory(RecordActivity.game_folder);
            }
            string PlayerName = Intent.GetStringExtra("PlayerName");
            String record = PlayerName + "," + duration + "," + startTime + "," + SaveClueBoard;
            StreamWriter sw = System.IO.File.AppendText(RecordActivity.record_file);
            sw.WriteLine(record);
            
            

            //int fCount = Directory.GetFiles(RecordActivity.game_folder, "*.txt", SearchOption.TopDirectoryOnly).Length;
            //Toast.MakeText(this, fCount.ToString(), ToastLength.Short).Show();
            sw.Flush();
            sw.Close();

            //




        }
        /// <summary>
        /// פעולה הנגרמת מלחיצה על כפתור של משבצת סודוקו
        /// קישרתי כל כפתור לפונקציה הזאת
        /// הפונקציה בודקת את מה שהמשתמש מנחש מול הלוח המקורי בלי המקומות הריקים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void SodokuActivity_Click(object sender, EventArgs e)
            {
            Button button = sender as Button;
            int btnTag = (int)button.Tag;
            //התיוג של הכפתור משמש לדעת האם המשבצת נרשמה על ידי המשתמש או על ידינו
            if (et != null && et.Text != null && btnTag != 2)
            {
                //בדיקת סוג הכלי אשר הוא מנסה לשנות איתו
                if (toolType == 0)
                {
                    button.Hint = "";
                }
                else if (toolType == 1)
                {
                    button.Hint = et.Text;
                }
                else
                {
                    try
                    {
                        int input = Int32.Parse(et.Text);
                        if (input > 0 && input < 10)
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
                                    Toast.MakeText(this, "GAME-OVER", ToastLength.Short).Show();
                                    timerclass.setStopped();
                                    Finish();
                                }
                                else
                                {
                                    Toast.MakeText(this, "You have:" + lives + "lives left", ToastLength.Long).Show();
                                }

                            }
                            else
                            {
                                if (!BoardToString(GuessCells).Contains('0'))
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
                        Toast.MakeText(this, "please enter numbers only", ToastLength.Long).Show();


                    }
                }
            }
        }
        /// <summary>
        /// פונקציה השומרת לענן את פרטי המשתמש ומסיימת את ריצת המחלקה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Publicly_Click(object sender, EventArgs e)
        {
            //doFireWallStuff
            string PlayerName = Intent.GetStringExtra("PlayerName");
            Player p = new Player(PlayerName, "" + duration, "" + startTime, SaveClueBoard);
            await FirebaseUser.AddScore(p);
            Finish();
        }
        /// <summary>
        /// פונקציה השומרת מקומית את פרטי המשתמש ומסיימת את ריצת המחלקה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Privately_Click(object sender, EventArgs e)
        {
            SaveBoard();
            Finish();
        }

        
        /// <summary>
        /// ניקוי הלייאאוט מהכפתורים
        /// יצרתי את הפעולה הזאת מכיוון שלפעמים הלייאואט זכר כפתורים מריצות קודמות של הקוד
        /// פעולה זו פותרת את הבעיה מכיוון שהיא מסירה את רשימת הכפתורים מהלייאאוט
        /// </summary>
        public static void clear_old_board()
        {
           foreach(Button _b in buttons_to_remove)
            {
                L1.RemoveView(_b);
            }
            buttons_to_remove.Clear();
        }
        /// <summary>
        /// הפונקציה מקבלת סודוקו שלם ובהתאם לרמת הקושי מחסירה מקומות בסודוקו
        /// הפעולה מחזירה סודוקו עם חורים
        /// </summary>
        /// <param name="Original"></param>
        /// <returns></returns>
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
                while (i < 3)
                {
                    int number = rnd.Next(0, 81);
                    if (board[number] != '0')
                    {
                        i++;
                        board[number] = '0';
                    }

                }
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

                }
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

                }
            }
            else
            {
                //לא אמור לקרות, רמת משחק לא מוכרת
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
            }

            strBoard = new string(board);
            SudokuCell[,] arr2 = new SudokuCell[9, 9];
            arr2 = StringToBoard(strBoard);
            SaveClueBoard = strBoard;
            return arr2;
        }
        /// <summary>
        /// הופך מערך דו ממדי של משבצות סודוקו למערך דו ממדי של מספרים שלמים
        /// </summary>
        /// <param name="suduko"></param>
        /// <returns></returns>
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
        }
        /// <summary>
        /// הופך מחרוזת למערך דו ממדי של משבצות סודוקו
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public SudokuCell[,] StringToBoard(string str)
        {

            SudokuCell[,] arr = new SudokuCell[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    arr[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, (Char)str[i * 9 + j] - '0', ButtonHeight, ButtonWidth, this);
                }
            }
            return arr;
        }
        /// <summary>
        /// פונקציה המראה למשתמש את הלוח
        /// הפונקציה יוצרת דינמטית את הכפתורים על הלוח ואת הפסי הפרדה
        /// </summary>
        public static void ShowBoard()
        {
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(GuessCells[0, 0].getWidth(), GuessCells[0, 0].getLength());
            L1.RemoveAllViews();
            clear_old_board();
            
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    if (GuessCells[i, j].getValue() != 0)
                    {
                        if (j % 3 == 0 & j != 0)
                        {
                            layoutParams.SetMargins(GuessCells[i, j].getX() - 10, GuessCells[i, j].getY(), 0, 0);
                            TextView tv = new TextView(_singleTone);
                            tv.Text = "|";
                            tv.LayoutParameters = layoutParams;
                            tv.TextSize = 20;
                            L1.AddView(tv);
                        }
                        Button b = GuessCells[i, j].getButton();
                        buttons_to_remove.Add(b);
                        L1.AddView(b);

                    }
                    else
                    {
                        Button b = GuessCells[i, j].getEmptyButton();
                        buttons_to_remove.Add(b);
                        L1.AddView(b);
                    }

                }
                if (i % 3 == 0 & i != 0)
                {
                    LinearLayout.LayoutParams layoutParams2 = new LinearLayout.LayoutParams(GuessCells[0, 0].getWidth(), 200);
                    for (int k = 0; k < 9; k++)
                    {
                        TextView tv2 = new TextView(_singleTone);
                        layoutParams2.SetMargins(GuessCells[0, 0].getLength() * k, GuessCells[0, 0].getLength() * i - 75, 0, 0);
                        tv2.Text = "_";
                        tv2.LayoutParameters = layoutParams2;
                        tv2.TextSize = 30;
                        L1.AddView(tv2);
                    }
                }
            }
        }
        /// <summary>
        /// פונקציה המקבלת מספר ומחזירה מספר רנדומלי בין 0 למספר
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int RandomNumber(int end)
        {
            Random rnd = new Random();
            int number = rnd.Next(0, end);
            return number;
        }
        /// <summary>
        /// פעולה זו היא פעולה רקורסיבית אשר יוצרת את לוח הסודוקו
        /// </summary>
        /// <param name="arr1">הפונקציה מקבלת מערך דו ממדי של מספרים שלמים שהם בעצם הלוח סודוקו</param>
        /// <param name="x">הפונקציה מקבלת מספר שלם המציג את המיקום של המשבצת הנוכחית במערך הדו ממדי של המספרים השלמים</param>
        /// <param name="y">הפונקציה מקבלת מספר שלם המציג את המיקום של המשבצת הנוכחית במערך הדו ממדי של המספרים השלמים</param>
        /// <param name="allow_to_use1"> הפונקציה מקבלת מערך חד ממדי של איזה מספרים ניתן להשתמש באותה שורה דבר זה נוצר בכדי לייעל את הקוד</param>
        /// <returns></returns>
        public static bool generate_array(int[,] arr1, int x, int y, int[] allow_to_use1)
        {

            //הפונקציה לא עבדה כאשר עבדתי על אותו מערך בלי לשכפל אותו
            int[,] arr = arr1.Clone() as int[,];

            // אם לא תשמש בסינון של מה שכבר השתמשתי, יקח פי כמה זמן לפונקציה 
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
                // אם הגענו לפה, המספר בסדר בשורה
                arr[x, y] = current_number;

                if (checkBoard(arr))
                {

                    if (x == 8)
                    {
                        if (y == 8)
                        {
                            //הגענו לסוף הלוח, הצלחה אמיתית GREAT SUCCESS
                            //print_arr(arr, x, y);הדפסת לוח לבדיקת תקינות בתהליך הפיתוח
                            _singleTone.BoardReady(arr);
                            finishedGenerating = true;
                            return true;

                        }
                        x = 0;
                        y++;
                        // אם שורה חדשה, תן לו אופציה לכל המספרים
                        allow_to_use = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                    else
                    {
                        x++;
                    }
                    if (!finishedGenerating)
                        generate_array(arr, x, y, allow_to_use);
                }

            }


            return true;
        }
        /// <summary>
        /// הפועלה נקראת בסוף יצירת הלוח 
        /// יוצרת רמזים בלוח 
        /// משייכת את הכפתורים לפונקציית לחיצה אחת
        /// ומראה את הכפתורים למשתמש
        /// </summary>
        /// <param name="board"></param>
        public void BoardReady(int[,] board)
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
        /// <summary>
        /// פונקצית האם לוח תקין
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
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


            //לבדוק כל y
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


            //לבדוק איזורים המחולקים ל3 על 3

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

        /// <summary>
        /// פונקציה הבודקת קיבוצים של 3 על 3 של משבצות סודוקו
        /// </summary>
        /// <param name="arr">כל המערך</param>
        /// <param name="start_pos_x">ההתחלה של האיזור הנבדק</param>
        /// <param name="start_pos_y">ההתחלה של האיזור הנבדק</param>
        /// <returns></returns>
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
        /* פונקציה אשר השתמשתי לבדיקת תקינות הסודוקו בתהליך פיתוח האפליקציה
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
                    System.Console.Write(arr1[x, y] + " ");
                }
                System.Console.WriteLine("\n");
            }
        }*/




        /// <summary>
        /// פונקציה המגדירה רשות לכתוב ולקרוא לקבצים חיצוניים
        /// </summary>
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
