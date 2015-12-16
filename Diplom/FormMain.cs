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

namespace Diplom
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        FormLoad f =new FormLoad();
        private void проектtxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void FormMain_Activate(object sender, EventArgs e)
        {
            if (f.flag == true)
            {
                textBox1.Text = "";
                int i, j;
                for (i = 0; i < f.linlen; i++ )
                {
                    for (j = 0; j < f.razm; j++)
                    {
                        textBox1.Text = textBox1.Text + f.numbersmas[i,j] + " ";
                    }
                    textBox1.Text = textBox1.Text + Environment.NewLine; 
                }
            } 
        }

        bool Add_edge = false, p_moving=false;//добавить ребро,флаг перемещения узла
        int d = 48;

        Point Point=new Point();
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Text = "";
            p_moving = false;
            if (e.Button == MouseButtons.Left)
            {
                pictureBox1.Invalidate();
                if (Add_edge)
                {
                    Add_edge = false;
                    Point.Save_cord(e.X, e.Y, d / 2, 2);
                    if (Point.ThePointIs(e.X, e.Y, d / 2) && Point.Point_num_1!=Point.Point_num_2)
                    {
                        Point.Add_edge();
                    }
                    else
                    {label1.Text = "Неверно выбран узел"; }
                }
                else if (Point.ThePointIs(e.X,e.Y,d))
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
                if (Point.ThePointIs(e.X,e.Y,d/2))
                {
                    Point.Save_cord(e.X, e.Y, d / 2,1);
                    contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(100, 0, 255, 0), 7);
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
     }
}
