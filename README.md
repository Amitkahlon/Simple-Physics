

# Simple Physics

A simple project to simulate a 2d physics engine.

Written in a way that any .net project can implement this library(Demo is in WPF).

Written following the rules of SOLID, in a way that is abstract and simplifies developer experience, developers only need to inherit from the library “shapes” and implement a few functions and properties and they are good to go.

- Example is included in the solution.


## Usage/Examples

To use the library you should implement one of the SimplePhysics abstract shapes, for example: 

```cs
using SimplePhysics.Shapes;

    public class WpfCircle : PhysicsCircle
    {
        public override double Radius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override SimplePhysics.Models.Point CenterPoint { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    }
```

For a circle, just implement these two properties, and the library will handle everything for you.
You will just have to handle the rendering on the screen, the class will update its properties during the run, and you can use those properties to render it to the screen. 

You will also have to initiate the `PhysicsLogic` class.
```cs
 Logic = new PhysicsLogic(GetScreenWidth, GetScreenHeight, entities.ToArray());
```
## Demo


[Insert gif or link to demo](https://user-images.githubusercontent.com/50583120/187715683-b3048ac1-4571-4346-ba26-4365be31bf4f.mp4)


## Authors

- [@Amit Kahlon](https://www.github.com/amitkahlon)





