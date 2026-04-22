using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class CharTable
    {
        private List<string> _names = new List<string>();
        private List<string> _types = new List<string>();
        public CharTable()
        {

        }
        public int lookup(string name)
        {
            int k = 0;
            foreach(string val in _names)
            {
                if (val == name) k++;
            }
            return k;
        }
        public int lookup_arith(string name)
        {
            for(int i = 0; i < _names.Count; i++)
            {
                
                if (_names[i] == name && i > 0) return 1;
            }
            return 0;
        }
        public int declare(string name, string type)
        {
            if (lookup(name) < 1)
            {
                _names.Add(name);
                _types.Add(type);
                return 1;
            }
            return 0;
        }
        public List<string> get_from_pos(int pos)
        {
            List<string> list = new List<string>();
            list.Add(_names[pos]);
            list.Add(_types[pos]);
            return list;
        }
        public int len()
        {
            return _names.Count;
        }

    }
}
