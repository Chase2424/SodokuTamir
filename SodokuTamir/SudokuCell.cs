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
        private int x, y;
        private bool Fixed = false;
        private int Value;
        private Button btn;
        private Context context;

        public SudokuCell(int x, int y, string Fixed,int Value, Context context):base(length,width)
        {
            this.x = x;
            this.y = y;
            this.context = context;
            this.Value = Value;
            if (this.Value != 0)
            {
                this.Fixed = true;
            }
            else
            {
                this.Fixed = false;
            }
      
            if (this.Fixed == false)
            {

                this.btn.SetText(this.Value);
            }
            this.btn.Click += Btn_Click;



       

        
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