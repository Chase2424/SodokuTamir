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
    public abstract class Cube
    {
        /// <summary>
        /// מחלקה בשם מרובע, המתארת רעיונית מהו מרובע אובייקט בעל עורך ורוחב 
        /// </summary>
        protected int length;//אורך האובייקט
        protected int width;//רוחב האובייקט
        
        public Cube(int length, int width)
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