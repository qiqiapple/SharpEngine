using System;
using System.IO;
using GLFW;
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
        private static Triangle triangle = new Triangle(
            new Vertex[]{
                new Vertex(new Vector(-0.1f, -0.1f), Color.Red),
                new Vertex(new Vector(0.1f, -0.1f), Color.Green),
                new Vertex(new Vector(0f, 0.1f), Color.Blue)
            }
        );
        
        private static Triangle triangle2 = new Triangle(
            new Vertex[]{
                new Vertex(new Vector(0.4f, 0.4f), Color.Red),
                new Vertex(new Vector(0.6f, 0.4f), Color.Green),
                new Vertex(new Vector(0.5f, 0.6f), Color.Blue)
            }
        );

        private static float multiplier = 0.999f;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            CreateShaderProgram();
            
            // engine rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents();
                ClearScreen();
                Render(window);
                Play();
            }
        }

        private static void Play()
        {
            //ChangeDirection();
            triangle.Bounce();
            triangle2.Bounce();
            triangle.Move();
            triangle2.Move();
            triangle.Rotate();
            triangle2.Rotate();
            if (triangle.CurrentScale >= 1f) multiplier = 0.999f;
            if (triangle.CurrentScale <= 0.5f) multiplier = 1.001f;
            triangle.Scale(multiplier);
            triangle2.Scale(multiplier);
        }

        private static void Render(Window window)
        {
            triangle.Render();
            triangle2.Render();
            Glfw.SwapBuffers(window);
        }

        private static void ClearScreen()
        {
            glClearColor(0.2f, 0.05f, 0.2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        private static Window CreateWindow()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}