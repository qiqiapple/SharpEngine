using System;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        // private static Triangle triangle = new Triangle(
        // new Vertex[]{
        //         new Vertex(new Vector(0f, 0f), Color.Red),
        //         new Vertex(new Vector(1f, 0f), Color.Green),
        //         new Vertex(new Vector(0f, 1f), Color.Blue)
        //     }
        // );
        // private static Triangle triangle = new Triangle(
        //     new Vertex[]{
        //         new Vertex(new Vector(-0.1f, -0.1f), Color.Red),
        //         new Vertex(new Vector(0.1f, -0.1f), Color.Green),
        //         new Vertex(new Vector(0f, 0.1f), Color.Blue)
        //     }
        // );
        //
        // private static Triangle triangle2 = new Triangle(
        //     new Vertex[]{
        //         new Vertex(new Vector(0.4f, 0.4f), Color.Red),
        //         new Vertex(new Vector(0.6f, 0.4f), Color.Green),
        //         new Vertex(new Vector(0.5f, 0.6f), Color.Blue)
        //     }
        // );
        // private static Triangle triangle = new Triangle(0.2f, 0.2f, new Vector(0, 0));
        
        // private static Cone cone = new Cone(0.1f, new Vector(-0.1f, -0.2f));

        // private static float multiplier_tri = 0.999f;
        // private static float multiplier_rec = 0.999f;
        // private static float multiplier_cir = 0.999f;
        // private static float multiplier_con = 0.999f;
        // private static float ratio;

        static void Main(string[] args)
        {
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);
            var newTriangle = new Triangle(new Vertex[]
            {
                new Vertex(new Vector(-0.1f, 0f), Color.Red),
                new Vertex(new Vector(0.1f, 0f), Color.Green),
                new Vertex(new Vector(0f, 0.133f), Color.Blue)
            }, material);
            
            scene.Add(newTriangle);
            // var newRectangle = new Rectangle(0.3f, 0.2f, new Vector(0f, -0.3f), material);
            // scene.Add(newRectangle);
            // var newCircle = new Circle(0.1f, new Vector(0.2f, 0.4f), material);
            // scene.Add(newCircle);
            // var newCone = new Cone(0.1f, new Vector(-0.2f, -0.4f), material);
            // scene.Add(newCone);

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);
            
            var multiplier = 0.999f;
            var ratioLocation = material.ratioLocation;
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;

            // engine rendering loop
            while (window.IsOpen())
            {
                if (Glfw.Time > previousFixedStep + fixedDeltaTime)
                {
                    previousFixedStep = Glfw.Time;
                    var walkDirection = new Vector();
                    // for (int i = 0; i < scene.shapes.Count; i++)
                    // {
                    //     var shape = scene.shapes[i];
                    //     if (shape.Transform.CurrentScale.GetMagnitude() >= 1f) multiplier = 0.95f;
                    //     if (shape.Transform.CurrentScale.GetMagnitude() <= 0.5f) multiplier = 1.05f;
                    //     //shape.Transform.Scale(multiplier);
                    //     //shape.Transform.Move(shape.direction);
                    //     //shape.Transform.Rotate(0.05f);
                    //     shape.Bounce(window.WindowAspectRatio);
                    //     //shape.Bounce(1f);
                    // }
                    if (window.GetKey(Keys.W))
                    {
                        walkDirection += new Vector(0, 1);
                    }
                    if (window.GetKey(Keys.S))
                    {
                        walkDirection += new Vector(0, -1);
                    }
                    if (window.GetKey(Keys.A))
                    {
                        walkDirection += new Vector(-1, 0);
                    }
                    if (window.GetKey(Keys.D))
                    {
                        walkDirection += new Vector(1, 0);
                    }
                    walkDirection = walkDirection.Normalize();
                    newTriangle.Transform.Position += walkDirection * movementSpeed * fixedDeltaTime;
                    glUniform1f(ratioLocation, window.WindowAspectRatio);
                }
                window.Render();
            }
        }
    }
}