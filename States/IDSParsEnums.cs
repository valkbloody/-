using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра.States
{
    public class IDSParsEnums : State
    {
        public IDSParsEnums(string text, string location)
        {
            this.text = text;
            this.location = location;

        }
        public int Parse()
        {
            Stringreader SR = new Stringreader(text);
            string error_value = "";
            Begin_err_idx = get_begin();
            End_err_idx = Begin_err_idx;
            char c = ',';
            char c1 = ')';
            while (true)
            {
                if (!SR.canGetNext && (SR.GetCurrent != c && SR.GetCurrent != c1))
                {
                    if (error_value != string.Empty)
                    {
                        error_value += SR.GetCurrent;
                        errors.addError($"Ожидалось {c} или {c1}, отброшенный фрагмент: " + error_value, -1, 0, location_error());
                        return 0;
                    }
                    else
                    {
                        error_value += SR.GetCurrent;
                        errors.addError($"Ожидалось {c} или {c1}, встречено: " + error_value, -1, 0, location_error());
                        return 0;
                    }
                }
                char cur_sym = SR.GetCurrent;
                if (cur_sym == c || cur_sym == c1)
                {
                    if (error_value != string.Empty)
                    {
                        errors.addError($"Ожидалось {c} или {c1}, отброшенный фрагмент: " + error_value, -1, 0, location_error());
                        error_value = "";
                    }
                    else
                    {
                        if (cur_sym == c) break;
                        else return 2;
                    }
                        End_err_idx++;
                        cur_sym = SR.getNext;
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
            return 1;
        }
    }
}
