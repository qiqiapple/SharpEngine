using System;
using System.IO;
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
        private static Triangle triangle = new Triangle(0.2f, 0.2f, new Vector(0, 0));
        //private static Triangle triangle2 = new Triangle(0.2f, 0.2f, new Vector(-0.2f, -0.3f));
        private static Rectangle rectangle = new Rectangle(0.3f, 0.2f, new Vector(-0.15f, -0.1f));
        private static Circle circle = new Circle(0.1f, new Vector(0.1f, -0.2f));
        private static Cone cone = new Cone(0.1f, new Vector(-0.1f, -0.2f));

        private static float multiplier_tri = 0.999f;
        private static float multiplier_rec = 0.999f;
        private static float multiplier_cir = 0.999f;
        private static float multiplier_con = 0.999f;
        private static float ratio;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            int ratioLocation = CreateShaderProgram();

            // engine rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                ChangeWindowSize(window, ratioLocation);
                Glfw.PollEvents();
                ClearScreen();
                Render(window);
                Play();
            }
        }

        private static void Play()
        {
            TrianglePlay();
            //rectangle.Rotate();
            RectanglePlay();
            CirclePlay();
            ConePlay();
        }

        private static void TrianglePlay()
        {
            triangle.Bounce(ratio);
            triangle.Move();
            triangle.Rotate();
            // triangle2.Bounce();
            // triangle2.Move();
            // triangle2.Rotate();
            if (triangle.CurrentScale >= 1f) multiplier_tri = 0.999f;
            if (triangle.CurrentScale <= 0.5f) multiplier_tri = 1.001f;
            triangle.Scale(multiplier_tri);
            //triangle2.Scale(multiplier_tri);
        }

        private static void RectanglePlay()
        {
            rectangle.Bounce(ratio);
            rectangle.Move();
            rectangle.Rotate();
            if (rectangle.CurrentScale >= 1.3f) multiplier_rec = 0.999f;
            if (rectangle.CurrentScale <= 0.3f) multiplier_rec = 1.001f;
            rectangle.Scale(multiplier_rec);
        }

        private static void CirclePlay()
        {
            circle.Bounce(ratio);
            circle.Move();
            circle.Rotate();
            if (circle.CurrentScale >= 0.9f) multiplier_cir = 0.999f;
            if (circle.CurrentScale <= 0.4f) multiplier_cir = 1.001f;
            circle.Scale(multiplier_cir);
        }

        private static void ConePlay()
        {
            cone.Bounce(ratio);
            cone.Move();
            cone.Rotate();
            if (cone.CurrentScale >= 0.7f) multiplier_con = 0.999f;
            if (cone.CurrentScale <= 0.3f) multiplier_con = 1.001f;
            cone.Scale(multiplier_con);
        }
        
        private static void ChangeWindowSize(Window window, int ratioLocation)
        {
            Glfw.GetWindowSize(window, out int width, out int height);
            glViewport(0, 0, width, height);
            ratio = (float) width / height;
            glUniform1f(ratioLocation, ratio);
        }


        private static void Render(Window window)
        {
            triangle.Render();
            //triangle2.Render();
            rectangle.Render();
            circle.Render();
            cone.Render();
            Glfw.SwapBuffers(window);
        }

        private static void ClearScreen()
        {
            glClearColor(0.2f, 0.05f, 0.2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static int CreateShaderProgram()
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

            int ratioLocation = glGetUniformLocation(program, "aspectRatio");
            return ratioLocation;
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