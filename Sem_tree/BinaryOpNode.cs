using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using фапра.Sem_tree;

namespace фапра
{
    public class BinaryOpNode : AstNode
    {
        public List<BinaryOpNode> _left_leaf = new List<BinaryOpNode>();
        public List<BinaryOpNode> _right_leaf = new List<BinaryOpNode>();
        public BinaryOpNode()
        {
            this.Type_node = "BinaryOpNode";
        }
        public BinaryOpNode(string name)
        {
            this.Name = name;
        }
        public void AddOP(string name,string left, string right)
        {
            _left_leaf.Clear();
            _left_leaf.Add(new BinaryOpNode(left));
            _right_leaf.Clear();
            _right_leaf.Add(new BinaryOpNode(right));
            this.Name = name;
        }
        private string tabing(int k)
        {
            string res = "";
            for(int i = 0; i < k; i++)
            {
                res += "    ";
            }
            return res;
        }
        public string ToStr(BinaryOpNode opr, int k)
        {
            if ((opr._left_leaf[0].Name == "+" || opr._left_leaf[0].Name == "*")&& (opr._right_leaf[0].Name == "+" || opr._right_leaf[0].Name == "*"))
            {
                return "\n|     |    " + tabing(k-2) + "|_"+opr.Type_node + "\n|     |    " + tabing(k) + "|_name: " + opr.Name + "\n|     |    " + tabing(k) + "|_left_leaf: " + ToStr(opr._left_leaf[0], k + 4) + "\n|     |    " + tabing(k) + "|_right_leaf: " + ToStr(opr._right_leaf[0], k + 4);
            }
            else if (opr._left_leaf[0].Name == "+" || opr._left_leaf[0].Name == "*")
            {

                return "\n|     |    " + tabing(k-2) + "|_" + opr.Type_node + "\n|     |    "+tabing(k)+"|_name: " + opr.Name + "\n|     |    "+ tabing(k) + "|_left_leaf: " + ToStr(opr._left_leaf[0],k+4) + "\n|     |    "+ tabing(k) + "|_right_leaf: " + opr._right_leaf[0].Name;
            }
            else if (opr._right_leaf[0].Name == "+" || opr._right_leaf[0].Name == "*")
                return "\n|     |    " + tabing(k - 2) + "|_" + opr.Type_node + "\n|     |    "+tabing(k)+"|_name: " + opr.Name + "\n|     |    "+tabing(k)+"|_left_leaf: " + opr._left_leaf[0].Name + "\n|     |    "+tabing(k)+"|_right_leaf: " + ToStr(opr._right_leaf[0],k+4);

            else if (opr._left_leaf[0].Name != "+" && opr._left_leaf[0].Name != "*" && opr._right_leaf[0].Name != "+" && opr._right_leaf[0].Name != "*")
                return "\n|     |    " + tabing(k-2) + "|_" + opr.Type_node + "\n|     |     "+tabing(k)+"|_name: " + opr.Name + "\n|     |     "+tabing(k)+"|_left_leaf: " + opr._left_leaf[0].Name + "\n|     |     "+tabing(k)+"|_right_leaf: " + opr._right_leaf[0].Name;

            return "____";
        }
        public List<string> Jsonny(BinaryOpNode opr) 
        {
            string res = ToStr(opr, 0);
            string[] list = res.Split('\n'); ;
            List<string> rez= new List<string>(); 
            foreach (string s in list) rez.Add(s);
            return rez;
        }
    }
}
