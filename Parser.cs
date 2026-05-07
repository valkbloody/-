using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using фапра.States;

namespace фапра
{
    public class Parser
    {
        /// <summary>
        /// Класс Парсер
        /// </summary>
        private enum Vn { Z, A, T, L, I, J, E, P, R, M, N, X, B, O, END, ERR } // нетерминальные символы
        
        private Vn cur_state = Vn.Z; // текущее состяние

        private int cur_lexem_id = -1; // индекс текущей ликсемы

        private List<Lexema> list_lexems; // список всех лексем

        private Lexema cur_lexem; // текущая лексема

        private Error_page errors = new Error_page(); // список ошибок
        public Error_page Errors 
        {
            get { return errors; }
        }
 
        public List<string> arith_opes = new List<string>();
        private bool ExcpectedError(string exp_str)
        {
            if (!CANgetnext)
            {
                errors.addError($"Ожидалось {exp_str}, но не было получено", -1, 0, cur_lexem.location);
                return true;
            }
            return false;
        }
        // осн. функция парсера
        public void Parse(List<Lexema> lexems)
        {
            list_lexems = lexems;
            getnext();

            ArithOpesState arith_op = new ArithOpesState(cur_lexem.name.ToLower(), cur_lexem.location);
            while (true)
            {
                arith_op.arith_oper(cur_lexem.id,cur_lexem.name, cur_lexem.location);
                if (!CANgetnext)
                {
                    arith_op.arith_oper(13, ";", cur_lexem.location);
                    break;
                }
                getnext();

            }
            errors.addErrors(arith_op.Errors.path, arith_op.Errors.line, arith_op.Errors.column, arith_op.Errors.message);
            arith_opes = arith_op.arith_exp;
        }
        public void addError(string buf, string location)
        {
            errors.addError(buf, -1, 0, location);
        }
        private void getnext()
        {
            cur_lexem_id++;
            cur_lexem = list_lexems[cur_lexem_id];
        }
        private bool CANgetnext
        {
            get => cur_lexem_id < list_lexems.Count-1;
        }

    }
}
