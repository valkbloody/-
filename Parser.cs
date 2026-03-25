using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private List<string> arith_exp = new List<string> { }; // выражение арифмитическое в ПОЛИС

        private List<char> opes = new List<char> { ' ' };  // буфет знаков для алгоритма Дейкстры

        private int errors_flag = 0; // 0 - знак, 1 - переменная ключ на ошибки

        private int brc_flag = 0; // ключ на скобки

        private List<string> brcs_pos = new List<string> { };  // позиция скобок
        //проверка приортета двух символов алг. Дейкстры      
        private bool IsBigger(char adding, char last_in)
            
        {
            int p1 = priority(adding),
                p2 = priority(last_in);
            if (p1 == 0 || p1 > p2) return true;
            else return false;
        }
        //проверка приортета двух символов для выталкивания алг. Дейкстры     
        private bool IsBiggerOrEq(char adding, char last_in)
        {
            int p1 = priority(adding),
                p2 = priority(last_in);
            if (p1 <= p2) return true;
            else return false;
        }
        // приоритет знаков
        private int priority(char op)
        {
            switch (op)
            {
                case '+':
                    return 7;
                case '*':
                    return 8;
                case '(':
                    return 0;
                case ')':
                    return 1;
            }
            return -1;
        }
        // Проверка корректности арифм. выражения.
        private int arith_oper()
        {
            if (cur_lexem.id == 3) // переменная
            {
                if (errors_flag == 1) // две подряд
                {
                    addError("Ожидался оператор, встречен идентификатор: " + cur_lexem.name, cur_lexem.location);
                    return 0;
                }
                arith_exp.Add(cur_lexem.name);
                errors_flag = 1;
                return 0;
            }
            else if (cur_lexem.id == 11 ||  // *
                cur_lexem.id == 10 ||       // +
                cur_lexem.id == 6 ||        // (
                cur_lexem.id == 7)          // )
            {
                if (cur_lexem.id == 11 || cur_lexem.id == 10) // оператор
                {
                    if (errors_flag == 0) // два подряд
                    {
                        addError("Ожидался инденификатор, встречен знак: " + cur_lexem.name, cur_lexem.location);
                        return 0;
                    }
                    errors_flag = 0;
                }
                if (cur_lexem.id == 6) // (
                {
                    brc_flag++;
                    brcs_pos.Add(" +"+cur_lexem.location);
                    if (errors_flag == 1)
                    {
                        addError("Отсутствует знак до открывающей скобки. ", cur_lexem.location);
                        errors_flag = 0;
                    }
                }
                if (cur_lexem.id == 7) // )
                {
                    brc_flag--;
                    brcs_pos.Add(" -" + cur_lexem.location);
                    if (errors_flag == 0)
                    {
                        addError("Отсутствует идентфикатор до закрывающей скобки. ", cur_lexem.location);
                        errors_flag = 1;
                    }
                }
                if (IsBigger(cur_lexem.name[0], opes[opes.Count - 1]) || opes.Count == 1) // доб. в буфер знаков
                {
                    if (opes[opes.Count - 1] == '(' && cur_lexem.id == 7) addError("Присутствуют незначащие скобки", cur_lexem.location);
                    opes.Add(cur_lexem.name[0]);
                }
                else
                {
                    while (IsBiggerOrEq(cur_lexem.name[0], opes[opes.Count - 1])) // выталкиваем знак из символа
                    {
                        arith_exp.Add(opes[opes.Count - 1].ToString());
                        opes.RemoveAt(opes.Count - 1);
                    }
                    if (opes[opes.Count - 1] == '(' && cur_lexem.id == 7)
                    {
                        opes.RemoveAt(opes.Count - 1);
                    }
                    else opes.Add(cur_lexem.name[0]);
                }
                return 0;
            }
            else
            {
                if (cur_lexem.id != 13)
                {
                    addError("Ожидалось арифмтическое выражение. Встречено: " + cur_lexem.name,cur_lexem.location);
                    return 0;
                }
            }
            // конец символ ; проверка на ошибки или дозапись оставшихся знаков в arith_ops
            if (brc_flag > 0) num_brcs_err(1);
            else if (brc_flag < 0) num_brcs_err(-1);
            if (arith_exp.Count < 2) 
                    {
                addError("Ожидается арифмитическое выражение, встречено: "+ cur_lexem.name,cur_lexem.location);
                return 0;
            }
            if (errors_flag != 1) addError("Встречен лишний знак: " + opes[opes.Count - 1], list_lexems[list_lexems.Count - 2].location);
                    else
                        while (opes.Count > 1)
                        {
                            arith_exp.Add(opes[opes.Count - 1].ToString());
                            opes.RemoveAt(opes.Count - 1);
                        }
            return 0;
        }
        // ошибка колечества скобок ( != )
        private void num_brcs_err(int key)
        {
            int k = 0;
            if (key == 1)
            {
                for (int i = 0; i < brc_flag; i++)
                {
                    for (int j = k; j < brcs_pos.Count; j++)
                        if (brcs_pos[j][1] == '+')
                        {
                            addError("Лишняя открывающая скобка", brcs_pos[j].Split('+')[1]);
                            k = j + 1;
                            break;
                        }
                }
            }
            else
            {
                k = brcs_pos.Count - 1;
                for (int i = 0; i < -brc_flag; i++)
                {
                    for (int j = k; j >= 0; j++)
                        if (brcs_pos[j][1] == '-')
                        {
                            addError("Лишняя закрывающая скобка", brcs_pos[j].Split('-')[1]);
                            k = j - 1;
                            break;
                        }
                }
            }
        }
        // осн. функция парсера
        public void Parse(List<Lexema> lexems)
        {
                list_lexems = lexems;
                while (cur_state != Vn.END)
                {
                    switch (cur_state)
                    {
                        case Vn.Z: // func
                            getnext();
                        if (cur_lexem.id == 2) cur_state = Vn.A;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось func, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                            break;
                        case Vn.X: // арифм. выражение
                            getnext();
                            arith_oper();
                            if (cur_lexem.id == 13) cur_state = Vn.END;
                            break;
                    case Vn.A: // <
                        getnext();
                        if (cur_lexem.id == 8) cur_state = Vn.T;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @<@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.T: // int
                        getnext();
                        if (cur_lexem.id == 1) cur_state = Vn.L;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @int@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.L: // , | >
                        getnext();
                        if (cur_lexem.id == 12) cur_state = Vn.T;
                        else if (cur_lexem.id == 9) cur_state = Vn.I;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @,@ или @>@ , встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.I: // имя функции
                        getnext();
                        if (cur_lexem.id == 3) cur_state = Vn.J;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось идентификатор, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.J: // =
                        getnext();
                        if (cur_lexem.id == 4) cur_state = Vn.E;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @=@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.E: // (
                        getnext();
                        if (cur_lexem.id == 6) cur_state = Vn.P;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @(@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.P: // переменная
                        getnext();
                        if (cur_lexem.id == 3) cur_state = Vn.R;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось идентификатор, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.R: // , || )
                        getnext();
                        if (cur_lexem.id == 12) cur_state = Vn.P;
                        else if (cur_lexem.id == 7) cur_state = Vn.M;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @,@ иди @)@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;
                    case Vn.M: // =>
                        getnext();
                        if (cur_lexem.id == 5) cur_state = Vn.X;
                        else
                        {
                            cur_state = Vn.ERR;
                            addError("Ожидалось @=>@, встречено: " + cur_lexem.name, cur_lexem.location);
                        }
                        break;

                    case Vn.ERR:
                            cur_state = Vn.END;
                            break;
                    }
                }
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
    }
}
