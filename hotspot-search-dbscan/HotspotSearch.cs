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
            ;
            var C = DBSCAN();
            ;
            DrawClusters(C);
        }

        List<List<Point>> DBSCAN()
        {
            var clusters = new List<List<Point>>();
            var I = new List<Point>(); //processed points

            foreach (var p in Points) //végigmegyünk az összes beolvasott ponton
            {
                ;
                if (/*!I.Contains(p)*/ p.isProcessed == false) //még nem lett feldolgozva
                {
                    var Q = GetNeighbors(p); //kiszedjük az összes szomszédját p-nek
                    ;
                    if (Q.Count >= minPts) //p belső pont is
                    {
                        var R = new List<Point>(); //aktuális klaszter elemei
                        foreach (var q in Q) //végigmegyünk a Q-ba kiszedett elemeken is
                        {
                            ;
                            if (/*!I.Contains(q)*/ q.isProcessed == false) //ha még nem lett feldolgozva a pont
                            {
                                //I.Add(q);
                                q.SetProcessToTrue();
                                R.Add(q);
                                var D = GetNeighbors(q);
                                ;
                                if (D.Count >= minPts)
                                {
                                    foreach (var d in D)
                                    {
                                        Q.Add(d);
                                    }
                                    ;
                                }
                            }
                        }
                        clusters.Add(R); //hozzáadjuk a clusterek-hez az elkészült R-t
                    }
                }
            }

            return clusters;
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

        void DrawClusters(List<List<Point>> clusters)
        {
            for (int i = 0; i < clusters.Count; i++) //végigmegyünk a klasztereken egyesével
            {
                foreach (var point in clusters[i]) //majd a klaszter összes pontján
                {
                    point.SetColor(i); //és beállítjuk a különböző klasztereken belül a pontokat a megfelelő színükre
                }
            }

            //konzol magassága és szélességének beállítása
            double maxX = clusters.SelectMany(cluster => cluster.Select(point => point.x)).Max();
            double maxY = clusters.SelectMany(cluster => cluster.Select(point => point.y)).Max();

            Console.WindowWidth = Math.Min(Console.LargestWindowWidth, (int)(maxX + 2));
            Console.WindowHeight = Math.Min(Console.LargestWindowHeight, (int)(maxY + 2));

            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            Console.Clear();

            //klaszterek kirajzolása)

            foreach (var cluster in clusters)
            {
                foreach (var point in cluster)
                {
                    Console.SetCursorPosition((int)point.x, (int)point.y);
                    Console.ForegroundColor = point.Color;
                    Console.Write("*");
                }
            }

            Console.ReadKey();
        }
    }

    public static class RNG
    {
        static Random rnd;

        public static double GenerateRandomDoubleWithBounds(double lowerBound, double upperBound)
        {
            rnd = new Random();
            return rnd.NextDouble() * (upperBound - lowerBound) + lowerBound;
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
        //lehet kell get is?
        public void SetProcessToTrue()
        {
            this.isProcessed = true;
        }

        public void SetColor(int clusterValue)
        {
            switch (clusterValue)
            {
                case 0:
                    this.Color = ConsoleColor.Red;
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
                default:
                    this.Color = ConsoleColor.White;
                    break;
            }

        }
    }
}
