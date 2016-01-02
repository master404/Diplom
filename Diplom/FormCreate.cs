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
        FolderBrowserDialog dialog = new FolderBrowserDialog();

        private void FormCreate_Load(object sender, EventArgs e)
        {
            dialog.SelectedPath = "D:\\Diplom";
            textBox1.Text = dialog.SelectedPath;
            textBox2.Text = "";
            label3.Text = "";
            path = "";
            r = "";
            name = "";
        }

        private void FormCreate_Activated(object sender, EventArgs e)
        {
            textBox2.Focus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dialog.ShowDialog();
            textBox1.Text = dialog.SelectedPath;
        }

        public string path="", r="",name="";
        public void button1_Click(object sender, EventArgs ea)
        {
            r = dialog.SelectedPath;
            path = dialog.SelectedPath+"\\"+textBox2.Text;
            name = textBox2.Text + ".txt";

            try
            {
                if (textBox2.Text.Substring(0,1) == " " || textBox2.Text == "")
                {
                    label3.Text = "Неверно задано имя проекта";
                }
                else
                    if (Directory.Exists(path))
                    {
                        label3.Text = "Такой проект уже создан";
                        return;
                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                        //label3.Text = "Директория была успешно создана: " + Directory.GetCreationTime(path) + ".";
                        FileStream fs = File.Create(path + "\\" + name);
                        this.Visible = false;
                    }

            }
            catch (Exception e)
            {
                label3.Text = "Ошибка:" + e.ToString() + ".";
            }
            finally { }
        }
    }
}
