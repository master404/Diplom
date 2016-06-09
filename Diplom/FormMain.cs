﻿using System;
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
            if (fl.flag == true)
            {
                textBox1.Text = "";
                int i, j;
                for (i = 0; i < 50; i++ )
                {
                    for (j = 0; j < 50; j++)
                    {
                        textBox1.Text = textBox1.Text + fl.numbersmas[i,j] + " ";
                    }
                    textBox1.Text = textBox1.Text + Environment.NewLine; 
                }
                fl.flag = false;
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
                        }
                        else
                        { label1.Text = "Неверно выбран узел"; }
                    }
                    else if (Point.ThePointIs(e.X, e.Y, d))
                    {
                        Point.Save_cord(e.X, e.Y, d / 2, 1);
                        p_moving = true;
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
            foreach (var nd in Point.Node)
            {
                e.Graphics.DrawImage(img, nd.Value.x - d / 2, nd.Value.y - d / 2, d, d);
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
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            p_moving = false;
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
            string pathname = fc.path + "\\" + fc.name;
            Point.Save_Project(pathname);   
        }

        private void удалитьРеброToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point.Del_edge();
            pictureBox1.Invalidate();
        }
     }
}
