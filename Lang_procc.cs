using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace фапра
{
    public partial class Lang_procc : Form
    {
        // Текущая строка
        private int curr_row = 0;
        
        // Текущее слово
        private string word = "";

        // Время выполнения
        private int time = 0;

        // Пути файлов
        private List<string> paths = new List<string>();

        //Ошибки в файлах
        public List<Error_page> errors = new List<Error_page>();
        public Lang_procc()
        {
            InitializeComponent();
            // только для сканера
            error_grid.Columns[0].HeaderText = "Условный код";
            error_grid.Columns[1].HeaderText = "Тип лексемы";
            error_grid.Columns[1].Width = 200;
            error_grid.Columns[2].HeaderText = "Лексема";
            error_grid.Columns[2].Width = 200;
            error_grid.Columns[3].HeaderText = "Местоположение";
            change_font(this, 12);

        }
        
        // Пункт меню Правка
        private void ctrla(object sender, EventArgs e)
        {
            RichTextBox edit_box = sender as RichTextBox;
            edit_box.Focus();
            SendKeys.Send("^(a)");
        }
        private void ctrlc(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }
        private void ctrlv(object sender, EventArgs e)
        {
            SendKeys.Send("^V");
        }
        private void ctrlx(object sender, EventArgs e)
        {
            SendKeys.Send("^X");
        }
        private void delete(object sender, EventArgs e)
        {
            SendKeys.Send("{DELETE}");
        }
        private void ctrlz(object sender, EventArgs e)
        {
            SendKeys.Send("^Z");
        }
        private void ctrly(object sender, EventArgs e)
        {
            SendKeys.Send("^Y");
        }
        public void add_error()
        {
            Error_page newerror = new Error_page();
            errors.Add(newerror);
        }
        private void clear_errors(int id)
        {
            errors[id].path.Clear();
            errors[id].line.Clear();
            errors[id].column.Clear();
            errors[id].message.Clear();
        }
        // Создание страницы Tabpage для работы с файлом
        private TabPage New_tabpage1(string name = "Новый документ") 
        {
            TabPage my_tab = new TabPage(name);
            RichTextBox code = new RichTextBox(); // Текст файла
            my_tab.Resize += resize;
            my_tab.Controls.Add(code);
            code.Location = new System.Drawing.Point(38, 3);
            code.Width = programs.Width - code.Location.X;
            code.Height = programs.Height - 50;
            code.KeyPress += edit_box_KeyPress;
            code.TextChanged += edit_box_TextChanged;
            code.VScroll += edit_box_TextChanged;
            code.MouseClick += edit_box_TextChanged;
            code.KeyDown += edit_box_Keydown;
            code.ScrollBars = RichTextBoxScrollBars.Both;
            code.MinimumSize = new Size(500,60);
            RichTextBox strings = new RichTextBox(); // Номер строк
            my_tab.Controls.Add(strings);
            strings.Location = new System.Drawing.Point(3, 3);
            strings.Width = code.Location.X - strings.Location.X;
            strings.Height = code.Height;
            strings.ReadOnly = true;
            strings.ScrollBars = RichTextBoxScrollBars.None;
            сохранитьToolStripMenuItem.Enabled = true;
            сохранитьКакToolStripMenuItem.Enabled = true;
            save_file_but.Enabled = true;
            start_but.Enabled = true;   
            paths.Add(" ");
            add_error();
            return my_tab;

        }
        // Справочная система
        private void help_but_Click(object sender, EventArgs e)
        {
            Form help = new Form();
            help.Size = new System.Drawing.Size(700, 700);
            help.Text = "Справочная служба";
            RichTextBox info = new RichTextBox();
            info.Font = toolStripStatusLabel1.Font;
            info.Size = new System.Drawing.Size(700-100, 700-100);
            info.Location = new Point(50, 50);
            info.Text = "Меню приложения:" + Environment.NewLine +
                "Файл: Создать файл - создатеся новый файл в текстовом редакторе(CTRL+N)" + Environment.NewLine +
                "Сохранить - (CTRL+S), Cохранить как - появляется диалоговое окно" + Environment.NewLine +
                "Пуск: появляется сообщение, что это кнопка пуск, также если пустой файл - выдаст ошибку (F5)" + Environment.NewLine +
                "Вызов справочной службы (F10)" + Environment.NewLine +
                "Язык: можно поментяь русский на китайский" + Environment.NewLine +
                "Размер текста: + прибавить - убавить";

            help.Controls.Add(info);
            help.ShowDialog();
                    
        }
        private void Lang_procc_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            if (programs.SelectedIndex > -1)
            {
                result = MessageBox.Show("У вас есть несохраненный файл, сохранить?", "сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    сохранитьToolStripMenuItem.PerformClick();
                }
            }
        }
        private void resize(object sender, EventArgs e)
        {
            object numstr_obj = programs.SelectedTab.GetChildAtPoint(new Point(50, 50));
            RichTextBox edit_box = numstr_obj as RichTextBox;
            numstr_obj = programs.SelectedTab.GetChildAtPoint(new Point(4, 4));
            RichTextBox num_str = numstr_obj as RichTextBox;
            if (edit_box != null && num_str != null)
            {
                edit_box.Width = programs.Width - edit_box.Location.X;
                edit_box.Height = programs.Height - 50;
                num_str.Height = edit_box.Height;
                error_grid.Width = programs.Width - error_grid.Location.X;
                error_grid.Height = this.Height - edit_box.Height - edit_box.Top;
            }
        }
        //Пункт меню Файл
        private void newfile(object sender, EventArgs e)
        {
            programs.TabPages.Add(New_tabpage1());
        }
        private void open_file(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private void save_as(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = programs.SelectedTab.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            object numstr_obj = programs.SelectedTab.GetChildAtPoint(new Point(50, 50));
            RichTextBox edit_box = numstr_obj as RichTextBox;
            System.IO.File.WriteAllText(filename, edit_box.Text);
            paths.RemoveAt(programs.SelectedIndex);
            paths.Insert(programs.SelectedIndex, filename);
            textBox1.Text = filename;
            programs.SelectedTab.Text = textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Count() - 1];
        }
        private void save(object sender, EventArgs e)
        {
            if (textBox1.Text == "") save_as(sender, e);
            else
            {
                string filename = textBox1.Text;
                object numstr_obj = programs.SelectedTab.GetChildAtPoint(new Point(50, 50));
                RichTextBox edit_box = numstr_obj as RichTextBox;
                System.IO.File.WriteAllText(filename, edit_box.Text);
            }
        }
        private void exit(object sender, EventArgs e)
        {
            this.Close();
        }
        // Кнопка Пуск
        private void start(object sender, EventArgs e)
        {
            try
            {
                object numstr_obj = programs.SelectedTab.GetChildAtPoint(new Point(50, 50));
                RichTextBox edit_box = numstr_obj as RichTextBox;
                error_grid.Rows.Clear();
                //создание сканера
                Scanner scanner = new Scanner();
                List<Lexema> lixemas = scanner.analyze(edit_box.Text);
                //сначала лексемы
                int selpage = programs.SelectedIndex;
                errors.RemoveAt(selpage);
                errors.Insert(selpage, scanner.Errors);
                //потом ошибки
                for (int i = 0; i < errors[selpage].column.Count; i++)
                {
                    error_grid.Rows.Add(errors[selpage].line[i],"лексическая ошибка" ,errors[selpage].path[i], errors[selpage].message[i]);
                }
                foreach (Lexema lexema in lixemas)
                {
                    error_grid.Rows.Add(lexema.id, lexema.type, lexema.name, lexema.location);
                }
                if (errors[selpage].column.Count == 0) MessageBox.Show("Ошибок не обнаружено!");
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Обработчик событий переключениия между вкладок
        private void programs_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = paths[programs.SelectedIndex];
            error_grid.Rows.Clear();
            Error_page errors_on_page = errors[programs.SelectedIndex];
            for(int i = 0; i < errors_on_page.message.Count;i++)
            {
                if (errors_on_page.message[i] != " " && errors_on_page.message[i] != "")
                {
                    error_grid.Rows.Add(errors_on_page.path[i], errors_on_page.line[i], 
                        errors_on_page.column[i], errors_on_page.message[i]);
                }
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            programs.TabPages.Add(New_tabpage1(openFileDialog1.SafeFileName));
            programs.SelectedTab = programs.TabPages[programs.TabPages.Count - 1];
            object numstr_obj = programs.TabPages[programs.TabPages.Count - 1].GetChildAtPoint(new Point(50, 50));
            RichTextBox edit_box = numstr_obj as RichTextBox;
            edit_box.Text = File.ReadAllText(openFileDialog1.FileName);
            paths.RemoveAt(programs.SelectedIndex);
            paths.Insert(programs.SelectedIndex, openFileDialog1.FileName);
            textBox1.Text = openFileDialog1.FileName;
        }
        private void dexter1(object sender, FormClosedEventArgs e)
        {
            dexter.Visible = false;
        }
        // О приложении
        private void about(object sender, EventArgs e)
        {
            Form about = new Form();
            about.Text = "О программе";
            about.StartPosition = FormStartPosition.CenterScreen;
            about.FormClosed += dexter1;
            System.Windows.Forms.TextBox te = new System.Windows.Forms.TextBox();
            te.ReadOnly = true;
            te.Multiline = true;
            te.Dock = DockStyle.Fill;
            te.Text = "Программа - языковой процессор!" + Environment.NewLine + "Выполнил: Плахин Даниил АП-327"
                + Environment.NewLine + "Проверил: Антонянц Егор Николаевич асс. каф. АСУ.";
            dexter.Visible = true;
            about.Controls.Add(te);
            about.Show();
        }
        private void text_changes_plus(object sender, EventArgs e)
        {
            float size = paste_but.Font.Size;
            Size begin_size = this.Size;
            if (size < 20.0)
            {
                change_font(this, size + 1);
                this.Size = begin_size;
                toolStripStatusLabel1.Text = $"Шрифт: Segoe UI {size + 1}pt";
            }
        }
        private void text_changes_minus(object sender, EventArgs e)
        {
            float size = paste_but.Font.Size;
            if (size > 11.0)
            {
                change_font(this, size - 1);
                toolStripStatusLabel1.Text = $"Шрифт: Segoe UI {size - 1}pt";
            }
        }
        private void change_font(Control control, float size)
        {
            control.Font = new Font("Segoe UI",size);
            foreach (Control sub in control.Controls)
            {
                change_font(sub, size);
            }
        }

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            change_lang(0);
        }

        private void китайскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            change_lang(1);
        }
        private void change_lang(int key)
        {
            if (key == 0) файлToolStripMenuItem.Text = "Файл";
            else файлToolStripMenuItem.Text = "文件";

            if (key == 0) правкаToolStripMenuItem.Text = "Правка";
            else правкаToolStripMenuItem.Text = "编辑";

            if (key == 0) текстToolStripMenuItem.Text = "Текст";
            else текстToolStripMenuItem.Text = "文本";

            if (key == 0) пускToolStripMenuItem.Text = "Пуск";
            else пускToolStripMenuItem.Text = "开始";

            if (key == 0) справкаToolStripMenuItem.Text = "Справка";
            else справкаToolStripMenuItem.Text = "参考";

            if (key == 0) размерТекстаToolStripMenuItem.Text = "Размер текста";
            else размерТекстаToolStripMenuItem.Text = "文字大小";

            if (key == 0) языкToolStripMenuItem.Text = "Язык";
            else языкToolStripMenuItem.Text = "语言";

            if (key == 0) создатьToolStripMenuItem.Text = "Создать";
            else создатьToolStripMenuItem.Text = "创造";

            if (key == 0) открытьToolStripMenuItem.Text = "Открыть";
            else открытьToolStripMenuItem.Text = "打开";

            if (key == 0) сохранитьToolStripMenuItem.Text = "Сохранить";
            else сохранитьToolStripMenuItem.Text = "节省";

            if (key == 0) сохранитьКакToolStripMenuItem.Text = "Сохранить как";
            else сохранитьКакToolStripMenuItem.Text = "另存为";

            if (key == 0) выходToolStripMenuItem.Text = "Выход";
            else выходToolStripMenuItem.Text = "出口";

            if (key == 0) отменитьToolStripMenuItem.Text = "Отменить";
            else отменитьToolStripMenuItem.Text = "取消";

            if (key == 0) повторитьToolStripMenuItem.Text = "Повторить";
            else повторитьToolStripMenuItem.Text = "重复";

            if (key == 0) вырезатьToolStripMenuItem.Text = "Вырезать";
            else вырезатьToolStripMenuItem.Text = "切";

            if (key == 0) копироватьToolStripMenuItem.Text = "Копировать";
            else копироватьToolStripMenuItem.Text = "复制";

            if (key == 0) вставитьToolStripMenuItem.Text = "Вставить";
            else вставитьToolStripMenuItem.Text = "插入";

            if (key == 0) удалитьToolStripMenuItem.Text = "Удалить";
            else удалитьToolStripMenuItem.Text = "删除";

            if (key == 0) выделитьВсеToolStripMenuItem.Text = "Выделить все";
            else выделитьВсеToolStripMenuItem.Text = "选择全部";

            if (key == 0) постановкаЗадачиToolStripMenuItem.Text = "Постановка задачи";
            else постановкаЗадачиToolStripMenuItem.Text = "问题陈述";

            if (key == 0) грамматикаToolStripMenuItem.Text = "Грамматика";
            else грамматикаToolStripMenuItem.Text = "语法";

            if (key == 0) классификацияГарамматикиToolStripMenuItem.Text = "Классификация грамматики";
            else классификацияГарамматикиToolStripMenuItem.Text = "语法分类";

            if (key == 0) методАнализаToolStripMenuItem.Text = "Метод анализа";
            else методАнализаToolStripMenuItem.Text = "分析方法";

            if (key == 0) тестовыйПримерToolStripMenuItem.Text = "Тестовый пример";
            else тестовыйПримерToolStripMenuItem.Text = "测试用例";

            if (key == 0) списокЛитературыToolStripMenuItem.Text = "Список литературы";
            else списокЛитературыToolStripMenuItem.Text = "参考";

            if (key == 0) исходныйКодПрограммыToolStripMenuItem.Text = "Исходный код программы";
            else исходныйКодПрограммыToolStripMenuItem.Text = "程序源码";

            if (key == 0) вызовСправкиToolStripMenuItem.Text = "Вызов справки";
            else вызовСправкиToolStripMenuItem.Text = "寻求帮助";

            if (key == 0) оПрограммеToolStripMenuItem.Text = "О программе";
            else оПрограммеToolStripMenuItem.Text = "关于该计划";

            if (key == 0) error_grid.Columns[0].HeaderText = "Путь файла";
            else error_grid.Columns[0].HeaderText = "文件路径";

            if (key == 0) error_grid.Columns[1].HeaderText = "Строка";
            else error_grid.Columns[1].HeaderText = "线";

            if (key == 0) error_grid.Columns[2].HeaderText = "Колонка";
            else error_grid.Columns[2].HeaderText = "柱子";

            if (key == 0) error_grid.Columns[3].Name = "Сообщение";
            else error_grid.Columns[3].HeaderText = "信息";

            if (key == 0) this.Text= "Языковой процессор";
            else this.Text = "语言处理器";

            if (key == 0) label1.Text = "Путь";
            else label1.Text = "小路";
        }
        // Обработка подсчета номеров строк
        private void edit_box_TextChanged(object sender, EventArgs e)
        {
            RichTextBox text_box = sender as RichTextBox;
            TabPage tabpage = text_box.Parent as TabPage;
            object numstr_obj= tabpage.GetChildAtPoint(new Point(4,4));
            RichTextBox num_str = numstr_obj as RichTextBox;

            if (curr_row != text_box.GetLineFromCharIndex(text_box.GetFirstCharIndexOfCurrentLine()) || curr_row == 0)
            {
                num_str.Text = "";
                int num = 0;
                int cur_lin = text_box.GetLineFromCharIndex(text_box.GetCharIndexFromPosition(new Point(1, 1)));
                int prev_line = Convert.ToInt32(num_str.GetLineFromCharIndex(num_str.GetCharIndexFromPosition(new Point(1,1)))-1);
                if (cur_lin != prev_line && text_box.Lines.Length > 20) 
                {
                    num = cur_lin;
                }
                foreach (string line in text_box.Lines)
                {
                    num++;
                    num_str.Text += num + "\n";
                    if (num_str.Lines.Length > 35) break;
                }
                curr_row = text_box.GetLineFromCharIndex(text_box.GetFirstCharIndexOfCurrentLine());
            }
            
            
        }
        private void backfillkeywords(object sender, int pos, int len)
        {
            RichTextBox text_box = sender as RichTextBox;
            text_box.SelectionStart = pos;
            text_box.SelectionLength = len;
            text_box.SelectionBackColor = Color.Yellow;
            text_box.SelectionStart = pos + len;
            text_box.SelectionLength = 0;
            text_box.SelectionBackColor = Color.White;
        }
        // Горячие клавиши
        private void edit_box_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                сохранитьToolStripMenuItem.PerformClick();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.N)
            {
                new_file_but.PerformClick();
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.F5)
            {
                start_but.PerformClick();
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.F10)
            {
                help_but.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        // Подсветка ключевых слов
        private void edit_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            RichTextBox text_box = sender as RichTextBox;
            if (e.KeyChar == '\r' || e.KeyChar == ' ' || e.KeyChar == '\0' || e.KeyChar == (char)Keys.Back)
            {
                int curr_pos=0;
                if (e.KeyChar == '\r') curr_row--;
                switch (word.ToLower())
                {
                    case "if":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box,curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "else":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box,curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "int":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box,curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "func":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box, curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "float":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box, curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "while":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box, curr_pos, word.Length);
                        e.Handled = true;
                        break;
                    case "for":
                        curr_pos = text_box.GetFirstCharIndexFromLine(curr_row) + text_box.Lines[curr_row].IndexOf(word);
                        backfillkeywords(text_box, curr_pos, word.Length);
                        e.Handled = true;
                        break;
                }
                word = "";
            }
            else word += e.KeyChar;
        }
        // Строка состояний Таймер
        private void timer1_Tick(object sender, EventArgs e)
        {
            time += 1;
            toolStripStatusLabel2.Text = "Время работы с приложением: " + time + "cек.";
        }
        // Перетаскивание файлов 
        private void Lang_procc_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Lang_procc_DragDrop(object sender, DragEventArgs e)
        {
            this.Cursor = Cursors.Default;
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            openFileDialog1.FileName = file[0];
            openFileDialog1.ShowDialog();

        }
        private void error_grid_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                object numstr_obj = programs.TabPages[programs.TabPages.Count - 1].GetChildAtPoint(new Point(50, 50));
                RichTextBox edit_box = numstr_obj as RichTextBox;
                edit_box.Focus();
                string pos = error_grid.Rows[e.RowIndex].Cells[3].Value.ToString();
                string row_numstr = pos.Split(' ')[1];
                row_numstr = row_numstr.Split(',')[0];
                int row_num = Convert.ToInt32(row_numstr);
                string inline = pos.Split(' ')[2];
                int positioninstr = Convert.ToInt32(inline.Split('-')[0]);
                edit_box.Select(edit_box.GetFirstCharIndexFromLine(row_num - 1) + positioninstr - 1, 1);
            }
        }
    }
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
    }
}

