using SimplePhysics.Interfaces;
using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SimplePhysics.Logic
{
    public class Collision
    {
        /// <summary>
        /// Represent the amount of speed a object lose when he hit an object, for example XVelocity * 1 - Friction.
        /// </summary>
        public float Friction { get; set; }

        /// <summary>
        /// function for receiving the current border width.
        /// </summary>
        public Func<double> BorderWidthFunc { get; set; }

        /// <summary>
        /// function for receiving the current border height.
        /// </summary>

        public Func<double> BorderHeightFunc { get; set; }

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
        public double ScreenWidth
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

        /// <summary>
        /// Returns the current height
        /// </summary>
        public  double ScreenHeight
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


        /// <summary>
        /// function checks if any collision occured and implements all calculations.
        /// </summary>
        /// <param name="shape"></param>
        internal void CollisionEffect(PhysicsShape shape)
        {
            if (IsCollideWithBottomBorder(shape, ScreenHeight))
            {
                Point point = shape.BottomVertex;
                point.Y = ScreenHeight;
                shape.BottomVertex = point;
                SetVelocityBorderHit(shape, false);
            }

            if (IsCollideWithLeftBorder(shape, 0))
            {
                Point point = shape.LeftVertex;
                point.X = 1;
                shape.LeftVertex = point;
                SetVelocityBorderHit(shape, true);
            }

            if(IsCollideWithRightBorder(shape, ScreenWidth))
            {
                Point point = shape.RightVertex;
                point.X = ScreenWidth - 1;
                shape.RightVertex = point;
                SetVelocityBorderHit(shape, true);
            }

        }
        private void SetVelocityBorderHit(PhysicsShape shape, bool isCollideWithX)
        {
            if (isCollideWithX)
            {
                shape.Velocity.X *= (float)1 - Friction;
                shape.Velocity.X *= -1;
            }
            else 
            {
                shape.Velocity.y *= (float)1 - Friction;
                shape.Velocity.y *= -1;
            }
        }
        internal bool IsCollideWithBottomBorder(PhysicsShape shape, double screenHeight)
        {
            return shape.BottomVertex.Y >= screenHeight;
        }
        internal bool IsCollideWithLeftBorder(PhysicsShape shape, double leftScreenX)
        {
            return shape.LeftVertex.X <= leftScreenX;
        }
        internal bool IsCollideWithRightBorder(PhysicsShape shape, double rightScreenX)
        {
            return shape.RightVertex.X >= rightScreenX;
        }
        
        
        public void CircleCollision(PhysicsCircle circle1, PhysicsCircle circle2)
        {
            //todo: implement entitiyCollision
            double xDistance = circle1.CenterPoint.X - circle2.CenterPoint.X;
            double yDistance = circle1.CenterPoint.Y - circle2.CenterPoint.Y;
            double radiusSum = circle1.Radius + circle2.Radius;

            if (xDistance * xDistance + yDistance * yDistance <= radiusSum * radiusSum)
            {
                FixOverlap(circle1, circle2);
                CalcHitVelocity(circle1, circle2);
            }
        }

        private void CalcHitVelocity(PhysicsCircle circle1, PhysicsCircle circle2)
        {
            double distance = circle1.CenterPoint.GetDistance(circle2.CenterPoint);

            // mormal
            double nx = (circle2.CenterPoint.X - circle1.CenterPoint.X) / distance;
            double ny = (circle2.CenterPoint.Y - circle1.CenterPoint.Y) / distance;

            // tangent
            double tx = -ny;
            double ty = nx;

            //dot product tangent
            double dpTan1 = circle1.Velocity.X * tx + circle1.Velocity.y * ty;
            double dpTan2 = circle2.Velocity.X * tx + circle2.Velocity.y * ty;

            dpTan1 *= 1.3;
            dpTan2 *= 1.3;


            circle1.Velocity.X = tx * dpTan1;
            circle1.Velocity.y = ty * dpTan1;
            circle2.Velocity.X = tx * dpTan2;
            circle2.Velocity.y = tx * dpTan2;


        }

        private void FixOverlap(PhysicsCircle circle1, PhysicsCircle circle2)
        {
            double xDistance = circle1.CenterPoint.X - circle2.CenterPoint.X;
            double yDistance = circle1.CenterPoint.Y - circle2.CenterPoint.Y;
            double cDistance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
            double Overlap = 0.5f * (cDistance - circle1.Radius - circle2.Radius);

            //fix pos of first circle
            double newX = circle1.CenterPoint.X - Overlap * (circle1.CenterPoint.X - circle2.CenterPoint.X) / cDistance;
            double newY = circle1.CenterPoint.Y - Overlap * (circle1.CenterPoint.Y - circle2.CenterPoint.Y) / cDistance;
            circle1.SetCenterPoint(newX, newY);

            //fix pos of second circle
            newX = circle2.CenterPoint.X + Overlap * (circle1.CenterPoint.X - circle2.CenterPoint.X) / cDistance;
            newY = circle2.CenterPoint.Y + Overlap * (circle1.CenterPoint.Y - circle2.CenterPoint.Y) / cDistance;
            circle2.SetCenterPoint(newX, newY);
        }

        internal void EntitiyCollision(List<PhysicsShape> entities, PhysicsShape checkedShape)
        {
            foreach (var s in entities)
            {
                if (!s.Equals(checkedShape))
                {
                    if(checkedShape is PhysicsCircle && s is PhysicsCircle)
                    {
                        CircleCollision((PhysicsCircle)checkedShape, (PhysicsCircle)s);
                    }
                }
            }
        }
    }
}
