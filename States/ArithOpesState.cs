using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра.States
{
    public class ArithOpesState : State
    {
        public List<string> arith_exp = new List<string> { }; // выражение арифмитическое в ПОЛИС

        private List<char> opes = new List<char> { ' ' };  // буфет знаков для алгоритма Дейкстры

        private int errors_flag = 0; // 0 - знак, 1 - переменная ключ на ошибки

        private int brc_flag = 0; // ключ на скобки

        private List<string> brcs_pos = new List<string> { };  // позиция скобок 
        
        public ArithOpesState(string text, string location) 
        {
            this.text = text;
            this.location = location;
            
        }
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
        public int arith_oper(int id, string name, string location)
        {
            if (id == 3) // переменная
            {
                if (errors_flag == 1) // две подряд
                {
                    errors.addError("Ожидался оператор, встречен идентификатор: " + name, -1, 0, location);
                    return 0;
                }
                arith_exp.Add(name);
                errors_flag = 1;
                return 0;
            }
            else if (id == 11 ||  // *
                id == 10 ||       // +
               id == 6 ||        // (
               id == 7)          // )
            {
                if (id == 11 || id == 10) // оператор
                {
                    if (errors_flag == 0) // два подряд
                    {
                        errors.addError("Ожидался инденификатор, встречен знак: " + name, -1, 0 ,location);
                        return 0;
                    }
                    errors_flag = 0;
                }
                if (id == 6) // (
                {
                    brc_flag++;
                    brcs_pos.Add(" +" + location);
                    if (errors_flag == 1)
                    {
                        errors.addError("Отсутствует знак до открывающей скобки. ",-1,0 ,location);
                        errors_flag = 0;
                    }
                }
                if (id == 7) // )
                {
                    brc_flag--;
                    brcs_pos.Add(" -" + location);
                    if (errors_flag == 0)
                    {
                        errors.addError("Отсутствует идентфикатор до закрывающей скобки. ", -1, 0,location);
                        errors_flag = 1;
                    }
                }
                if (IsBigger(name[0], opes[opes.Count - 1]) || opes.Count == 1) // доб. в буфер знаков
                {
                    if (opes[opes.Count - 1] == '(' && id == 7) errors.addError("Присутствуют незначащие скобки", -1, 0 ,location);
                    opes.Add(name[0]);
                }
                else
                {
                    while (IsBiggerOrEq(name[0], opes[opes.Count - 1])) // выталкиваем знак из символа
                    {
                        arith_exp.Add(opes[opes.Count - 1].ToString());
                        opes.RemoveAt(opes.Count - 1);
                    }
                    if (opes[opes.Count - 1] == '(' && id == 7)
                    {
                        opes.RemoveAt(opes.Count - 1);
                    }
                    else opes.Add(name[0]);
                }
                return 0;
            }
            else
            {
                if (id != 13)
                {
                    errors.addError("Ожидалось арифмтическое выражение. Встречено: " + name, -1, 0 , location);
                    return 0;
                }
            }
            // конец символ ; проверка на ошибки или дозапись оставшихся знаков в arith_ops
            if (brc_flag > 0) num_brcs_err(1);
            else if (brc_flag < 0) num_brcs_err(-1);
            if (arith_exp.Count < 2)
            {
                errors.addError("Ожидается арифмитическое выражение, встречено: " + name, -1, 0 ,location);
                return 0;
            }
            if (errors_flag != 1) errors.addError("Встречен лишний знак: " + opes[opes.Count - 1], -1,0 ,location);
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
                            errors.addError("Лишняя открывающая скобка",-1, 0, brcs_pos[j].Split('+')[1]);
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
                            errors.addError("Лишняя закрывающая скобка",-1, 0, brcs_pos[j].Split('-')[1]);
                            k = j - 1;
                            break;
                        }
                }
            }
        }
    }
}
