using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        // static float[] vertices = new float[]
        // {
        //     // vertex 1 x, y, z
        //     -0.5f, -0.5f, 0f,
        //     // vertex 2 x, y, z
        //     0.5f, -0.5f, 0f,
        //     // vertex 3 x, y, z
        //     0f, 0.5f, 0f
        // };
        
        static float[] vertices = new float[]
        {
            // vertex 1 x, y, z
            -0.433f, -0.25f, 0f,
            // vertex 2 x, y, z
            0.433f, -0.25f, 0f,
            // vertex 3 x, y, z
            0f, 0.5f, 0f
        };

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer();
            CreateShaderProgram();

            //float angle = 0f;
            //float r = (float) (0.5 * Math.Sqrt(2));

            // engine rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents();
                glClearColor(0.2f, 0.05f, 0.2f, 1);
                glClear(GL_COLOR_BUFFER_BIT);
                glDrawArrays(GL_TRIANGLES, 0,3);
                glFlush();
                // Rotate the triangle around its center continuously
                // method 1
                // angle += 0.002f;
                // vertices[6] = (float)(0.5 * Math.Sin(angle));
                // vertices[7] = (float)(0.5 * Math.Cos(angle));
                
                // vertices[3] = (float)(r * Math.Sin(angle + Math.PI * 135 / 180));
                // vertices[4] = (float)(r * Math.Cos(angle + Math.PI * 135 / 180));
                //
                // vertices[0] = (float)(r * Math.Sin(angle - Math.PI * 135 / 180));
                // vertices[1] = (float)(r * Math.Cos(angle - Math.PI * 135 / 180));
                
                // vertices[3] = (float)(0.5 * Math.Sin(angle + Math.PI * 120 / 180));
                // vertices[4] = (float)(0.5 * Math.Cos(angle + Math.PI * 120 / 180));
                //
                // vertices[0] = (float)(0.5 * Math.Sin(angle - Math.PI * 120 / 180));
                // vertices[1] = (float)(0.5 * Math.Cos(angle - Math.PI * 120 / 180));

                // method 2
                float angle = 0.002f;
                float tmp = vertices[0];
                vertices[0] = (float)(vertices[0] * Math.Cos(angle) + vertices[1] * Math.Sin(angle));
                vertices[1] = (float)(vertices[1] * Math.Cos(angle) - tmp * Math.Sin(angle));
                
                tmp = vertices[3];
                vertices[3] = (float)(vertices[3] * Math.Cos(angle) + vertices[4] * Math.Sin(angle));
                vertices[4] = (float)(vertices[4] * Math.Cos(angle) - tmp * Math.Sin(angle));
                
                tmp = vertices[6];
                vertices[6] = (float)(vertices[6] * Math.Cos(angle) + vertices[7] * Math.Sin(angle));
                vertices[7] = (float)(vertices[7] * Math.Cos(angle) - tmp * Math.Sin(angle));

                UpdateTriangleBuffer();
            }
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/red-triangle.frag"));
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
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            glEnableVertexAttribArray(0);
        }

        private static unsafe void UpdateTriangleBuffer()
        {
            fixed (float* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
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
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}


