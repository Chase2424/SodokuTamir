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
    public class SudokuCell : Cube
    {
       
        public int Medium = 2;
        private int x, y;
        
        private Button btn;
        private Context context;
        

        public SudokuCell(int x, int y, int Value, int length, int width, Context context) : base(length, width)
        {
            
            this.x = x;
            this.btn = new Button(context) ;

            this.btn.Text = "" + Value;
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(width, length);

            layoutParams.SetMargins(x, y+20 , 0, 0);
            this.btn.LayoutParameters = layoutParams;
            this.y = y;
            this.context = context;

        }
        public int getX()
        {
            return this.x;
        }
        public int getY()
        {
            return this.y;
        }
        public void setValue(int value)
        {
            
            this.btn.Text = ""+value;
        }
        public int getValue()
        {
            if (btn.Text != "")
            {
                return Int32.Parse(btn.Text);
            }
            return 0;
            
        }

        public Button getButton()
        {
            
            btn.Tag = 2;
            return this.btn;
        }
        public Button getEmptyButton()
        {
            this.btn.Text = "";
            this.btn.SetBackgroundColor(Android.Graphics.Color.Gray);
            
            btn.Tag = 1;
            return this.btn;
        }
        /*
        private void Btn_Click(object sender, EventArgs e)
        {
            // תנאי לסיום משחק
            // לבדוק אם הכל שונה מ-0
            // לבדוק שהכל כתוב עם עט
            //
            // למנוע מחיקה או שינוי של תאי מקור
            // להוסיף כפתור לאיפוס הלוח
            // להפריד צבעים עט ועפרון
            //so.checkBoard();
            

            
            if (permissions!=2)
            {
                this.btn.Text = ""+permissions;
                Intent i = new Intent(context, typeof(SodokuActivity));
                i.PutExtra("L",this.L);
                i.PutExtra("M",this.M);
                context.StartActivity(i);

                //int[,] board = so.getBoard(so.StringToBoard(str));
            }

            
            
            

            
        }*/
    }

}