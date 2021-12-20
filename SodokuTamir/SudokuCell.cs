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
        private int[] remaining;
        private int x, y;
        private bool Fixed = false;
        private int Value;
        private Button btn ;
        private Context context;

        public SudokuCell(int x, int y, int Value, int length, int width, Context context) : base(length, width)
        {
            remaining = new int[9];
            for (int i = 1; i < 10; i++)
            {
                remaining[i] = i;
            }

            this.x = x;
            this.btn = new Button(context);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(width, length);

            layoutParams.SetMargins(x + width - 120, y + length - 100, 0, 0);
            this.btn.LayoutParameters = layoutParams;


            this.y = y;
            this.context = context;
            this.Value = Value;
            //this.btn.Width = this.x + this.width;
            if (this.Value != 0)
            {
                this.Fixed = true;
            }
            else
            {
                this.Fixed = false;
            }

            // if (this.Fixed == false)
            //{

            //   this.btn.SetText(this.Value);
            // }
            this.btn.Click += Btn_Click;






        }

        public void setValue(int value)
        {
            this.Value = value;
        }
        public int getValue()
        {
            return this.Value;
        }
        public Button getButton()
        {
            return this.btn;
        }
        private void Btn_Click(object sender, EventArgs e)
        {
           if (Fixed)
            {
                
            }
        }
        /*public int Value { get; set; }
public bool Fixed { get; set; }
public int X { get; set; }
public int Y { get; set; }
Button btn;
public void Clear()
{
   this.Value = "";
   this.Fixed= false;
}
public SudokuCell(int Value,int x,int y,Context context)
{
   if(this.Value!=0)
   {
       this.Fixed = true;
   }
   else
   {
       this.Fixed = false;
   }
   this.Value = Value;
   this.btn = new Button(context);
   this.X = X;
   this.Y = Y;
   if(this.Fixed == false)
   {
       btn.SetText(String.valueOf(this.Value));
   }
   this.btn.Click += Btn_Click;



}

private void Btn_Click(object sender, System.EventArgs e)
       {
           if(Fixed)
                {

                }
       }
*/
    }

}