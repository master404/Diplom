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
                    }
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
            int i;
            int cof = 0;
            Pen pen;
            Pen pen2 = new Pen(Color.FromArgb(100, 0, 200, 0), dl);
            Image img = Properties.Resources.circle_blue__1_;
            foreach (var nd in Point.Node)
            {
                foreach (int s in nd.Value.L)
                {
                    cof = 0;
                    for(int k=0;k<25;k++)
                    {
                        if(Point.w_f[k,0]==nd.Key)
                        {
                            if (Point.w_f[k, 1]==s)
                            {
                                cof = Convert.ToInt32(Point.w_f[k, 3]*4000);
                                break;
                            }
                        }
                        else
                        if (Point.w_f[k, 1] == nd.Key)
                        {
                            if (Point.w_f[k, 0] == s)
                            {
                                cof = Convert.ToInt32(Point.w_f[k, 3]*4000);
                                break;
                            }
                        }
                    }
                    if (cof > 255) cof = 255;
                    pen = new Pen(Color.FromArgb(100, cof, 255-cof/2, 0), dl);
                    e.Graphics.DrawLine(pen, nd.Value.x, nd.Value.y, Point.Node[s].x, Point.Node[s].y);
                    pen.Dispose();
                }
            }
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
                            textBox1.Text = textBox1.Text + "(" + Point.w_f[i, 0] + ";" + Point.w_f[i, 1] + ") вес ребра: " + Point.w_f[i, 2] + " tau:" + Point.w_f[i, 3];
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
        double[] way = new double[20];
        bool StartFlag = true,StopFlag=false;
        //int h = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            double p = 0, alpha = 0, beta = 0, S = 0, F = 0;
            int nak = 0;
            try
            {
                p = Convert.ToDouble(textBox3.Text);
                alpha = Convert.ToDouble(textBox4.Text);
                beta = Convert.ToDouble(textBox5.Text);
                S = Convert.ToDouble(textBox7.Text);
                F = Convert.ToDouble(textBox8.Text);
            }
            catch { MessageBox.Show("Одно из полей заполнено не корректно"); }
            while (true)
            {
                nak++;
                for (int h = 0; h <20; h++)
                {
                    if(h%10==0)
                    {
                        StopFlag = false;
                        for (int i = 0; i < 25; i++)
                        {
                            if (Point.w_f[i, 0] != 0)
                            { if (Point.w_f[i, 3] > 0.04) { StopFlag = true; break; } }

                        }
                    }
                    if (StopFlag) break;
                    if (h < 8)
                    {
                        StartFlag = true;
                        AntTravel(p, alpha, beta, S, F, StartFlag);
                    }
                    else
                    {
                        StartFlag = false;
                        #region Проход 1 муравья
                        AntTravel(p, alpha, beta, S, F, StartFlag);

                        #endregion
                    }
                   // h++;
                }
                if (StopFlag) break;
                if (nak>30) { MessageBox.Show("Оптимальный путь не найден");break;}
            }
            if(StopFlag)
            {
                for (int j = 0; j < 20;j++ )
                {
                    if (way[j] != -1)
                    {
                        if (j == 0)
                        {
                            label1.Text = "Оптимальный путь: " + way[j];
                        }
                        else
                        {
                            label1.Text = label1.Text + " - " + way[j];
                        }
                    }
                    else break;
                }
            }
        }

        private void AntTravel(double p,double alpha,double beta,double S,double F,bool StartFlag)
        {
            double P = 0, AntLoc = 0,MaxP = 0, PSum = 0, L = 0;
            int nak = 0, ver = 0, ind = 0; 
            bool StompFlag = false, TabuFlag = false;
            Random rnd = new Random();
            nak = 0;//индекс массива пути муравья 
            AntLoc = S; //узел,в котором находится муравей
            // L = 0;//Длина пройденного пути
            for (int i = 0; i < 20; i++)
            {
                way[i] = -1;
            }
            while (true)
            {
                StompFlag = true;//Флаг, не существует доступных путей
                ver = rnd.Next(0, 101); //Куда пойдёт муравей
                PSum = 0;//Сумма для определения куда попадает ver

                for (int i = 0; i < 25; i++)
                {
                    TabuFlag = false;
                    if (Point.w_f[i, 0] == AntLoc)
                    {
                        ind = 1;
                        TabuFlag = Tabu(i, ind);
                        if (TabuFlag == false)
                        {

                            StompFlag = false;
                            P = initP(i, AntLoc, p, alpha, beta,StartFlag);
                            if (PSum <= ver && PSum + P * 100 > ver)
                            {
                                L += Point.w_f[i, 2];
                                MaxP = Point.w_f[i, 1];
                            }
                            PSum += P * 100;
                        }
                    }
                    else if (Point.w_f[i, 1] == AntLoc)
                    {
                        ind = 0;
                        TabuFlag = Tabu(i, ind);
                        if (TabuFlag == false)
                        {

                            StompFlag = false;
                            P = initP(i, AntLoc, p, alpha, beta,StartFlag);
                            if (PSum <= ver && PSum + P * 100 > ver)
                            {
                                L += Point.w_f[i, 2];
                                MaxP = Point.w_f[i, 0];
                            }
                            PSum += P * 100;
                        }
                    }

                }
                way[nak] = AntLoc;
                AntLoc = MaxP;
                nak++;
                if (StompFlag == true) { way[nak] = AntLoc; break; }
                if (AntLoc == F) { way[nak] = AntLoc; break; }
            }
            update_f(p, L);
            pictureBox1.Invalidate();
            output_text();
        }

        private double initP(int i, double AntLoc, double p, double alpha, double beta, bool StartFlag)
        {
            double Tau = 1, n = 1, Sum = 0, P = 0;
            if (StartFlag == false)
            {
            Tau = Point.w_f[i, 3];
             n = 1 / Point.w_f[i, 2]; 
            }
            Sum = sum(AntLoc, p, alpha, beta,StartFlag);
            P = (Math.Pow(Tau, alpha) * Math.Pow(n, beta)) / Sum;
            return P;
        }

        private bool Tabu(int i,int ind)
        {
            bool TabuFlag = false;
            for (int j = 0; j < 20; j++)
            {
                if (way[j] != -1)
                {
                    if (way[j] == Point.w_f[i, ind])
                    {
                        TabuFlag = true;
                        break;
                    }
                }
            }
            return TabuFlag;
        }

        private void update_f(double p,double L)
        {
            double DelTau = 0, Tau = 0,Q=1;
            bool flag = false;
            for (int k = 0; k < 25; k++)
            {
                if (Point.w_f[k, 0]!=0)
                {
                    DelTau = 0;
                    flag = false;
                    for (int j = 0; j < 19;j++ )
                    {
                        if (Point.w_f[k, 0] == way[j] && Point.w_f[k, 1] == way[j + 1] || Point.w_f[k, 1] == way[j] && Point.w_f[k, 0] == way[j + 1])
                        {
                            DelTau = Q / L;
                            Tau = (p) * Point.w_f[k, 3] + DelTau;
                            flag = true;
                            break;
                        }
                    }
                    if (flag == false)
                    {
                        Tau = (1-p) * Point.w_f[k, 3] + DelTau;
                    }
                    Point.w_f[k, 3] = Tau;

                }
            }
        }
        private double sum(double AntLoc, double p, double alpha, double beta, bool StartFlag)
        {
            double  Tau = 1,n=1,Sum=0;
            bool TabuFlag=false;
            for (int j = 0; j < 25; j++)
            {
                n = 1; Tau = 1;
                if (Point.w_f[j, 0] == AntLoc || Point.w_f[j, 1] == AntLoc)
                {
                    TabuFlag = false;
                    for (int k = 0; k < 20; k++)
                    {
                        if (way[k] != -1 && way[k] != AntLoc)
                        {
                            if (Point.w_f[j, 1] == way[k] || Point.w_f[j, 0] == way[k]) { TabuFlag = true; break; }
                        }
                    }
                    if (TabuFlag == false)
                    {
                        if (StartFlag == false) 
                        {
                            Tau = Point.w_f[j, 3];
                            n = 1 / Point.w_f[j, 2]; 
                        }
                        Sum += Math.Pow(Tau, alpha) * Math.Pow(n, beta);
                    }
                }

            }
            return Sum;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                if(Point.w_f[i, 0] != 0)
                { Point.w_f[i, 3] = 0.01; }
                
            }
            output_text();
            pictureBox1.Invalidate();
        }
     }
}
