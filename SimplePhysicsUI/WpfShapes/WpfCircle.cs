using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SimplePhysicsUI
{
    public class WpfCircle : PhysicsCircle
    {
        public Ellipse Ellipse { get; private set; }

        public WpfCircle(double radius)
        {
            CreateEllipse(radius);
        }

        private void CreateEllipse(double radius)
        {
            Ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2
            };
            Canvas.SetLeft(Ellipse, 0);
            Canvas.SetTop(Ellipse, 0);
            Radius = radius;
            Ellipse.Fill = System.Windows.Media.Brushes.Red;
            SetCenterPoint(new SimplePhysics.Models.Point(300, 50));
        }
        public override double Radius { get; set; }
        public double XLoc { get; private set; }
        public double YLoc { get; private set; }
        public override SimplePhysics.Models.Point CenterPoint
        {
            get
            {
                double x, y;
                x = XLoc + Radius;
                y = YLoc + Radius;
                return new SimplePhysics.Models.Point(x, y);
            }
            protected set
            {
                XLoc = value.X - Radius;
                YLoc = value.Y - Radius;
            }
        } 
    }
}
