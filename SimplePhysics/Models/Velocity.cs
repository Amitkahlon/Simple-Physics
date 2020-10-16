namespace SimplePhysics.Models
{
    public class Velocity
    {
        //Represent The Velocity In the X axis
        //+Velocity: right, -Velocity: left
        
        public double XVelocity { get; set; }

        //Represent The Velocity In the Y axis
        //+Velocity: down, -Velocity: up
        public double YVelocity { get; set; }

        public Velocity()
        {

        }

        public Velocity(double x, double y)
        {
            this.XVelocity = x;
            this.YVelocity = y;
        }
    }
}
