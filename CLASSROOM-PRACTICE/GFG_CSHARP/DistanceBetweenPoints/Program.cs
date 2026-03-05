using System;

namespace DistanceBetweenPoints
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double CalculateDistance(Point2D other)
        {
            // TODO: Implement distance formula
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Point2D p1 = new Point2D(3, 4);
            Point2D p2 = new Point2D(7, 1);

            double distance = p1.CalculateDistance(p2);

            Console.WriteLine("Distance: " + distance);

            Console.ReadLine();
        }
    }
}