using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace фапра
{
    public class Lexema
    {
        /// <summary>
        /// Класс представляющий лексему
        /// </summary>
        public readonly int id; // код

        public readonly string type; // тип

        public readonly string name; // в тексте

        public readonly string location; // позиция строка ..,х-у
        public Lexema(int id, string type, string name,string location)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.location = location;
        }
    } 
    public class Scanner
    {
        /// <summary>
        /// Сканер
        /// </summary>
        private enum States { S, ID, FIN, ASGN, ERR }; // конечные автоматы
        // S - старт, ID - идентификатор, FIN - конец, ASGN - для лямбда символа
        // ERR - недопустимый символ

        private States cur_state = States.S; // на старт 

        private string buf = ""; // буфер хранящий лексемы

        private char cym; // текущий символ

        private string text; // текст

        private int pos_curr = 0; // текущая позиция

        private int pos_in_line = 0; // тек позиция в линии

        private int curr_line = 1; // тек линия

        private List<Lexema> lexems = new List<Lexema>(); // список лексем

        //словарь служебных слов
        private Dictionary<int, string> key_words = new Dictionary<int, string> { { 1, "int" }, { 2, "func" } };
        //список ошибок
        private Error_page errors = new Error_page();
        public Error_page Errors
        {
            get { return errors; }
        }
        public List<Lexema> analyze(string edit_text)
        {
            text = edit_text;
            getNext();
            while (cur_state != States.FIN)
            {
                switch (cur_state)
                {
                    //начало обработки лексемы
                    case States.S:
                        if (cym == '\0')
                        {
                            getNext();
                        }
                        else if (cym == ' ')
                        {
                            AddLexema(0, "разделитель пробел", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '\n')
                        {
                            pos_in_line = 0;
                            curr_line++;
                            getNext();
                        }
                        else if (char.IsLetter(cym))
                        {
                            cur_state = States.ID;
                            addbuf(cym);
                            getNext();
                        }
                        else if (cym == ';')
                        {
                            AddLexema(13, "оператор заврешения", cym.ToString(),curr_line);
                            cur_state = States.FIN;
                        }
                        else if (cym == '=')
                        {
                            addbuf(cym);
                            getNext();
                            cur_state = States.ASGN;
                        }
                        else if (cym == '(')
                        {
                            AddLexema(6, "открывающая скобка", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == ')')
                        {
                            AddLexema(7, "закрывающая скобка", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '<')
                        {
                            AddLexema(8, "открывающая скобка треугольная", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '>')
                        {
                            AddLexema(9, "закрывающая скобка треугольная", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '*')
                        {
                            AddLexema(10, "оператор умножения", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '+')
                        {
                            AddLexema(11, "оператор присваивания", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == ',')
                        {
                            AddLexema(12, "оператор перечисления", cym.ToString(), curr_line);
                            getNext();
                        }
                        else
                        {
                            addbuf(cym);
                            cur_state = States.ERR;
                        }
                            break;
                    // идентификатор
                    case States.ID:
                        if (char.IsLetterOrDigit(cym))
                        {
                            addbuf(cym);
                            getNext();
                        }
                        else
                        {
                            int is_key_word = find_key_word(buf);
                            if (is_key_word == -1) AddLexema(3, "идентификатор", buf, curr_line);
                            else
                            {
                                string val = "служебное слово " + key_words[is_key_word].ToString();
                                AddLexema(is_key_word, val, buf, curr_line);
                            }
                            clearbuf();
                            cur_state = States.S;
                        }
                        break;
                    case States.ASGN:
                        if (cym == '>')
                        {
                            addbuf(cym);
                            AddLexema(5, "лямбда оператор", buf, curr_line);
                            getNext();
                        }
                        else
                        {
                            AddLexema(4, "оператор присваивания", buf, curr_line);
                        }
                        clearbuf();
                        cur_state = States.S;
                        
                        break;
                    case States.ERR:
                        addError(buf, curr_line);
                        clearbuf();
                        getNext();
                        cur_state = States.S;
                        break;
                    case States.FIN:
                        break;
                }
            }



            return lexems;
        }
        private int find_key_word(string buf)
        {
            foreach (var keyword in key_words) 
            { 
                if (keyword.Value == buf.ToLower())
                    return keyword.Key;
            }
            return -1;
        }
        private void AddLexema(int id, string type, string name,int location)
        {
            string loc = getLocation(name, location);
            lexems.Add(new Lexema(id, type, name, loc));
        }
        // полуение локации символа
        private string getLocation(string name, int curr_line) 
        {
            int len = name.Length;
            if (len == 1 && cur_state != States.ID) len = 0;
            int leng = pos_in_line - len;
            if (cur_state == States.ID)
            {
                
                return $"строка {curr_line}, {leng}-{pos_in_line-1}";
            }
            if (cur_state == States.ASGN)
            {
                if (len == 0) return $"строка {curr_line}, {leng - 1}-{pos_in_line-1}";
                return $"строка {curr_line}, {leng + len - 1}-{pos_in_line}";
            }
            return $"строка {curr_line}, {leng}-{pos_in_line}";
        }
        private void getNext()
        {
            try
            {
                cym = text[pos_curr];
                pos_curr++;
                pos_in_line++;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception("В конце строки не обнаружено ;");
            }
        }
        // добавление символа в буфер
        private void addbuf(char cym)
        {
            buf += cym;
        }
        // отчистить буфер
        private void clearbuf()
        {
            buf = "";
        }
        public void addError(string buf, int curr_line)
        {
            string mess = "Недопустимый символ: " + buf;
            errors.addError(mess, 0, 0, getLocation(buf, curr_line));
        }
    }
}
