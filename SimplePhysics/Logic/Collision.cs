using SimplePhysics.Models;
using SimplePhysics.Shapes;
using System;
using System.Collections.Generic;
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
                shape.Velocity.XVelocity *= (float)1 - Friction;
                shape.Velocity.XVelocity *= -1;
            }
            else 
            {
                shape.Velocity.YVelocity *= (float)1 - Friction;
                shape.Velocity.YVelocity *= -1;
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
        
        
        public void EntitiyCollision(PhysicsShape shape1, PhysicsShape shape2)
        {
            //todo: implement entitiyCollision
            throw new NotImplementedException();
            //work in progress
            var v1 = shape1.Velocity;
            var v2 = shape2.Velocity;

            shape1.Velocity.XVelocity = v2.XVelocity;
        }
    }
}
