using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace фапра
{
    public class Error_page
    {
        /// <summary>
        /// Класс который представляет ошибки
        /// </summary>
        public List<string> path = new List<string>(); // путь

        public List<int> line = new List<int>(); // линия

        public List<int> column = new List<int>(); // колонка

        public List<string> message = new List<string>(); // сообщение
        public Error_page()
        {

        }
        public Error_page(string path, int line, int column, string message)
        {
            addError(path, line, column, message);
        }
        public void addError(string path = "", int line = 0, int column = 0, string message = "")
        {
            this.path.Add(path);
            this.line.Add(line);
            this.column.Add(column);
            this.message.Add(message);
        }
        public void addErrors(List<string> path, List<int> line, List<int> column, List<string> message)
        {
            for (int i = 0; i < path.Count; i++)
            {
                this.path.Add(path[i]);
                this.line.Add(line[i]);
                this.column.Add(column[i]);
                this.message.Add(message[i]);
            }
        }
    }
}
