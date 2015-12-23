using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Diplom
{
    public partial class FormCreate : Form
    {
        public FormCreate()
        {
            InitializeComponent();
        }

        private void FormCreate_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void FormCreate_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label3.Text = "";
        }

        private void FormCreate_Activated(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label3.Text = "";
        }
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        private void button2_Click(object sender, EventArgs e)
        {
            dialog.ShowDialog();
            textBox1.Text = dialog.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs ea)
        {
            string r = dialog.SelectedPath;
            string path = dialog.SelectedPath+"\\"+textBox2.Text;

            try
            {
                if (Directory.Exists(path))
                {
                    label3.Text ="Такой проект уже создан";
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(path);
                label3.Text = "Директория была успешно создана: " + Directory.GetCreationTime(path) + ".";
                FileStream fs = File.Create(path + "\\" + textBox2.Text+".txt");

            }
            catch (Exception e)
            {
                label3.Text = "Ошибка:" + e.ToString() + ".";
            }
            finally { }
        }
    }
}
