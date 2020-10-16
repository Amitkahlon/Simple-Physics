using SimplePhysics;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private DispatcherTimer FollowMouseTimer;
        private DispatcherTimer RenderUiTimer;
        private PhysicsLogic Logic;
        private List<WpfCircle> entities;
        private WpfCircle selectedShape;
        private bool IsHoldingMouse;


        public MainWindow()
        {
            InitializeComponent();

            SetMouseEvents();
            SetUiElements();
            CreateLogic();
            SetRenderTimer(300);
            SetFollowMouseTimer();

            Logic.TimerSwitch = true;


            void SetMouseEvents()
            {
                PreviewMouseDown += HoldMouseEventHandler;
                PreviewMouseUp += ReleaseMouseEventHandler;
            }
            void SetUiElements()
            {
                var c = new WpfCircle(50);
                Cnvs.Children.Add(c.Ellipse);
                entities = new List<WpfCircle>() { c };
            }
            void CreateLogic()
            {
                Logic = new PhysicsLogic(GetScreenWidth, GetScreenHeight, entities.ToArray());
            }
            void SetFollowMouseTimer()
            {
                FollowMouseTimer = new DispatcherTimer(DispatcherPriority.Send);
                FollowMouseTimer.Tick += FollowMouseTick;
                FollowMouseTimer.Interval = TimeSpan.FromMilliseconds(16);
            }
            void SetRenderTimer(int ticksPerSecond)
            {
                RenderUiTimer = new DispatcherTimer(DispatcherPriority.Send);
                RenderUiTimer.Tick += Draw;
                RenderUiTimer.Interval = TimeSpan.FromMilliseconds(1000 / ticksPerSecond);
                RenderUiTimer.Start();
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            foreach (var entitiy in entities)
            {
                UIElement uiElement = entitiy.Ellipse;
                Canvas.SetLeft(uiElement, entitiy.XLoc);
                Canvas.SetTop(uiElement, entitiy.YLoc);
            }
        }
        private void ReleaseMouseEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (IsHoldingMouse)
            {
                Logic.TimerSwitch = true;
                Logic.StopDrag(selectedShape);
                FollowMouseTimer.Stop();
                IsHoldingMouse = false;
                selectedShape = null;
            }
        }
        private void HoldMouseEventHandler(object sender, MouseButtonEventArgs e)
        {
            foreach (var s in entities)
            {
                if (s.Ellipse.IsMouseOver)
                {
                    IsHoldingMouse = true;
                    Logic.TimerSwitch = false;
                    FollowMouseTimer.Start();
                    selectedShape = s;
                    return;
                }
            }


        }
        private void FollowMouseTick(object sender, EventArgs e)
        {
            Point p = Mouse.GetPosition(Cnvs);
            Logic.FollowMouse(p.X, p.Y, selectedShape);
        }
        private double GetScreenWidth()
        {
            return Cnvs.ActualWidth;
        }
        private double GetScreenHeight()
        {
            return Cnvs.ActualHeight;
        }
    }


}
