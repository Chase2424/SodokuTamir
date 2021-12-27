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
        static SudokuCell[,] cells;
        static RelativeLayout L1;
        static List<int[,]> Solutions;
        static int result_num = 0;
        static bool finishedGenerating = false;
        private bool mExternalStorageAvailable;
        private bool mExternalStorageWriteable;
        String input;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SodokuLayout);
            // Create your application here
            L1 = (RelativeLayout)FindViewById(Resource.Id.Board);
            cells = new SudokuCell[9, 9];
            int ButtonHeight = 120, ButtonWidth = 120;
            setPermissions();
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell(j * ButtonWidth, i * ButtonHeight, 0, ButtonHeight, ButtonWidth, this);




                }
            }
            Console.WriteLine("Hello World!");
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
        public static void ShowBoard()
        {
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    L1.AddView(cells[i, j].getButton());
                }
            }
        }
        public static int RandomNumber(int end)
        {
            Random rnd = new Random();
            int number = rnd.Next(0, end);
            return number;
        }
        static bool generate_array(int[,] arr1, int x, int y, int[] allow_to_use1)
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
                            for (int k = 0; k < 9; k++)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    cells[k, j].setValue(arr[k, j]);

                                }
                            }
                            finishedGenerating = true;
                            ShowBoard();
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

        
        static bool checkBoard(int[,] arr)
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


        static bool check_sub_arr(int[,] arr, int start_pos_x, int start_pos_y)
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
        static void print_arr(int[,] arr, int pos_x, int pos_y)
        {
            int[,] arr1 = arr.Clone() as int[,];
            result_num++;
            Console.WriteLine("*****************************\n");
            Console.WriteLine("result_num:" + result_num + " x:" + pos_x + " ,y:" + pos_y);
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Console.Write(arr1[x, y] + " ");
                }
                Console.WriteLine("\n");
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
