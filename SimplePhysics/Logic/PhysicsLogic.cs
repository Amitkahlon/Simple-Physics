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
    public delegate void Draw();
    public class PhysicsLogic
    {
        public Draw Draw;

        /// <summary>
        /// Represent the gravity, this value will be added to a falling object or will slow going upwards objects.
        /// </summary>
        public double Gravity { get; set; }

        /// <summary>
        /// Represent the amount of speed a object lose when he hit an object, for example XVelocity * 1 - Friction.
        /// </summary>
        public float Friction { get; set; }
        /// <summary>
        /// Represent the power of an external force on the objects
        /// this power determinte the velocity gained from external forces.
        /// for example: YVelocity = distance * power.
        /// </summary>
        public float Power { get; set; }

        /// <summary>
        /// function for receiving the current border width.
        /// </summary>
        private Func<double> BorderWidthFunc;

        /// <summary>
        /// function for receiving the current border height.
        /// </summary>

        private Func<double> BorderHeightFunc;

        /// <summary>
        /// Border width that is not dynamic.
        /// </summary>
        public double BorderWidthAbsolute { get; set; }

        /// <summary>
        /// border height that is not dynamic.
        /// </summary>
        public double BorderHeightAbsolute { get; set; }

        /// <summary>
        /// Returns the current width.
        /// </summary>
        private double ScreenWidth
        {
            get
            {
                if (BorderWidthFunc != null)
                {
                    return BorderWidthFunc();
                }
                else
                {
                    return BorderWidthAbsolute;
                }
            }
        }

        public void StopDrag(PhysicsShape s)
        {
            s.HistoryPosition.Clear();
        }

        /// <summary>
        /// Returns the current height
        /// </summary>
        private double ScreenHeight
        {
            get
            {
                if (BorderHeightFunc != null)
                {
                    return BorderHeightFunc();
                }
                else
                {
                    return BorderHeightAbsolute;
                }
            }

        }

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

        /// <summary>
        /// Creates a new instance of PhysicsLogic with absolute values.
        /// </summary>
        /// <param name="borderWidth">Border width</param>
        /// <param name="borderHeight">Border Height</param>
        /// <param name="shapes">Shapes</param>
        public PhysicsLogic(double borderWidth, double borderHeight, params PhysicsCircle[] shapes)
        {
            Init(shapes);
            BorderHeightAbsolute = borderHeight;
            BorderWidthAbsolute = borderWidth;
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
            this.BorderHeightFunc = screenHeightFunc;
            this.BorderWidthFunc = screenWidthFunc;
        }

        /// <summary>
        /// initialize with the defualt values.
        /// </summary>
        /// <param name="shapes"></param>
        public void Init(PhysicsShape[] shapes)
        {
            LoadDefualtSettings();
            Timer = CreateTimer();
            Shapes = new List<PhysicsShape>(shapes);
        }

        public void LoadDefualtSettings()
        {
            Gravity = 1;
            Friction = 0.3f;
            Power = 0.3f;
        }
        public void LoadSpaceSettings()
        {
            Gravity = 0.01;
            Friction = 0.4f;
            Power = 0.1f;
        }
        private System.Timers.Timer CreateTimer()
        {
            var timer = new System.Timers.Timer
            {
                Interval = 16
            };
            timer.Elapsed += TimerTick;
            return timer;
        }
        /// <summary>
        /// Represent the logic tick(60 ticks per second),
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            foreach (var s in Shapes)
            {
                GravityEffect(s);
                VelocityEffect(s);
                CollisionCheck(s);
                //Draw.Invoke();
            }
        }
        /// <summary>
        /// Caculate the effect the velocity have on an object.
        /// </summary>
        /// <param name="s">Shape</param>
        private void VelocityEffect(PhysicsShape s)
        {
            CalcVelocity();

            void CalcVelocity()
            {
                var p = s.CenterPoint;
                p.Y += s.Velocity.YVelocity;
                p.X += s.Velocity.XVelocity;

                s.SetCenterPoint(p);
            }
        }
        /// <summary>
        /// Represent the effect that gravity have on an object.
        /// </summary>
        /// <param name="s"></param>
        private void GravityEffect(PhysicsShape s)
        {
            CalcVelocity();

            void CalcVelocity()
            {
                s.Velocity.YVelocity += Gravity;
            }
        }
        /// <summary>
        /// function checks if any collision occured.
        /// </summary>
        /// <param name="s"></param>
        private void CollisionCheck(PhysicsShape s)
        {
            //collision tasks.
            //Collision with borders
            //Collision with entities
            CollisionBorders();

            void CollisionBorders()
            {
                //bottom
                if (s.BottomVertex.Y >= ScreenHeight)
                {
                    Collision(s, false);
                    BottomCollision(s);
                }
                //left
                if (s.LeftVertex.X <= 0)
                {
                    Collision(s, true);
                    LeftCollision(s);
                }
                //right
                if (s.RightVertex.X >= ScreenWidth)
                {
                    Collision(s, true);
                    RightCollision(s);
                }
            }
        }

        private void Collision(PhysicsShape s, bool isX)
        {
            if (isX) //if hit a x axis
            {
                s.Velocity.XVelocity *= (float)1 - Friction; //lose velocity
                s.Velocity.XVelocity *= -1; //volocity turns negitive
            }
            else // if hit y axis
            {
                s.Velocity.YVelocity *= (float)1 - Friction; //lose velocity
                s.Velocity.YVelocity *= -1; //volocity turns negitive
            }

            //Debug.WriteLine(s.Velocity.YVelocity);
        }
        private void BottomCollision(PhysicsShape s)
        {
            var Point = new Point(s.CenterPoint.X, ScreenHeight - 1);
            s.BottomVertex = Point;
        }
        private void LeftCollision(PhysicsShape s)
        {
            var Point = new Point(1, s.CenterPoint.Y);
            s.LeftVertex = Point;
        }
        private void RightCollision(PhysicsShape s)
        {
            var Point = new Point(ScreenWidth - 1, s.CenterPoint.Y);
            s.RightVertex = Point;
        }
        private void EntitiyCollision(PhysicsShape s1, PhysicsShape s2)
        {
            //work in progress
            var v1 = s1.Velocity;
            var v2 = s2.Velocity;

            s1.Velocity.XVelocity = v2.XVelocity;
        }
        public void FollowMouse(double x, double y, PhysicsShape s)
        {
            Point p = new Point(x, y);
            s.SetCenterPoint(p);
            s.HistoryPosition.AddHistory(p);
            ExternalForce(s);
        }
        private void ExternalForce(PhysicsShape s)
        {
            //need to write better logic.

            Velocity v = s.HistoryPosition.GetVelocityGained();
            v.XVelocity = v.XVelocity * Power;
            v.YVelocity = v.YVelocity * Power;

            s.Velocity = v;

            //Debug.WriteLine($"{v.XVelocity}, {v.YVelocity}");
        }

        
    }
}
