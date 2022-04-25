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
    public class Cube
    {
        
        protected int length;
        protected int width;
        protected int x, y;
        public Cube(int x, int y, int length, int width)
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
        public int getX()
        {
            return this.x;
        }
        public int getY()
        {
            return this.y;
        }
    }
}