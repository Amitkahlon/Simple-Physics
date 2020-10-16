using SimplePhysics.Logic;
using SimplePhysics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePhysics.Shapes
{
    public abstract class PhysicsShape
    {
        public abstract Point CenterPoint { get; protected set; }
        public abstract Point TopVertex { get; set; }
        public abstract Point LeftVertex { get; set; }
        public abstract Point RightVertex { get; set; }
        public abstract Point BottomVertex { get; set; }

        public Velocity Velocity { get; set; }
        public void SetCenterPoint(Point p)
        {
            CenterPoint = p;
            CircleMoveEvent?.Invoke(this, new ShapeMoveEventArgs(p));
            //Debug.WriteLine($"{p.X}, {p.Y}");
        }
        public abstract bool IsCollidedWithX(double x);
        public abstract bool IsCollidedWithY(double y);

        public PhysicsShape()
        {
            Velocity = new Velocity();
            HistoryPosition = new PreviousPosition();
        }
        public PreviousPosition HistoryPosition { get; private set; }

        public delegate void ShapeMove(object sender, ShapeMoveEventArgs e);
        public event ShapeMove CircleMoveEvent;
    }

    public class ShapeMoveEventArgs : EventArgs
    {
        public Point Point { get; set; }

        public ShapeMoveEventArgs(Point p)
        {
            Point = p;
        }
    }
}
