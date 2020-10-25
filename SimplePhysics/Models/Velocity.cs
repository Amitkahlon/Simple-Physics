namespace SimplePhysics.Models
{
    public class Velocity
    {
        //Represent The Velocity In the X axis
        //+Velocity: right, -Velocity: left
        
        public double X { get; set; }

        //Represent The Velocity In the Y axis
        //+Velocity: down, -Velocity: up
        public double y { get; set; }

        public Velocity()
        {

        }

        public Velocity(double x, double y)
        {
            this.X = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{X}, {y}";
        }
    }
}
