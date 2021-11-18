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
            window.Load(scene);

            // var ground = new Rectangle(material);
            // ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            // ground.Transform.Position = new Vector(0f, -1f);
            // ground.Mass = float.PositiveInfinity;
            // ground.gravityScale = 0f;
            // scene.Add(ground);
            // var rectangle = new Rectangle(material);
            // rectangle.Transform.Position = Vector.Left + Vector.Backward * 0.2f;
            // rectangle.linearForce = Vector.Right * 0.3f;
            // rectangle.Mass = 4f;
            // scene.Add(rectangle);
            for (int i = 0; i < 30; i++)
            {
                var radius = GetRandomFloat(random, 0.3f);
                var position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1f));
                var circle = new Circle(position, material);
                circle.Transform.CurrentScale = new Vector(radius,radius,1f);
                circle.velocity= position.Normalize() * (-1) * GetRandomFloat(random, 0.15f, 0.3f);
                circle.Mass = MathF.PI * radius * radius;
                scene.Add(circle);
            }

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
            }
        }
    }
}