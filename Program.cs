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

        private static Vector velocity = new Vector(0.003f, 0.002f);
        private static Vector velocity2 = new Vector(0.003f, 0.002f);
        private static float multiplier = 0.999f;
        private static Vector center;
        private static Vector center2;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer();
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
            center = triangle.GetCenter();
            center2 = triangle2.GetCenter();
            ChangeDirection();
            triangle.Move(velocity);
            triangle2.Move(velocity2);
            Rotate();
            if (triangle.CurrentScale >= 1f) multiplier = 0.999f;
            if (triangle.CurrentScale <= 0.5f) multiplier = 1.001f;
            triangle.Scale(multiplier);
            triangle2.Scale(multiplier);
        }

        private static void ChangeDirection()
        {
            if (triangle.GetMaxBounds().x >= 1 && velocity.x>0 || triangle.GetMinBounds().x<=-1 && velocity.x<0)
            {
                velocity.x *= -1;
            }
            if (triangle.GetMaxBounds().y >= 1 && velocity.y>0 || triangle.GetMinBounds().y<=-1 && velocity.y<0)
            {
                velocity.y *= -1;
            }
            
            if (triangle2.GetMaxBounds().x >= 1 && velocity2.x>0 || triangle2.GetMinBounds().x<=-1 && velocity2.x<0)
            {
                velocity2.x *= -1;
            }
            if (triangle2.GetMaxBounds().y >= 1 && velocity2.y>0 || triangle2.GetMinBounds().y<=-1 && velocity2.y<0)
            {
                velocity2.y *= -1;
            }
        }
        
        private static void Rotate()
        {
            float angle = 0.003f;
            Vector rotation1 = new Vector(MathF.Cos(angle), MathF.Sin(angle));
            Vector rotation2 = new Vector(-MathF.Sin(angle), MathF.Cos(angle));
            for (int i = 0; i < triangle.vertices.Length; i++)
            {
                Vector vector2 = triangle.vertices[i].position - center;
                triangle.vertices[i].position = new Vector(rotation1 * vector2, rotation2 * vector2) + center;
            }
            for (int i = 0; i < triangle2.vertices.Length; i++)
            {
                Vector vector2 = triangle2.vertices[i].position - center2;
                triangle2.vertices[i].position = new Vector(rotation1 * vector2, rotation2 * vector2) + center2;
            }
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

        private static unsafe void LoadTriangleIntoBuffer()
        {
            // load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            //glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), NULL);
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
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