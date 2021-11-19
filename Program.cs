using System;
using System.Numerics;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static float Lerp(float from, float to, float t)
        {
            return from + (to - from) * t;
        }
        static float GetRandomFloat(Random random, float min=0, float max=1)
        {
            return Lerp(min, max, (float)random.NextDouble());

        }
        static void Main(string[] args)
        {
            var random = new Random();
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            var physics = new Physics(scene);
            var camera = new Camera();
            window.Load(scene);

            // var ground = new Rectangle(material);
            // ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            // ground.Transform.Position = new Vector(0f, -1f);
            // ground.Mass = float.PositiveInfinity;
            // ground.gravityScale = 0f;
            // scene.Add(ground);
            // var rectangle = new Rectangle(material);
            // rectangle.Transform.Position = Vector.Left * 0.2f + Vector.Backward * 0.2f;
            // rectangle.linearForce = Vector.Right * 0.3f;
            // rectangle.Mass = 4f;
            // scene.Add(rectangle);
            
            for (int i = 0; i < 30; i++)
            {
                var radius = 0.1f * GetRandomFloat(random, 0.3f);
                var position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1f));
                var circle = new Circle(radius, position, material);
                circle.velocity = (-0.1f) * GetRandomFloat(random, 0.15f, 0.3f) * position.Normalize();
                circle.Mass = 1f * MathF.PI * radius * radius;
                scene.Add(circle);
            }

            for (int i = 0; i < 5; i++)
            {
                var position = new Vector(GetRandomFloat(random, -0.7f,0.7f), GetRandomFloat(random, -0.7f,0.7f));
                var width = 0.2f * GetRandomFloat(random, 0.3f);
                var height = 0.2f * GetRandomFloat(random, 0.3f);
                var rectangle = new Rectangle(width, height, position, material);
                rectangle.Mass = 4f * rectangle.width * rectangle.height;
                scene.Add(rectangle);
            }


            // var radius = 0.05f;
            // var circle = new Circle(radius, new Vector(-0.2f, 0), material);
            // circle.velocity = new Vector(0.2f,0.05f);
            // circle.Mass = MathF.PI * radius * radius;
            // scene.Add(circle);
            //
            // var position1 = new Vector(0.1f, 0);
            // var rectangle1 = new Rectangle(0.1f, 0.1f, position1, material);
            // rectangle1.Mass = 5f * rectangle1.width * rectangle1.height;
            // scene.Add(rectangle1);
            //
            // var radius2 = 0.1f;
            // var circle2 = new Circle(radius2,  new Vector(0.4f, 0), material);
            // circle2.Mass = MathF.PI * radius2 * radius2;
            // scene.Add(circle2);
        

            var ratioLocation = material.ratioLocation;
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            var previousFixedStep = 0.0;
            const int maxStepsPerFrame = 5;

            while (window.IsOpen())
            {
                var stepCount = 0;
                while (Glfw.Time > previousFixedStep + fixedDeltaTime && stepCount++ < maxStepsPerFrame)
                {
                    //previousFixedStep = Glfw.Time;
                    previousFixedStep += fixedDeltaTime;
                    physics.Update(fixedDeltaTime);
                    glUniform1f(ratioLocation, window.WindowAspectRatio);
                }
                window.Render();
                camera.CameraControl(window, material);
            }
        }
    }
}