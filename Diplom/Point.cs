using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Diplom
{
    public struct AddPoint
    {
        public int x, y;
        public List<int> L ;
        public AddPoint(int px, int py,List<int> Li)
        {
            x = px;
            y = py;
            L = Li;
        }
    }

    public class Point
    {
        public Dictionary<int, AddPoint> Node = new Dictionary<int, AddPoint>(100);
        public double[,] w_f = new double[25,4];
        public int num = 0; //num-кол-во узлов

        public Point()
        {
            for(int i=0;i<25;i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    
                    if (j == 2) { w_f[i, j] = -1; }
                    else
                    if (j == 3) { w_f[i, j] = 0.01; }
                    else
                    { w_f[i, j] = 0; }
                }
            }
        }


        public void Add_To_Dict(int x,int y,int d) //Добавляем точку в словарь
        {
            List<int> list = new List<int>(25);
            AddPoint P = new AddPoint(x, y, list);
            num++;
            Node.Add(num, P);    //Добавляем в словарь
        }

        public void Save_in_wf(int P1, int P2) //Добавляем ребро в массив w_f
        {
            for (int i = 0; i < 25; i++)
            {
                if (w_f[i, 0] == 0)
                {
                    if (P1 < P2)
                    {
                        w_f[i, 0] = P1;
                        w_f[i, 1] = P2;
                        break;
                    }
                    else
                    {
                        w_f[i, 0] = P2;
                        w_f[i, 1] = P1;
                        break;
                    }
                }
                
            }
        }

        public void Add_To_Dict_L(int num2,int x, int y, int d) //Добавляем точку в словарь при загрузке с файла
        {
            List<int> list = new List<int>(25);
            AddPoint P = new AddPoint(x, y, list);
            Node.Add(num2, P);    //Добавляем в словарь
        }

        public bool ThePointIs(int x, int y,int d)//Есть ли здесь точка(узел)
        {
            bool The_point_is = false;
            foreach (var nd in Node)
            {
                if (Math.Sqrt(Math.Pow((x - nd.Value.x), 2) + Math.Pow((y - nd.Value.y), 2)) <= d && num != 0) The_point_is = true;
            }
            return The_point_is;
        }

        public int Point_num_1, Point_num_2;
        public void Save_cord(int x, int y, int d, int Point_num)//Сохраняем координаты
        {
            if (Point_num == 1)//первой
            {
                foreach (var nd in Node)
                {
                    if (Math.Sqrt(Math.Pow((x - nd.Value.x), 2) + Math.Pow((y - nd.Value.y), 2)) <= d && num != 0) Point_num_1 = nd.Key;
                }
            }
            if (Point_num == 2)//второй
            {
                foreach (var nd in Node)
                {
                    if (Math.Sqrt(Math.Pow((x - nd.Value.x), 2) + Math.Pow((y - nd.Value.y), 2)) <= d && num != 0) Point_num_2 = nd.Key;
                }
            }

        }

        public void Add_edge()//Добавляем ребро к точкам
        {
                int P1, P2;
                P1 = Point_num_1;
                P2 = Point_num_2;
                List<int> list = new List<int>(50);
                AddPoint K = new AddPoint();
                K=Node[P1];
                list = K.L;
                list.Add(P2);
                K.L = list;
                Node[P1]=K;
                K = Node[P2];
                list = K.L;
                list.Add(P1);
                K.L = list;
                Node[P2] = K;
                Save_in_wf(P1,P2);

        }

        public void Add_edge(int P1,int P2)//Добавляем ребро к точкам при загрузке
        {
            List<int> list = new List<int>(50);
            AddPoint K = new AddPoint();
            K = Node[P1];
            list = K.L;
            list.Add(P2);
            K.L = list;
            Node[P1] = K;
        }

        public void Del_node()
        {
            Node.Remove(Point_num_1);
            foreach (var nd in Node)
            {
                nd.Value.L.Remove(Point_num_1);
            }
            for (int i = 0; i < 25; i++)
            {
                if (w_f[i, 0] == Point_num_1 || w_f[i, 1] == Point_num_1)
                {
                    w_f[i, 0] = 0;
                    w_f[i, 1] = 0;
                    w_f[i, 2] = -1;
                    w_f[i, 3] = 0;
                }

            }
        }

        public void P_moving(int x,int y,int d)
        {
            int P = Point_num_1;
            AddPoint K = new AddPoint();
            K = Node[P];
            K.x = x;
            K.y = y;
            Node[P] = K;
        }

        public bool ThePointIs_mov(int x, int y, int d)//Есть ли здесь точка(узел)
        {

            foreach (var nd in Node)
            {
                if (nd.Key != Point_num_1)
                {
                    if (Math.Sqrt(Math.Pow((x - nd.Value.x), 2) + Math.Pow((y - nd.Value.y), 2)) <= d && num != 0) return true;
                }
            }
            return false;
        }

        public int Edge_num_1, Edge_num_2;
        public bool TheLineIs(int x, int y, int d)
        {
            d = d * 3;
            foreach (var nd in Node)
            {
                foreach(var l in nd.Value.L)
                {
                    if ((x - nd.Value.x) * ((Node[l].y - d) - (nd.Value.y - d)) - (Node[l].x - nd.Value.x) * (y - (nd.Value.y - d)) >= 0 && (x - nd.Value.x) * ((Node[l].y + d) - (nd.Value.y + d)) - (Node[l].x - nd.Value.x) * (y - (nd.Value.y + d)) <= 0 && x <= nd.Value.x && x >= Node[l].x)
                    {
                        Edge_num_1 = nd.Key;
                        Edge_num_2 = l;
                        return true;
                    }
                }
                
            }
            return false;
        }

        public void Del_edge()
        {
            Node[Edge_num_1].L.Remove(Edge_num_2);
            Node[Edge_num_2].L.Remove(Edge_num_1);
            for (int i = 0; i < 25; i++)
            {
                if (w_f[i, 0] == Edge_num_1 && w_f[i, 1] == Edge_num_2 || w_f[i, 1] == Edge_num_1 && w_f[i, 0] == Edge_num_2)
                {
                    w_f[i, 0] = 0;
                    w_f[i, 1] = 0;
                    w_f[i, 2] = -1;
                    w_f[i, 3] = 0;
                    break;
                }

            }
        }

        public void Change_w(int w)//изменить вес
        {
            for (int i = 0; i < 25; i++)
            {
                if (w_f[i, 0] == Edge_num_1 && w_f[i, 1] == Edge_num_2 || w_f[i, 1] == Edge_num_1 && w_f[i, 0] == Edge_num_2)
                {
                    w_f[i, 2] = w; 
                }

            }
        }
        public void Save_Project(string path)
        {
            string edges;
            FileStream fs = File.Create(path);
            fs.Close();
            foreach (var n in Node)
            {
                edges = "";
                foreach (int edge in n.Value.L)
                    {
                        edges=edges+edge+" ";
                    }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(n.Key + " " + n.Value.x + " " + n.Value.y + " " + edges);
                }
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("--------------------------------");
                for (int i = 0; i < 25; i++)
                {
                    if (w_f[i, 0] != 0)
                    {
                        sw.WriteLine(w_f[i, 0] + " " + w_f[i, 1] + " " + w_f[i, 2] + " ");
                    }
                }
            }
        }
    }
}
