using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_problem_hill_climbing
{
    public class SmallestBoundaryPolygonProblem
    {

        List<Point> Points;
        double fitness;
        double prev_fitness;
        double errorMargin;
        double t;
        double epsilon;

        public SmallestBoundaryPolygonProblem(double epsilon, double t, double errorMargin)
        {
            Points = new List<Point>();
            this.epsilon = epsilon;
            this.t = t;
            this.errorMargin = errorMargin;
            fitness = double.MaxValue;
            prev_fitness = double.MaxValue;
        }

        public void Run(string filename)
        {
            LoadPointsFromFile(filename);
        }

        void LoadPointsFromFile(string filename)
        {

        }

        public void SavePointsToFile(string filename, List<Point> pointVector)
        {

        }

        double DistanceFromLine(Point lp1, Point lp2, Point p)
        {
            // https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
            return ((lp2.y - lp1.y) * p.x - (lp2.x - lp1.x) * p.y + lp2.x * lp1.y - lp2.y * lp1.x) / Math.Sqrt(Math.Pow(lp2.y - lp1.y, 2) + Math.Pow(lp2.x - lp1.x, 2));
        }

        double OuterDistanceToBoundary(List<Point> solution)
        {
            return 0;
        }

        double LengthOfBoundary(List<Point> solution)
        {
            return 0;
        }

    }

    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
