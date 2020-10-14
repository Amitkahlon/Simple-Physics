using SimplePhysics.Logic;
using SimplePhysics.Models;
using System;
using System.Diagnostics;

namespace SimplePhysics.Shapes
{
    public abstract class PhysicsCircle : PhysicsShape
    {
        //shoudld not use in logic for generic purpse
        public override Point TopVertex
        {
            get
            {
                double y = CenterPoint.Y - Radius;
                return new Point(CenterPoint.X, y);
            }
            set
            {
                var p = new Point(value.X, value.Y + Radius);
                SetCenterPoint(p);
            }
        }
        public override Point LeftVertex
        {
            get
            {
                double x = CenterPoint.X - Radius;
                return new Point(x, CenterPoint.Y);
            }
            set
            {
                var p = new Point(value.X + Radius, value.Y);
                SetCenterPoint(p);
            }

        }
        public override Point RightVertex
        {
            get
            {
                double x = CenterPoint.X + Radius;
                return new Point(x, CenterPoint.Y);
            }
            set
            {
                var p = new Point(value.X - Radius, value.Y);
                SetCenterPoint(p);
            }
        }
        public override Point BottomVertex
        {
            get
            {
                double y = CenterPoint.Y + Radius;
                return new Point(CenterPoint.X, y);
            }
            set
            {
                var p = new Point(value.X, value.Y - Radius);
                SetCenterPoint(p);
            }
        }
        public abstract double Radius { get; set; }
        public double Diameter { get => Radius + Radius; }
        public override bool IsCollidedWithX(double x)
        {
            var l = LeftVertex;
            var r = RightVertex;

            if (l.X <= x && r.X >= x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool IsCollidedWithY(double y)
        {
            var t = TopVertex;
            var b = BottomVertex;

            if (t.Y <= y && b.Y >= y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public PhysicsCircle() : base()
        {

        }
    }
}
