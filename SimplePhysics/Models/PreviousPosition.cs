using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace SimplePhysics.Logic
{
    public class PreviousPosition
    {
        private const int Capacity = 5;
        public PhysicsShape Shape { get; set; }
        public List<Point> History { get; private set; }

        public PreviousPosition()
        {
            History = new List<Point>(Capacity);
        }

        public void AddHistory(Point p)
        {
            if (History.Count < Capacity)
            {
                History.Add(p);
            }
            else
            {
                History.RemoveAt(0);
                History.Add(p);
            }
        }

        public double GetDistanceTravel()
        {
            var a = History[0];
            var b = History.Last();

            double dis = Point.GetDistance(a, b);

            return dis;
        }

        public Velocity GetVelocityGained()
        {
            Point a, b;
            a = History[0];
            b = History.Last();

            double xDis = b.X - a.X;
            double yDis = b.Y - a.Y;
            return new Velocity()
            {
                XVelocity = xDis,
                YVelocity = yDis
            };

        }

        internal void Clear()
        {
            History.Clear();
        }
    }

}
