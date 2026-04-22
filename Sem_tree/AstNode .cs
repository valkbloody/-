using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class AstNode
    {
        private string _type_node = "";
        private string _type = "";
        private string _name = "";

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Type_node
        {
            get { return _type_node; }
            set { _type_node = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public AstNode()
        {
            _type_node = "AstNode";
        }
        public void Print()
        {
            Console.WriteLine(_type_node);
            Console.WriteLine($"|_type: {_type}");
            Console.WriteLine($"|_name: {_name}");
        }
    }
}
