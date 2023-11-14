using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polygon_problem_hill_climbing
{
    public class SmallestBoundaryPolygonProblem
    {
        List<Point> P;
        List<Point> Points;
        double p_fitness;
        double last_fitness;
        double errorMargin;
        double t;
        double epsilon;

        public SmallestBoundaryPolygonProblem(double epsilon, double t, double errorMargin)
        {
            this.Points = new List<Point>();
            this.P = new List<Point>();
            this.epsilon = epsilon;
            this.t = t;
            this.errorMargin = errorMargin;
            this.p_fitness = double.MaxValue;
            this.last_fitness = double.MaxValue;
        }

        public void RunHC_Stachostic(string filename)
        {
            LoadPointsFromFile(filename);
            Optimize(filename);
        }

        void Optimize(string filename)
        {
            t = 1;
            while (!StopCondition())
            {
                List<Point> q = RandomNeighbour();

                double q_fitness = Objective(q);
                if (q_fitness < p_fitness)
                {
                    P = q.ToList();
                    last_fitness = p_fitness;
                    p_fitness = q_fitness;
                    SavePointsToFile(P);
                    Console.WriteLine($"Fitness: {q_fitness}");
                }
                t++;
            }
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

        public void SavePointsToFile(List<Point> pointVector)
        {
            using (StreamWriter writer = new StreamWriter("log-" + epsilon + "-" + errorMargin, true))
            {
                foreach (Point pnt in pointVector)
                {
                    writer.WriteLine($"{pnt.x}\t{pnt.y}");
                }
            }
        }

        List<Point> RandomNeighbour()
        {
            List<Point> randomNeighborPoints = new List<Point>();

            foreach (Point p in P)
            {
                double randomXModification = RNG.GenerateRandomDoubleWithBounds(-epsilon, epsilon);
                double randomYModification = RNG.GenerateRandomDoubleWithBounds(-epsilon, epsilon);

                Point newPoint = new Point(p.x + randomXModification, p.y + randomYModification);
                randomNeighborPoints.Add(newPoint);
            }

            return randomNeighborPoints;
        }

        bool StopCondition()
        {
            if ((last_fitness - p_fitness) / last_fitness <= errorMargin)
            {
                t--;
            }
            return (t <= 0);
        }

        double DistanceFromLine(Point lp1, Point lp2, Point p)
        {
            // https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
            return ((lp2.y - lp1.y) * p.x - (lp2.x - lp1.x) * p.y + lp2.x * lp1.y - lp2.y * lp1.x) / Math.Sqrt(Math.Pow(lp2.y - lp1.y, 2) + Math.Pow(lp2.x - lp1.x, 2));
        }

        double OuterDistanceToBoundary(List<Point> solution)
        {
            double sum_min_distances = 0;

            for (int pi = 0; pi < Points.Count; pi++)
            {
                double min_dist = double.MaxValue;
                for (int li = 0; li < solution.Count; li++)
                {
                    double act_dist = DistanceFromLine(solution[li], solution[(li + 1) % solution.Count], Points[pi]);
                    if (li == 0 || act_dist < min_dist)
                        min_dist = act_dist;
                }
                if (min_dist < 0)
                    sum_min_distances += -min_dist;
            }
            return sum_min_distances;
        }

        double LengthOfBoundary(List<Point> solution)
        {
            double sum_length = 0;

            for (int li = 0; li < solution.Count - 1; li++)
            {
                Point p1 = solution[li];
                Point p2 = solution[(li + 1) % solution.Count];
                sum_length += Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
            }
            return sum_length;
        }

        double Objective(List<Point> solution)
        {
            return LengthOfBoundary(solution);
        }

        double Constraint(List<Point> solution)
        {
            return -OuterDistanceToBoundary(solution);
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
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
