using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotspot_search_dbscan
{
    public class HotspotSearch
    {

        List<Point> Points;
        double epsilon;
        double minPts;
        public HotspotSearch(double epsilon, double minPts)
        {
            this.Points = new List<Point>();
            this.epsilon = epsilon;
            this.minPts = minPts;
        }

        public void DoHotspotSearch(string filename)
        {
            LoadPointsFromFile(filename);
            DoDBSCAN();
        }
       
        void DoDBSCAN()
        {
            var clusters = new List<List<Point>>();
            var I = new List<Point>(); //processed points

            foreach (var p in Points) //végigmegyünk az összes beolvasott ponton
            {

                if (!ContainsPoint(I, p)) //még nem lett feldolgozva
                {
                    var Q = GetNeighbors(p); //kiszedjük az összes szomszédját p-nek

                    if (Q.Count >= minPts) //p belső pont is
                    {
                        var R = new List<Point>(); //aktuális klaszter elemei

                        for (int i = 0; i < Q.Count; i++)
                        {
                            if (!ContainsPoint(I, Q[i])) //ha még nem lett feldolgozva a pont
                            {
                                I.Add(Q[i]); //hozzáadjuk a feldolgozott pontokhoz
                                Q[i].SetProcessToTrue();
                                R.Add(Q[i]);
                                var D = GetNeighbors(Q[i]);

                                if (D.Count >= minPts)
                                {
                                    foreach (var d in D)
                                    {
                                        if (!ContainsPoint(Q, d))
                                            Q.Add(d);
                                    }
                                }
                            }
                        }
                        clusters.Add(R); //hozzáadjuk a clusterek-hez az elkészült R-t
                    }
                }
            }
            SaveClustersToFile(clusters);
        }

        bool ContainsPoint(List<Point> list, Point p)
        {
            foreach (var point in list)
            {
                if (point.x.Equals(p.x) && point.y.Equals(p.y) && point.isProcessed.Equals(p.isProcessed))
                    return true;
            }
            return false;
        }

        private List<Point> GetNeighbors(Point choosenPoint)
        {
            var neighbors = new List<Point>();
            foreach (var point in Points)
            {
                if (AreNeighboring(point, choosenPoint))
                {
                    neighbors.Add(point);
                }
            }

            return neighbors;
        }

        bool AreNeighboring(Point p1, Point p2)
        {
            double dist = DistanceFromPoint(p1, p2);
            
            if (dist <= epsilon)
                return true;
            return false;
        }

        double DistanceFromPoint(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow((p2.x - p1.x), 2) + Math.Pow((p2.y - p1.y), 2) );
        }

        void LoadPointsFromFile(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(' ');

                    if (values.Length == 2)
                    {
                        Point pnt = new Point(double.Parse(values[0]), double.Parse(values[1]));
                        Points.Add(pnt);
                    }
                }
            }
        }

        void SaveClustersToFile(List<List<Point>> clusters)
        {
            using (StreamWriter writer = new StreamWriter("log-" + epsilon + "-" + minPts + ".txt", false))
            {
                for (int i = 0; i < clusters.Count; i++) //végigmegyünk a klasztereken egyesével
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    writer.WriteLine($"{i + 1}. cluster: \n");
                    Console.WriteLine($"{i + 1}. cluster: \n");
                    foreach (var point in clusters[i]) //majd a klaszter összes pontján
                    {
                        point.SetColor(i);
                        Console.ForegroundColor = point.Color;
                        writer.WriteLine($"({point.x};{point.y})");
                        Console.WriteLine($"({point.x};{point.y})"); //és kiíratjuk a klasztereket
                    }
                    writer.WriteLine("\n");
                    Console.WriteLine("\n");
                }


                Console.ForegroundColor = ConsoleColor.White;
                writer.WriteLine("Points which does not fit into any cluster: \n");
                Console.WriteLine("Points which does not fit into any cluster: \n");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var p in Points.Where(x => x.Color == ConsoleColor.White)) //kiíratjuk azokat a pontokat, amelyek egyik klaszterbe sem kerültek bele
                {
                    writer.WriteLine($"({p.x};{p.y})");
                    Console.WriteLine($"({p.x};{p.y})");
                }
            }

            Console.ReadLine();
        }
    }

    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public ConsoleColor Color { get; set; }
        public bool isProcessed { get; set; }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.isProcessed = false;
            this.Color = ConsoleColor.White;
        }
        public void SetProcessToTrue()
        {
            this.isProcessed = true;
        }

        public void SetColor(int clusterValue)
        {
            switch (clusterValue)
            {
                case 0:
                    this.Color = ConsoleColor.Magenta;
                    break;
                case 1:
                    this.Color = ConsoleColor.DarkGreen;
                    break;
                case 2:
                    this.Color = ConsoleColor.Cyan;
                    break;
                case 3:
                    this.Color = ConsoleColor.DarkMagenta;
                    break;
                case 4:
                    this.Color = ConsoleColor.Blue;
                    break;
                case 5:
                    this.Color = ConsoleColor.Yellow;
                    break;
                case 6:
                    this.Color = ConsoleColor.Green;
                    break;
                case 7:
                    this.Color = ConsoleColor.DarkCyan;
                    break;
                case 8:
                    this.Color = ConsoleColor.DarkYellow;
                    break;
                case 9:
                    this.Color = ConsoleColor.DarkBlue;
                    break;
                case 10:
                    this.Color = ConsoleColor.DarkGray;
                    break;
                default:
                    this.Color = ConsoleColor.White;
                    break;
            }

        }
    }
}
