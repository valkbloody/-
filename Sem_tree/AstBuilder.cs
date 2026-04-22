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

        private void Char_table(List<Lexema> lexems)
        {
            string name = "";
            string type = "";
            int num_types = Get_Num_Types(lexems);
            int num_ids = Get_Num_Ids(lexems);
            int nums = 0;
            if (num_types > num_ids) nums = num_types; else nums = num_ids;
            for (int i = 1; i <= nums; i++)
            {
                type = GetType(lexems, i);
                name = GetName(lexems, i);
                if (type == "")
                {
                    errors.addError($"Идентификатор не имеет типа данных: " + name, -1, 0, loc_name);
                    continue;
                }
                if (name == "")
                {
                    errors.addError($"Указан лишний тип данных: " + type, -1, 0, loc_type);
                    continue;
                }
                if (chartable.declare(name, type) == 0) errors.addError($"Идентификатор уже был объявлен ранее: " + name, -1, 0, loc_name);
            }
            function.Name = chartable.get_from_pos(0)[0];
            function.Type = chartable.get_from_pos(0)[1];
        }
        private List<string> POLIS_to_INVERS(List<string> arirh_op_polis) 
        {
            List<string> right_order = new List<string>();
            for (int i = 0; i < arirh_op_polis.Count; i++)
            {
                string cur = arirh_op_polis[i];
                if (cur == "+" || cur == "*") 
                {
                    string right = arirh_op_polis[i - 1];
                    string left = arirh_op_polis[i - 2];
                    if (chartable.lookup_arith(right) != 1 && right != "SOS") 
                        errors.addError($"Используется не объявленный идентификатор: " + right, -1, 0, get_pos(right));
                    if (chartable.lookup_arith(left) != 1 && left != "SOS") 
                        errors.addError($"Используется не объявленный идентификатор: " + left, -1, 0, get_pos(left));
                    if (right == "SOS" && left != "SOS") 
                    {
                        right_order.Add(cur + " " + left + " " + Convert.ToString(right_order.Count - 1));
                    }
                    else if (right != "SOS" && left == "SOS")
                    {
                        right_order.Add(cur + " " + Convert.ToString(right_order.Count - 1) + " " + right);
                    }
                    else if (right == "SOS" && left == "SOS")
                    {
                        right_order.Add(cur + " " + Convert.ToString(right_order.Count - 2) + " " + Convert.ToString(right_order.Count - 1));
                    }
                    else
                    {
                        right_order.Add(cur + " " + left + " " + right);
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
            Char_table(lexems);
            function.FromChartable(chartable);
            function.ArthOp(POLIS_to_INVERS(arirh_op_polis));
            List<string> tree =function.get_tree();
            return tree;
        }   
    }
}
