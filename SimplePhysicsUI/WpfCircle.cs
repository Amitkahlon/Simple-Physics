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
        private Action<Action> UpdateUi;
        private Func<Func<double>, double> GetDoubleFromUI;
        public Ellipse ellipse { get; private set; }

        public WpfCircle(double radius, Action<Action> updateUi, Func<Func<double>, double> getDoubleFromUi)
        {
            ellipse = new Ellipse();
            Canvas.SetTop(ellipse, 100);
            Canvas.SetLeft(ellipse, 100);

            UpdateUi = updateUi;
            this.GetDoubleFromUI = getDoubleFromUi;
            Radius = radius;
            ellipse.Fill = System.Windows.Media.Brushes.Red;
        }


        //Radius of the circle
        public override double Radius { get => GetDoubleFromUI.Invoke(() => ellipse.Width / 2); set { UpdateUi.Invoke(() => ellipse.Width = value); ellipse.Height = value; } }

        //represent the x location from the top left of the circle
        private double XLoc
        {
            get 
            {
                return GetDoubleFromUI(() => Canvas.GetLeft(ellipse));
                 
            }
            set
            {
                UpdateUi.Invoke(() => Canvas.SetLeft(ellipse, value));
            }
        }

        //represent the y location from the top left of the circle
        private double YLoc
        {
            get
            {
                return GetDoubleFromUI(() => Canvas.GetTop(ellipse));
            }
            set
            {
                UpdateUi.Invoke(() => Canvas.SetTop(ellipse, value));
            }
        }

        //represent the center point
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
