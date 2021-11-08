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
    class Square
    {
        
        protected int length;
        protected int width;
        public Square(int length, int width)
        {
            this.length = length;
            this.width = width;

        }
        public int getLength()
        {
            return this.length;
        }
        public int getWidth()
        {
            return this.width;
        }
    }
}