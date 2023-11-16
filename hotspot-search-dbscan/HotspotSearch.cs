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
            var C = DBSCAN();
            DrawClusters(C);
        }

        List<List<Point>> DBSCAN()
        {
            var clusters = new List<List<Point>>();
            var I = new List<Point>(); //processed points

            foreach (var p in Points) //végigmegyünk az összes beolvasott ponton
            {
                if (!I.Contains(p)) //még nem lett feldolgozva
                {
                    var Q = GetNeighbors(p); //kiszedjük az összes szomszédját p-nek
                    if (Q.Count >= minPts) //p belső pont is
                    {
                        var R = new List<Point>(); //aktuális klaszter elemei
                        foreach (var q in Q) //végigmegyünk a Q-ba kiszedettelemeken is
                        {
                            if (!I.Contains(q)) //ha még nem lett feldolgozva a pont
                            {
                                I.Add(q);
                                q.SetProcessToTrue(); //ez lehet nem fog kelleni
                                R.Add(q);
                                var D = GetNeighbors(q);
                                if (D.Count >= minPts)
                                {
                                    foreach (var d in D)
                                    {
                                        Q.Add(d);
                                    }
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
                if (AreNeighbors(point, choosenPoint))
                {
                    neighbors.Add(point);
                }
            }

            return neighbors;
        }

        bool AreNeighbors(Point p1, Point p2)
        {
            //ide kéne az egyenlet Evelintől
            // p1-től p2 max epsilon távolságra lévő elem (hogy számolom ki, Pytagoras?)
            return false;
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
            //majd kirajzolni Console-ra -> egy for-ral végigmenni és az alapján baszakodni a színekkel
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
        public Color Color { get; set; }
        public bool isProcessed { get; set; }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.isProcessed = false;
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
                    this.Color = Color.Red;
                    break;
                case 1:
                    this.Color = Color.DarkGreen;
                    break;
                case 2:
                    this.Color = Color.Cyan;
                    break;
                case 3:
                    this.Color = Color.DarkMagenta;
                    break;
                case 4:
                    this.Color = Color.Blue;
                    break;
                case 5:
                    this.Color = Color.Yellow;
                    break;
                case 6:
                    this.Color = Color.Green;
                    break;
                default:
                    this.Color = Color.White;
                    break;
            }

        }
    }
}
