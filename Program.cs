using System;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            var physics = new Physics(scene);
            window.Load(scene);

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            ground.Mass = float.PositiveInfinity;
            ground.gravityScale = 0f;
            scene.Add(ground);

            var circle = new Circle(0.1f, Vector.Left, material);
            circle.velocity= Vector.Right * 0.3f;
            scene.Add(circle);
            
            // var rectangle = new Rectangle(material);
            // rectangle.Transform.Position = Vector.Left + Vector.Backward * 0.2f;
            // rectangle.linearForce = Vector.Right * 0.3f;
            // rectangle.Mass = 4f;
            // scene.Add(rectangle);
            
            var circle2 = new Circle(0.1f, Vector.Right * 0.5f, material);
            //circle.velocity= Vector.Right * 0.3f;
            scene.Add(circle2);

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
                    physics.Update(fixedDeltaTime);
                    glUniform1f(ratioLocation, window.WindowAspectRatio);
                }
                window.Render();
            }
        }
    }
}