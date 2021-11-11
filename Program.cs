using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
       private static Vertex[] vertices = new Vertex[]
        {
            new Vertex(new Vector(-0.1f, -0.1f), Color.Red),
            new Vertex(new Vector(0.1f, -0.1f), Color.Green),
            new Vertex(new Vector(0f, 0.1f), Color.Blue),
            new Vertex(new Vector(0.4f, 0.4f), Color.Red),
            new Vertex(new Vector(0.6f, 0.4f), Color.Green),
            new Vertex(new Vector(0.5f, 0.6f), Color.Blue)
            // new Vertex(new Vector(0f, 0f), Color.Red),
            // new Vertex(new Vector(1f,0f), Color.Green),
            // new Vertex(new Vector(0f,1f), Color.Blue)
        };
        
        const int vertexSize = 3;

        private static Vector velocity = new Vector(0.003f, 0.002f);
        private static float multiplier = 0.999f;
        private static float scale = 1f;
        private static Vector center1;
        private static Vector center2;

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
                Render(window);
                
                //MoveRight();
                //Shrink();
                //ScaleUp();
                //GetCenter();
                //angle += 0.01f;
                //RotateMethod1(angle); // method 1
                //RotateMethod2(); // method 2
                Move();
                UpdateTriangleBuffer();
            }
        }

        private static void Move()
        {
            GetCenter();
            ChangeDirection();
            MoveRight();
            MoveUp();
            RotateMethod2();
            if (scale >= 1f) multiplier = 0.999f;
            if (scale <= 0.5f) multiplier = 1.001f;
            ScaleUp();
        }

        private static void GetCenter()
        {
            var min = vertices[0].position;
            var max = vertices[0].position;
            scale *= multiplier;
            for (int i = 0; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
                max = Vector.Max(max, vertices[i].position);
            }
            center1 = (min + max) / 2;
            
            min = vertices[3].position;
            max = vertices[3].position;
            scale *= multiplier;
            for (int i = vertices.Length/2; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
                max = Vector.Max(max, vertices[i].position);
            }
            center2 = (min + max) / 2;
        }

        private static void ChangeDirection()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].position.x>=1 && velocity.x>0 || vertices[i].position.x<=-1 && velocity.x<0)
                {
                    velocity.x *= -1;
                    break;
                }
                if (vertices[i].position.y>=1 && velocity.y>0 || vertices[i].position.y<=-1 && velocity.y<0)
                {
                    velocity.y *= -1;
                    break;
                }
            }
        }

        private static void RotateMethod2()
        {
            float angle = 0.003f;

            Vector vector2 = vertices[0].position - center1;
            vertices[0].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[0].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;
            vector2 = vertices[1].position - center1;
            vertices[1].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[1].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;
            vector2 = vertices[2].position - center1;
            vertices[2].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[2].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;

            vector2 = vertices[3].position - center2;
            vertices[3].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[3].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
            
            vector2 = vertices[4].position - center2;
            vertices[4].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[4].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
            
            vector2 = vertices[5].position - center2;
            vertices[5].position.x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[5].position.y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
        }
        
        private static void RotateMethod1(float angle)
        {
            
            float r = 0.707f;
            
            vertices[2].position.x = (float) (0.5 * Math.Sin(angle));
            vertices[2].position.y = (float) (0.5 * Math.Cos(angle));
        
            vertices[1].position.x = (float) (0.5 * Math.Sin(angle + Math.PI * 120 / 180));
            vertices[1].position.y = (float) (0.5 * Math.Cos(angle + Math.PI * 120 / 180));
        
            vertices[0].position.x = (float) (0.5 * Math.Sin(angle - Math.PI * 120 / 180));
            vertices[0].position.y = (float) (0.5 * Math.Cos(angle - Math.PI * 120 / 180));
        }
        
        private static void ScaleUp()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                //vertices[i].position = (vertices[i].position - center1) * multiplier + center1;
                if (i < vertices.Length/2)
                {
                    vertices[i].position = (vertices[i].position - center1) * multiplier + center1;
                }
                else
                {
                    vertices[i].position = (vertices[i].position - center2) * multiplier + center2;
                }
            }
        }

        private static void MoveUp()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += new Vector(0, velocity.y);
            }
        }
        private static void MoveRight()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += new Vector(velocity.x, 0);
            }
        }
        private static void Render(Window window)
        {
            //glDrawArrays(GL_TRIANGLES, 0, vertices.Length/vertexSize);
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            //glFlush();
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
            UpdateTriangleBuffer();
            //glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), NULL);
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        private static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vertex* vertex = &vertices[0])
            {
                //glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_STATIC_DRAW);
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
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}