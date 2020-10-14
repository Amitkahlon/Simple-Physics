using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePhysics.Logic
{
    public class HistoryPosition
    {
        private const int Capacity = 5;
        public PhysicsShape Shape { get; set; }
        public List<Point> HistoryOfLoc { get; private set; }

        public HistoryPosition()
        {
            HistoryOfLoc = new List<Point>(Capacity);
        }

        public void AddHistory(Point p)
        {
            if (HistoryOfLoc.Count < Capacity)
            {
                HistoryOfLoc.Add(p);
            }
            else
            {
                HistoryOfLoc.RemoveAt(0);
                HistoryOfLoc.Add(p);
            }
        }

        public double GetDistanceTravel()
        {
            var a = HistoryOfLoc[0];
            var b = HistoryOfLoc.Last();

            double dis = Point.GetDistance(a, b);

            return dis;
        }

        public Velocity GetVelocityGained()
        {
            Point a, b;
            a = HistoryOfLoc[0];
            b = HistoryOfLoc.Last();

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
            HistoryOfLoc.Clear();
        }
    }

}
