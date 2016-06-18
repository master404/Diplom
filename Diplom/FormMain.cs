using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Проверка Git

namespace Diplom
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        FormCreate fc = new FormCreate();
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            Point.Node.Clear();
            pictureBox1.Invalidate();
            fc.ShowDialog();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        FormLoad fl =new FormLoad();
        private void проектtxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fl.Show();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void FormMain_Activate(object sender, EventArgs e)
        {
            /*
            pictureBox1.BackColor = Color.White;
            pictureBox1.Enabled = true;
            textBox1.BackColor = Color.White;
            textBox1.Enabled = true;
            label2.Text = fc.path + "\\" + fc.name;
            */
            if (fl.flag == true)
            {
                fl.flag = false;
                textBox1.Text = "";
                int i, j;
                Point.Node.Clear();
                for (i = 0; i < 50; i++ )
                {
                   // MessageBox.Show("" + i);
                    if (fl.numbersmas[i, 0] != "")
                    {
                        Point.Add_To_Dict_L(Convert.ToInt32(fl.numbersmas[i, 0]), Convert.ToInt32(fl.numbersmas[i, 1]), Convert.ToInt32(fl.numbersmas[i, 2]), d);
                        Point.num = Convert.ToInt32(fl.numbersmas[i, 0]);
                    }
                    if (fl.numbersmas2[i, 0] != "")
                    {
                        Point.w_f[i, 0] = Convert.ToInt32(fl.numbersmas2[i, 0]);
                        Point.w_f[i, 1] = Convert.ToInt32(fl.numbersmas2[i, 1]);
                        Point.w_f[i, 2] = Convert.ToInt32(fl.numbersmas2[i, 2]);
                    }
                    if (fl.numbersmas[i, 0] == "" && fl.numbersmas2[i, 0] == "") { break; }
                    for (j = 0; j < 50; j++)
                    {
                        if(j>2)
                        {
                            if(fl.numbersmas[i, j]!="")
                            {
                                Point.Add_edge(Convert.ToInt32(fl.numbersmas[i, 0]), Convert.ToInt32(fl.numbersmas[i, j]));
                            }
                        }
                        //textBox1.Text = textBox1.Text + fl.numbersmas[i,j] + " ";
                    
                    }
                    //textBox1.Text = textBox1.Text + Environment.NewLine; 
                }
                pictureBox1.Invalidate();
                output_text();
            }
            if (fc.path != "")
            {
                pictureBox1.BackColor = Color.White;
                pictureBox1.Enabled = true;
                textBox1.BackColor = Color.White;
                textBox1.Enabled = true;
                label2.Text = fc.path+"\\"+fc.name;
            }
            else if (fl.path != "")
            {
                pictureBox1.BackColor = Color.White;
                pictureBox1.Enabled = true;
                textBox1.BackColor = Color.White;
                textBox1.Enabled = true;
                label2.Text = fl.path;
            }


        }

        bool Add_edge = false, p_moving=false;//добавить ребро,флаг перемещения узла
        int d = 48,dl=7;

        Point Point=new Point();
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Text = "";
            p_moving = false;
        //    toolTip1.Hide(pictureBox1);
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    pictureBox1.Invalidate();
                    if (Add_edge)
                    {
                        Add_edge = false;
                        Point.Save_cord(e.X, e.Y, d / 2, 2);
                        if (Point.ThePointIs(e.X, e.Y, d / 2) && Point.Point_num_1 != Point.Point_num_2)
                        {
                            Point.Add_edge();
                            output_text();
                        }
                        else
                        { label1.Text = "Неверно выбран узел"; }
                    }
                    else if (Point.ThePointIs(e.X, e.Y, d))
                    {
                        Point.Save_cord(e.X, e.Y, d / 2, 1);
                        p_moving = true;
                    }
                    else if (Point.TheLineIs(e.X, e.Y, dl))
                    {
                //        toolTip1.SetToolTip(pictureBox1, "Вес ребра:");
                    }
                    else
                    {
                        Point.Add_To_Dict(e.X, e.Y, d);
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (Point.ThePointIs(e.X, e.Y, d / 2))
                    {
                        Point.Save_cord(e.X, e.Y, d / 2, 1);
                        contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
                    }
                    else
                    if (Point.TheLineIs(e.X, e.Y, dl))
                    {
                        contextMenuStrip2.Show(MousePosition, ToolStripDropDownDirection.Right);
                    }

                }
            }
            catch (Exception error)
            {
                label1.Text = "Ошибка:" + error.ToString() + ".";
            }

        }
        Label[] labels = new Label[25];
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            Pen pen = new Pen(Color.FromArgb(100, 0, 255, 0), dl);
            Image img = Properties.Resources.circle_blue__1_;
            foreach (var nd in Point.Node)
            {
                foreach (int s in nd.Value.L)
                {
                    e.Graphics.DrawLine(pen, nd.Value.x, nd.Value.y, Point.Node[s].x, Point.Node[s].y);
                }
            }
            int i;
            for (i = 0; i < 25;i++ )
            {
                if (labels[i] != null)
                {
                    labels[i].Dispose();
                }
            }
            i = 0;
                foreach (var nd in Point.Node)
                {
                    e.Graphics.DrawImage(img, nd.Value.x - d / 2, nd.Value.y - d / 2, d, d);
                    if (!p_moving)
                    {
                        labels[i] = new Label();
                        labels[i].Width = 20;
                        labels[i].Height = 16;
                        labels[i].Location = new System.Drawing.Point(nd.Value.x-8, nd.Value.y-40);
                        labels[i].Text = nd.Key.ToString();
                        pictureBox1.Controls.Add(labels[i]);
                    }
                    i++;
                }
        }

        private void добавитьРеброToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_edge = true;
            label1.Text = "Выберите узел";
        }

        private void удалитьЭлементToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point.Del_node();
            pictureBox1.Invalidate();
            output_text();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            p_moving = false;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (p_moving)
            {
                if (!Point.ThePointIs_mov(e.X, e.Y, d))
                {
                    Point.P_moving(e.X, e.Y,d);
                    pictureBox1.Invalidate();
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathname = "";
            if (fc.path != "")
            {
                pathname = fc.path + "\\" + fc.name;
            }
            else if (fl.path != "")
            {
                pathname = fl.path;
            }
            Point.Save_Project(pathname);
            //label1.Text = "Успешно сохранено";
        }

        private void удалитьРеброToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point.Del_edge();
            pictureBox1.Invalidate();
            output_text();
        }

        private void изменитьВесToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            textBox2.Visible = true;
            button2.Visible = true;
        }

        private void output_text()
        {
            textBox1.Text = "";
                    for (int i = 0; i < 25; i++)
                    {
                        if (Point.w_f[i, 0] != 0)
                        {
                            textBox1.Text = textBox1.Text + "("+Point.w_f[i, 0] + ";" + Point.w_f[i, 1] + ") вес ребра: " + Point.w_f[i, 2];
                            textBox1.Text = textBox1.Text + Environment.NewLine;
                        }

                    }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text!="")
            {
                try
                {
                    int w = Convert.ToInt32(textBox2.Text);
                    Point.Change_w(w);
                    output_text();
                }
                catch { MessageBox.Show("Невереный тип данных"); }

            }
            button1.Focus();
            textBox2.Text = "";
            label3.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
        }

     }
}
