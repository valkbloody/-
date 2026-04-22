using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class VarDeclNode : AstNode
    {
        public VarDeclNode()
        {
            this.Type_node = "VarDeclNode";
        }
        public VarDeclNode(string name, string type) 
        {
            this.Type = type;
            this.Name = name;
            this.Type_node = "VarDeclNode";
        }
        public string ToStr()
        {
            return this.Type_node+ "\n|     |    |_name: " + this.Name+ "\n|     |    |_type: " + this.Type+"\n";
        }
    }
}
