using SimplePhysics.Logic;
using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;

namespace SimplePhysics
{
    public class PhysicsLogic
    {
        //todo: add configurions.
        //todo: fix on x axis valocity get reduced.
        //todo: fix objects never reach 0 velocity
        //todo: Continue The Libary!! add new shapes, logic, entitiy collisions!!!

        /// <summary>
        /// Represent the gravity, this value will be added to a falling object or will slow going upwards objects.
        /// </summary>
        public double Gravity { get; set; }

        /// <summary>
        /// Represent the power of an external force on the objects
        /// this power determinte the velocity gained from external forces.
        /// for example: YVelocity = distance * power.
        /// </summary>
        public float Power { get; set; }

        private System.Timers.Timer Timer;

        private bool timerSwitch;
        /// <summary>
        /// Turns the timer that drives the code on and off.
        /// </summary>
        public bool TimerSwitch
        {
            get => timerSwitch;
            set
            {
                timerSwitch = value;
                if (timerSwitch)
                {
                    Timer.Start();
                }
                else
                {
                    Timer.Stop();
                }
            }
        }

        /// <summary>
        /// A List of all the shapes that are invole in the physics logic.
        /// </summary>
        public List<PhysicsShape> Shapes { get; set; }
        private Collision Collision;

        /// <summary>
        /// Creates a new instance of PhysicsLogic with absolute values.
        /// </summary>
        /// <param name="borderWidth">Border width</param>
        /// <param name="borderHeight">Border Height</param>
        /// <param name="shapes">Shapes</param>
        public PhysicsLogic(double borderWidth, double borderHeight, params PhysicsCircle[] shapes)
        {
            Init(shapes);
            Collision.BorderWidthAbsolute = borderWidth;
            Collision.BorderHeightAbsolute = borderHeight;
        }

        /// <summary>
        /// Creates a new instance of PhysicsLogic with dynamic values.
        /// </summary>
        /// <param name="screenWidthFunc">Func that returns the current width</param>
        /// <param name="screenHeightFunc">Func that returns the current height</param>
        /// <param name="shapes">Shapes</param>
        public PhysicsLogic(Func<double> screenWidthFunc, Func<double> screenHeightFunc, params PhysicsShape[] shapes)
        {
            Init(shapes);
            Collision.BorderWidthFunc = screenWidthFunc;
            Collision.BorderHeightFunc = screenHeightFunc;
        }

        /// <summary>
        /// initialize with the defualt values.
        /// </summary>
        /// <param name="shapes"></param>
        public void Init(PhysicsShape[] shapes)
        {
            Collision = new Collision();
            Timer = CreateTimer();
            Shapes = new List<PhysicsShape>(shapes);
            LoadDefualtSettings();
        }
        public void LoadDefualtSettings()
        {
            Gravity = 1;
            Collision.Friction = 0.3f;
            Power = 0.3f;
        }
        public void LoadSpaceSettings()
        {
            Gravity = 0.01;
            Collision.Friction = 0.4f;
            Power = 0.1f;
        }
        private System.Timers.Timer CreateTimer()
        {
            var timer = new System.Timers.Timer
            {
                Interval = 16
            };
            timer.Elapsed += MainTick;
            return timer;
        }
        /// <summary>
        /// Represent the logic tick(60 ticks per second),
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTick(object sender, ElapsedEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                foreach (var shape in Shapes)
                {
                    CalcGravityEffect(shape);
                    CalcVelocity(shape);
                    Collision.CollisionEffect(shape);
                }
            }
            Debug.WriteLine($"{Debugger.IsAttached}");
        }

        /// <summary>
        /// Calc the effect the velocity have on an object.
        /// </summary>
        /// <param name="shape">Shape</param>
        private void CalcVelocity(PhysicsShape shape)
        {
            var point = shape.CenterPoint;
            point.Y += shape.Velocity.YVelocity;
            point.X += shape.Velocity.XVelocity;
            shape.SetCenterPoint(point);
        }
        /// <summary>
        /// Calc the gravity on an object.
        /// </summary>
        /// <param name="shape"></param>
        private void CalcGravityEffect(PhysicsShape shape)
        {
            if(!Collision.IsCollideWithBottomBorder(shape, Collision.ScreenHeight))
            {
                shape.Velocity.YVelocity += Gravity;
            }
        }

        public void FollowMouse(double x, double y, PhysicsShape shape)
        {
            Point p = new Point(x, y);
            shape.SetCenterPoint(p);
            shape.HistoryPosition.AddHistory(p);
            CalcExternalVelocity(shape);
        }
        private void CalcExternalVelocity(PhysicsShape shape)
        {
            //todo: maybe get better logic.
            Velocity velocity = shape.HistoryPosition.GetVelocityGained();
            velocity.XVelocity *= Power;
            velocity.YVelocity *= Power;
            shape.Velocity = velocity;
        }
        public void StopDrag(PhysicsShape shape)
        {
            shape.HistoryPosition.Clear();
        }
    }
}
