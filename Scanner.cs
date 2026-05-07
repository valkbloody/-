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
    public class Scanner
    {
        /// <summary>
        /// Сканер
        /// </summary>
        private enum States { S, ID, FIN, NUM, ERR }; // конечные автоматы
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
                        if (cym == '\0' || cym == ' ')
                        {
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
                        else if (char.IsDigit(cym))
                        {
                            cur_state = States.NUM;
                            addbuf(cym);
                            getNext();
                        }
                        else if (cym == '(')
                        {
                            AddLexema(3, "открывающая скобка", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == ')')
                        {
                            AddLexema(4, "закрывающая скобка", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '/')
                        {
                            AddLexema(5, "оператор деления", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '%')
                        {
                            AddLexema(9, "остаток от деления", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '*')
                        {
                            AddLexema(6, "оператор умножения", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '+')
                        {
                            AddLexema(7, "оператор сложения", cym.ToString(), curr_line);
                            getNext();
                        }
                        else if (cym == '-')
                        {
                            AddLexema(8, "оператор разности", cym.ToString(), curr_line);
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
                        if (char.IsLetterOrDigit(cym) || cym == ' ')
                        {
                            if (cym != ' ') addbuf(cym);
                            getNext();
                        }
                        else
                        {
                            AddLexema(1, "идентификатор", buf, curr_line);
                            clearbuf();
                            cur_state = States.S;
                        }
                        break;
                    case States.NUM:
                        if (char.IsDigit(cym))
                        {
                            addbuf(cym);
                            getNext();
                        }
                        else
                        {
                            AddLexema(2, "число", buf, curr_line);
                            clearbuf();
                            cur_state = States.S;
                        }                        
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
            if (cur_state == States.ID || cur_state == States.NUM)
            {
                
                return $"строка {curr_line}, {leng}-{pos_in_line-1}";
            }
            return $"строка {curr_line}, {leng}-{pos_in_line}";
        }
        private void getNext()
        {
            if (pos_curr < text.Length)
            {
                cym = text[pos_curr];
                pos_curr++;
                pos_in_line++;
            }
            else
            {
                if (cur_state == States.ID) AddLexema(1, "идентификатор", buf, curr_line);
                if (cur_state == States.NUM) AddLexema(2, "число", buf, curr_line);
                cur_state = States.FIN;
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
            errors.addError(buf, -1, 0, getLocation(buf, curr_line));
        }
    }
}
