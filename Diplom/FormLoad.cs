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
    public partial class FormLoad : Form
    {
        public FormLoad()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        public string[,] numbersmas;
        public string[,] numbersmas2;
        public bool flag=false;
        public int linlen, razm = 0;
        public string path="";

        public void LoadFile()
        {
            bool flag_razd = false;
            textBox2.Text = "";
            button3.Visible = false;
            string r = Path.GetExtension(dialog.FileName);
            if (textBox1.Text == "")
            {
                textBox3.Text = textBox3.Text + "Выберите файл" + Environment.NewLine;
            }
            else if (r != ".txt")
            {
                textBox3.Text = textBox3.Text + "Неверный формат файла" + Environment.NewLine;
                textBox2.Text = "";
            }
            else
            {
                string[] lines = File.ReadAllLines(dialog.FileName);
                linlen = lines.Length;
                bool error1 = false, error2 = false;//1-формат 2-размерность
                double nak = 0, n = 100.0 / lines.Length;
                char razdelitel = ' ';
                int i = 0, j;
                int i2 = 0, j2 = 0;
                string[] numbers;
                numbersmas = new string[50,50];
                numbersmas2 = new string[50, 50];
                textBox2.Text = "";
                progressBar1.Value = 0;
                if (lines.Length > 0 && n < 1) progressBar1.Increment(1);
                for (i = 0; i < 50;i++ )
                {
                    for (j = 0; j < 50; j++)
                    {
                        numbersmas[i, j] = "";
                        numbersmas2[i, j] = "";
                    }
                }
                i = 0;
                foreach (string line in lines)
                    {
                        j = 0;
                        if (n < 1)
                        {
                            nak += n;
                            if (nak >= 1)
                            {
                                progressBar1.Increment(1);
                                nak = nak % 1;
                            }
                        }
                        else
                        {
                            nak += n;
                            progressBar1.Increment(Convert.ToInt32(nak / 1));
                            nak = nak % 1;
                        }
                        textBox2.Text = textBox2.Text + line + Environment.NewLine;
                        numbers = line.Split(razdelitel);
                        //try
                        //{
                        if (line == "--------------------------------") flag_razd = true;
                        if (flag_razd == false)
                        {
                            foreach (string s in numbers)
                            {
                                //textBox2.Text = textBox2.Text + s + " ";
                                numbersmas[i, j] = s;
                                j++;
                            }
                        }
                        else if (line != "--------------------------------")
                        {
                            j2=0;
                            foreach (string s in numbers)
                            {
                                numbersmas2[i2, j2] = s;
                                j2++;
                            }
                            i2++;
                        }
                        //}
                        //catch (FormatException) { error1 = true; break; }
                        // if (i == 0) razm = j;//Запоминаю начальную размерность
                        // if (razm != j) { error2 = true; break; }
                        i++;
                    }
                if (error1 == true) { textBox3.Text = textBox3.Text + "Неверный формат данных в файле (симв.)" + Environment.NewLine; }
               // else if (error2 == true) { textBox3.Text = textBox3.Text + "Неверный формат данных в файле (размерность)" + Environment.NewLine;}
                else if (progressBar1.Value >= 100) { textBox3.Text = textBox3.Text + "Файл \"" + Path.GetFileName(dialog.FileName) + "\" успешно загружен" + Environment.NewLine; button3.Visible = true; path = dialog.FileName; }
                else if (progressBar1.Value == 0) { textBox3.Text = textBox3.Text + "Файл \"" + Path.GetFileName(dialog.FileName) + "\" успешно загружен(пустой)" + Environment.NewLine; button3.Visible = true; path = dialog.FileName; }
                else { textBox3.Text = textBox3.Text + "Ошибка загрузки файла" + Environment.NewLine; }
             }
        }

        OpenFileDialog dialog = new OpenFileDialog();
        private void button2_Click(object sender, EventArgs e)
        {
            dialog.ShowDialog();
            textBox1.Text = dialog.FileName;
            button3.Visible = false;
        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }

        private void FormLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

    }
}
