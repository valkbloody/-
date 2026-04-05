using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра.States
{
    public class State
    {
        public string text = "";

        public string location = "";

        public int Begin_err_idx = 0;

        public int End_err_idx = 0;

        public Error_page errors = new Error_page(); // список ошибок

        public Error_page Errors
        {
            get { return errors; }
        }
        public int get_begin()
        {
            string inline = location.Split(' ')[2];
            return Convert.ToInt32(inline.Split('-')[0]);
        }
        public string location_error()
        {
            string row_numstr = location.Split(' ')[1];
            row_numstr = row_numstr.Split(',')[0];
            int row_num = Convert.ToInt32(row_numstr);
            return $"строка {row_num}, {Begin_err_idx}-{End_err_idx - 1}";
        }
        public int Parse(string word)
        {
            Stringreader SR = new Stringreader(text);
            string error_value = "";
            Begin_err_idx = get_begin();
            End_err_idx = Begin_err_idx;
            int get_full = 0;
            foreach (char c in word)
            {
                while (true)
                {
                    if (!SR.canGetNext && SR.GetCurrent != c)
                    {
                        if (error_value != string.Empty)
                        {
                            error_value += SR.GetCurrent;
                            errors.addError($"Ожидалось {word}, отброшенный фрагмент: "+error_value, -1, 0, location_error());
                            if (get_full == 1) return 1;
                            return 0;
                        }
                        else
                        {
                            error_value += SR.GetCurrent;
                            errors.addError($"Ожидалось {word}, встречено: " + error_value, -1, 0, location_error());
                            return 0;
                        }
                    }
                    char cur_sym = SR.GetCurrent;
                    if (cur_sym == c)
                    {
                        if (error_value != string.Empty)
                        {
                            errors.addError($"Ожидалось {word}, отброшенный фрагмент: " + error_value, -1, 0, location_error());
                            error_value = "";
                        }
                        End_err_idx++;
                        if (word[word.Length - 1] == cur_sym)
                        {
                            if (!SR.canGetNext) break;
                            get_full = 1;
                        }
                        else
                        {
                            if (SR.canGetNext) cur_sym = SR.getNext;
                            break;
                        }
                    }
                    else
                    {
                        if (error_value == string.Empty)
                        {
                            Begin_err_idx = End_err_idx;
                        }
                        error_value += cur_sym;
                    }
                    if (SR.canGetNext) cur_sym = SR.getNext;
                    End_err_idx++;
                }
            }
            return 1;
        }
    }
}
