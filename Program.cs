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
        //     -0.1f, -0.1f, 0f,
        //     // vertex 2 x, y, z
        //     0.1f, -0.1f, 0f,
        //     // vertex 3 x, y, z
        //     0f, 0.1f, 0f,
        //     
        //     //Add another triangle
        //     0.4f, 0.4f, 0f,
        //     0.6f, 0.4f, 0f,
        //     0.5f, 0.6f, 0f
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
        
        const int vertexX = 0;
        const int vertexY = 1;
        const int vertexSize = 3;

        const float center1X = 0;
        const float center1Y = -0.033f;
        const float center2X = 0.5f;
        const float center2Y = 0.467f;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer();
            CreateShaderProgram();

            float angle = 0f;

            // engine rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents();
                ClearScreen();
                Render();
                
                // angle += 0.01f;
                // RotateMethod1(angle); // method 1
                
                RotateMethod2(); // method 2
                //ScaleUp();

                UpdateTriangleBuffer();
            }
        }

        private static void RotateMethod2()
        {
            float angle = 0.001f;
            float tmp = vertices[0];
            vertices[0] = (float) (vertices[0] * Math.Cos(angle) + vertices[1] * Math.Sin(angle));
            vertices[1] = (float) (vertices[1] * Math.Cos(angle) - tmp * Math.Sin(angle));

            tmp = vertices[3];
            vertices[3] = (float) (vertices[3] * Math.Cos(angle) + vertices[4] * Math.Sin(angle));
            vertices[4] = (float) (vertices[4] * Math.Cos(angle) - tmp * Math.Sin(angle));

            tmp = vertices[6];
            vertices[6] = (float) (vertices[6] * Math.Cos(angle) + vertices[7] * Math.Sin(angle));
            vertices[7] = (float) (vertices[7] * Math.Cos(angle) - tmp * Math.Sin(angle));
        }

        private static void RotateMethod1(float angle)
        {
            float r = 0.707f;
            
            vertices[6] = (float) (0.5 * Math.Sin(angle));
            vertices[7] = (float) (0.5 * Math.Cos(angle));

            vertices[3] = (float) (r * Math.Sin(angle + Math.PI * 135 / 180));
            vertices[4] = (float) (r * Math.Cos(angle + Math.PI * 135 / 180));

            vertices[0] = (float) (r * Math.Sin(angle - Math.PI * 135 / 180));
            vertices[1] = (float) (r * Math.Cos(angle - Math.PI * 135 / 180));

            vertices[3] = (float) (0.5 * Math.Sin(angle + Math.PI * 120 / 180));
            vertices[4] = (float) (0.5 * Math.Cos(angle + Math.PI * 120 / 180));

            vertices[0] = (float) (0.5 * Math.Sin(angle - Math.PI * 120 / 180));
            vertices[1] = (float) (0.5 * Math.Cos(angle - Math.PI * 120 / 180));
        }

        private static void ScaleUp()
        {
            for (int i = vertexX; i < vertices.Length/2; i+=3)
            {
                vertices[i] += 0.001f * Math.Sign(vertices[i] - center1X);
            }
            for (int i = vertexY; i < vertices.Length/2; i+=3)
            {
                vertices[i] += 0.001f * Math.Sign(vertices[i] - center1Y);
            }
            for (int i = vertices.Length/2; i < vertices.Length; i+=3)
            {
                vertices[i] += 0.001f * Math.Sign(vertices[i] - center2X);
            }
            for (int i = vertices.Length/2+1; i < vertices.Length; i+=3)
            {
                vertices[i] += 0.001f * Math.Sign(vertices[i] - center2Y);
            }
        }
        private static void Stretch()
        {
            vertices[1] -= 0.0001f;
            vertices[4] -= 0.0001f;
            vertices[7] += 0.0001f;
        }

        private static void Shrink()
        {
            for (int i = vertexX; i < vertices.Length; i++)
            {
                vertices[i] *= 0.999f;
            }
        }
        private static void MoveDown()
        {
            for (int i = vertexY; i < vertices.Length; i += vertexSize)
            {
                vertices[i] -= 0.0001f;
            }
        }
        private static void MoveRight()
        {
            for (int i = vertexX; i < vertices.Length; i += vertexSize)
            {
                vertices[i] += 0.0001f;
            }
        }
        private static void Render()
        {
            glDrawArrays(GL_TRIANGLES, 0, 2*vertices.Length/vertexSize);
            glFlush();
        }

        private static void ClearScreen()
        {
            glClearColor(0.2f, 0.05f, 0.2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/green.frag"));
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
            glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
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


