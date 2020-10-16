using System;

namespace SimplePhysics.Models
{
    public struct Point
    {
        public double Y { get; set; }
        public double X { get; set; }

        public Point(double x, double y)
        {
            Y = y;
            X = x;
        }

        public double GetDistance(Point other)
        {
            return GetDistance(this, other);
        }

        public static double GetDistance(Point p1, Point p2)
        {
            double dis = ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            dis = Math.Sqrt(dis);
            return dis;
        }
    }
}
