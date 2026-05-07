using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace фапра.Sem_tree
{
    public class AstBuilder
    {
        private Error_page errors = new Error_page(); // список ошибок
        public Error_page Errors
        {
            get { return errors; }
        }

        private string loc_name = "";

        private string loc_type = "";

        private CharTable chartable = new CharTable();

        private FunctionDeclNode function = new FunctionDeclNode();
        public AstBuilder() { }
        private string GetName(List<Lexema> lexems, int k)
        {
            foreach (Lexema lexema in lexems)
            {
                if (lexema.id == 3)k--;
                if (lexema.id == 3 && k == 0)
                {
                    loc_name = lexema.location;
                    return lexema.name;
                }
                if (lexema.id == 5) break;

            }
            return "";
        }

        private string GetType(List<Lexema> lexems, int k)
        {
            foreach (Lexema lexema in lexems)
            {
                if (lexema.id == 1) k--;
                if (lexema.id == 1 && k == 0)
                {
                    loc_type = lexema.location;
                    return lexema.name;
                }
                if (lexema.id == 5) break;

            }
            return "";
        }

        private int Get_Num_Types(List<Lexema> lexems)
        {
            int res = 0;
            foreach (Lexema lexema in lexems)
            {
                if (lexema.id == 1) res++;
                if (lexema.id == 5) break;

            }
            return res;
        }

        private int Get_Num_Ids(List<Lexema> lexems)
        {
            int res = 0;
            foreach (Lexema lexema in lexems)
            {
                if (lexema.id == 3) res++;
                if (lexema.id == 5) break;

            }
            return res;
        }
        private List<string> POLIS_to_INVERS(List<string> arirh_op_polis) 
        {
            List<string> right_order = new List<string>();
            for (int i = 0; i < arirh_op_polis.Count; i++)
            {
                string cur = arirh_op_polis[i];
                if (cur == "+" || cur == "*" || cur == "-" || cur == "/" || cur == "%") 
                {
                    string right = arirh_op_polis[i - 1];
                    string left = arirh_op_polis[i - 2];
                    //if (chartable.lookup_arith(right) != 1 && right != "SOS") 
                    //    errors.addError($"Используется не объявленный идентификатор: " + right, -1, 0, get_pos(right));
                    //if (chartable.lookup_arith(left) != 1 && left != "SOS") 
                    //    errors.addError($"Используется не объявленный идентификатор: " + left, -1, 0, get_pos(left));
                    if (right == "SOS" && left != "SOS")
                    {
                        if (int.TryParse(left, out int result))
                        {
                            right_order.Add(cur + " " + left + " #" + Convert.ToString(right_order.Count - 1) + " ");
                            right_order[right_order.Count - 1] += oper_res(right_order);
                        }
                        else
                        right_order.Add(cur + " " + left + " #" + Convert.ToString(right_order.Count - 1));
                    }
                    else if (right != "SOS" && left == "SOS")
                    {
                        if (int.TryParse(right, out int result))
                        {
                            right_order.Add(cur + " #" + Convert.ToString(right_order.Count - 1) + " " + right + " ");
                            right_order[right_order.Count - 1] += oper_res(right_order);
                        }
                        else
                        right_order.Add(cur + " #" + Convert.ToString(right_order.Count - 1) + " " + right);
                    }
                    else if (right == "SOS" && left == "SOS")
                    {
                        right_order.Add(cur + " #" + Convert.ToString(right_order.Count - 2) + " #" + Convert.ToString(right_order.Count - 1) + " ");
                        right_order[right_order.Count - 1] += oper_res(right_order);
                    }
                    else
                    {
                        if (int.TryParse(left, out int result3) && int.TryParse(right, out int result4))
                        {
                            right_order.Add(cur + " " + left + " " + right + " " + action(right_order,left,right,cur[0]));
                        }
                        else
                        {
                            right_order.Add(cur + " " + left + " " + right);
                        }
                    }
                    arirh_op_polis.RemoveAt(i - 1);
                    arirh_op_polis.RemoveAt(i - 1);
                    i = i - 2;
                    arirh_op_polis[i] = "SOS";
                }
            }
            return right_order;
        }
        private List<Lexema> lexemsa = new List<Lexema>();
        private int oper_res(List<string> table)
        {
            string op = table[table.Count - 1];
            string[] splitted = op.Split(' ');
            char sign = op[0];
            int key = 0;
            string v1 = splitted[1];
            string v2 = splitted[2];
            string r1 = v1;
            string r2 = v2;
            if (v1[0] == '#') r1 = get_prev(table, v1.Remove(0, 1));
            if (v2[0] == '#') r2 = get_prev(table, v2.Remove(0, 1));
            return action(table,r1,r2, sign);
        }
        private string get_prev(List<string> table, string id)
        {
            string op = table[Convert.ToInt32(id)];
            string[] splitted = op.Split(' ');
            return splitted[3];
        }
        private int action(List<string> table, string v1, string v2, char sign)
        {
            int res = 0;
            switch (sign)
            {
                case '-':
                    res = Convert.ToInt32(v1) - Convert.ToInt32(v2);
                    break;
                case '+':
                    res = Convert.ToInt32(v1) + Convert.ToInt32(v2);
                    break;
                case '%':
                    res = Convert.ToInt32(v1) % Convert.ToInt32(v2);
                    break;
                case '/':
                    res = Convert.ToInt32(v1) / Convert.ToInt32(v2);
                    break;
                case '*':
                    res = Convert.ToInt32(v1) * Convert.ToInt32(v2);
                    break;
            }
            return res;
        }
        
            private string get_pos(string need)
        {
            int num = chartable.lookup(need);
            foreach(Lexema lexema in lexemsa)
            {
                if (lexema.name == need)
                {
                    if (num == 0)
                        return lexema.location;
                    else num--;
                }
            }
            return "";

        }
        public List<string> Get_Result(List<Lexema> lexems, List<string> arirh_op_polis)
        {
            lexemsa = lexems;
            return POLIS_to_INVERS(arirh_op_polis); 
        }   
    }
}
