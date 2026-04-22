using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace фапра
{
    public class FunctionDeclNode : AstNode
    {
        private List<VarDeclNode> parameters = new List<VarDeclNode>();
        private BinaryOpNode body = new BinaryOpNode();
        public FunctionDeclNode()
        {
            this.Type_node = "FunctionDeclNode";
        }
        public void FromChartable(CharTable chartable)
        {
            for (int i = 0; i < chartable.len(); i++)
            {
                List<string> list = chartable.get_from_pos(i);
                parameters.Add(new VarDeclNode(list[0], list[1]));
            }
        }
        public List<string> get_tree()
        {
            List<string> res = new List<string>();
            res.Add(this.Type_node);
            res.Add("|__name: " + this.Name);
            res.Add("|__type: " + this.Type);
            res.Add("|__params: ");
            for (int i = 1; i < parameters.Count; i++)
            {
                res.Add("|     |__" + parameters[i].ToStr() + "|     |");
            }
            res.Add("|__body: ");
            foreach (string str in body.Jsonny(body)) if (!String.IsNullOrWhiteSpace(str))res.Add(str);

            return res;
        }

        private BinaryOpNode leaf(int indx, BinaryOpNode leafy, List<string> artih_op)
        {
            string[] cur = artih_op[indx].Split(' ');
            string oper = cur[0];
            string lefty = cur[1];
            string righty = cur[2];
            if (int.TryParse(lefty, out int result1))
            {
                leafy.Name = oper;
                leafy._right_leaf.Add(new BinaryOpNode());
                leafy._left_leaf.Add(new BinaryOpNode());
                leafy._right_leaf[0].Name = righty;
                leaf(result1, leafy._left_leaf[0], artih_op);
            }
            if (int.TryParse(righty, out int result))
            {
                leafy.Name = oper;
                leafy._right_leaf.Add(new BinaryOpNode());
                if (leafy._left_leaf.Count == 0)
                {
                    leafy._left_leaf.Add(new BinaryOpNode());
                    leafy._left_leaf[0].Name = lefty;
                }
                leaf(result, leafy._right_leaf[0], artih_op);
            }
            if (!int.TryParse(righty, out int result3) &&
                !int.TryParse(lefty, out int result4))
            {
                leafy.AddOP(oper, lefty, righty);
                return leafy;
            }
            return leafy;
        }
        public void ArthOp(List<string> artih_op)
        {
            body._left_leaf.Add(new BinaryOpNode());
            body._right_leaf.Add(new BinaryOpNode());
            int i = artih_op.Count - 1;
            string[] cur = artih_op[i].Split(' ');
            string oper = cur[0];
            string lefty = cur[1];
            string righty = cur[2];
            int q = 0;
            if (int.TryParse(lefty, out int result1))
            {
                body._left_leaf[0] = leaf(result1, body._left_leaf[0], artih_op);
                body._right_leaf[0].Name = righty;
                q = 1;
                
            }
            if (int.TryParse(righty, out int result))
            {
                body._right_leaf[0] = leaf(result, body._right_leaf[0], artih_op);
                if (q == 0) body._left_leaf[0].Name =lefty;
                q = 1;
            }
            body.Name = oper;
            if (q == 0)
            {
                body.AddOP(oper,lefty,righty);  
            }
       }
     
    }
}

