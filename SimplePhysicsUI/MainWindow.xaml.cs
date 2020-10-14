using SimplePhysics;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimplePhysicsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer FollowMouseTimer { get; set; }
        private PhysicsLogic Logic;
        private List<WpfCircle> entities;
        private bool IsMouseHold;

        public MainWindow()
        {
            InitializeComponent();

            //PreviewMouseLeftButtonDown += MouseHold;

            PreviewMouseDown += HoldMouseEventHandler;
            PreviewMouseUp += ReleaseMouseEventHandler;

            var c = new WpfCircle(100, InvokeDispatcher, GetDoubleFromUI);
            //var c1 = new WpfCircle(100, InvokeDispatcher, GetDoubleFromUI);

            Logic = new PhysicsLogic(GetScreenWidth, GetScreenHeight, c);
            Cnvs.Children.Add(c.ellipse);

            entities = new List<WpfCircle>()
            {
                c
            };

            FollowMouseTimer = new DispatcherTimer(DispatcherPriority.Send);
            FollowMouseTimer.Tick += Timer_Tick;
            FollowMouseTimer.Interval = TimeSpan.FromMilliseconds(16);

            Logic.TimerSwitch = true;
            //Logic.LoadSpaceSettings();

            Logic.Draw += Draw;
        }

        private WpfCircle selectedShape;
        private void ReleaseMouseEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseHold)
            {
                Logic.TimerSwitch = true;
                Logic.StopDrag(selectedShape);
                FollowMouseTimer.Stop();
                IsMouseHold = false;
                selectedShape = null;
            }
        }
        private void HoldMouseEventHandler(object sender, MouseButtonEventArgs e)
        {
            foreach (var s in entities)
            {
                if (s.ellipse.IsMouseOver)
                {
                    IsMouseHold = true;
                    Logic.TimerSwitch = false;
                    FollowMouseTimer.Start();
                    selectedShape = s;
                    return;
                }
            }

            
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Point p = Mouse.GetPosition(Cnvs);
            double x, y;
            x = p.X;
            y = p.Y;
            Logic.FollowMouse(x, y, selectedShape);
        }
        public void InvokeDispatcher(Action a)
        {
            try
            {
                Dispatcher.Invoke(() => a.Invoke());
            }
            catch (Exception)
            {

            }
        }
        public double GetDoubleFromUI(Func<double> a)
        {
            try
            {
                double res = Dispatcher.Invoke(a);
                return res;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        private double GetScreenWidth()
        {
            return Cnvs.ActualWidth;
        }
        private double GetScreenHeight()
        {
            return Cnvs.ActualHeight;
        }
        public void Draw()
        {
            foreach (var item in entities)
            {
                
            }
        }
    }


}
