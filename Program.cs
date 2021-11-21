using System;
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
            var material = new Material("shaders/projection-view-model-color.vert", "shaders/vertex-color.frag");
            var camera = new Camera();
            camera.Transform.Position = new Vector(0f, 0f, -1f);
            var scene = new Scene(camera);
            var physics = new Physics(scene);
            window.Load(scene);

            for (int i = 0; i < 800; i++)
            {
                var radius = 0.1f * GetRandomFloat(random, 0.2f, 0.3f);
                var circle = new Circle(radius, material);
                circle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1f));
                circle.velocity = (-0.9f) * GetRandomFloat(random, 0.15f, 0.3f) * circle.Transform.Position.Normalize();
                circle.Mass = MathF.PI * radius * radius;
                scene.Add(circle);
            }
            
            for (int i = 0; i < 20; i++)
            {
                var width = 0.2f * GetRandomFloat(random, 0.2f, 0.3f);
                var height = 0.2f * GetRandomFloat(random, 0.3f, 0.4f);
                var rectangle = new Rectangle(width, height, material);
                rectangle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1f));
                rectangle.Mass = rectangle.width * rectangle.height;
                scene.Add(rectangle);
            }

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
                    camera.CameraControl(window, fixedDeltaTime);
                    previousFixedStep += fixedDeltaTime;
                    physics.Update(fixedDeltaTime);
                }
                window.Render();
            }
        }
    }
}