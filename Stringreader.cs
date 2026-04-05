using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class Stringreader
    {
        private string str = "";

        private int index = 0;

        public string Src
        {
            get { return str; }
            set
            {
                str = value;
                index = 0;
            }
        }
        public int Index
        {
            get => index;
            set
            {
                if (value > str.Length - 1 || value < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                index = value;
            }
        }
        public char getNext
        {
            get { return str[Index++]; }
        }
        public bool canGetNext
        {
            get { if (str.Length - 1 > Index) return true; else return false; }
        }
        public char GetCurrent
        {
            get { return str[Index]; }
        }
        public bool canGetCurrent
        {
            get { if (str.Length - 1 >= Index) return true; else return false; }
        }
        public Stringreader(string text)
        {
            Src = text;
        }
    }
}
