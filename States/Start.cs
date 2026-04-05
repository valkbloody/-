using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра.States
{
    public class Start : State
    {
        public Start(string text, string location) 
        {
            this.text = text;
            this.location = location;
            
        }
    }
}
