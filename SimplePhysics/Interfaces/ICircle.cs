using SimplePhysics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePhysics.Interfaces
{
    public interface ICircle
    {
        double Radius { get; }
        Point CenterPoint { get; }
    }
}
