using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace фапра
{
    public partial class Lang_procc : Form
    {
        public Lang_procc()
        {
            InitializeComponent();
        }
        private void ctrla(object sender, EventArgs e)
        {
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
        private void newfile(object sender, EventArgs e)
        {
            programs.TabPages.Add(new TabPage($"Новый документ {programs.TabCount+1}"));
        }
        private void open_file(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            programs.TabPages.Add(new TabPage(openFileDialog1.FileName));
            programs.SelectedTab = programs.TabPages[programs.TabPages.Count - 1];    

        }
        private void exit(object sender, EventArgs e)
        {
            this.Close();
        }
        private void save_as(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }
        private void save(object sender, EventArgs e)
        {
            programs.TabPages.Add(new TabPage($"Новый документ {programs.TabCount + 1}"));
        }
        private void dexter1(object sender, FormClosedEventArgs e)
        {
            dexter.Visible = false;
        }
        private void about(object sender, EventArgs e)
        {
            Form about = new Form();
            about.StartPosition = FormStartPosition.CenterScreen;
            about.FormClosed += dexter1;
            TextBox te = new TextBox();
            te.ReadOnly = true;
            te.Multiline = true;
            te.Dock = DockStyle.Fill;
            te.Text = "Программа - языковой процессор!"+Environment.NewLine+"Выполнил: Плахин Даниил АП-327" 
                + Environment.NewLine + "Антонянц Егор Николаевич асс. каф. АСУ.";
            dexter.Visible = true;
            about.Controls.Add(te);
            about.Show();
        }

    }
}

