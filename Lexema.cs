using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class Lexema
    {
        /// <summary>
        /// Класс представляющий лексему
        /// </summary>
        public readonly int id; // код

        public readonly string type; // тип

        public readonly string name; // в тексте

        public readonly string location; // позиция строка ..,х-у
        public Lexema(int id, string type, string name, string location)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.location = location;
        }
    }
}
