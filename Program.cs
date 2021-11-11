using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public static Vector operator +(Vector u, Vector v)
        {
            return new Vector(u.x + v.x, u.y + v.y, u.z + v.z);
        }
        
        public static Vector operator -(Vector u, Vector v)
        {
            return new Vector(u.x - v.x, u.y - v.y, u.z - v.z);
        }

        public static Vector Max(Vector a, Vector b)
        {
            float maxX = Math.Max(a.x, b.x);
            float maxY = Math.Max(a.y, b.y);
            return new Vector(maxX, maxY);
        }

        public static Vector Min(Vector a, Vector b)
        {
            float minX = Math.Min(a.x, b.x);
            float minY = Math.Min(a.y, b.y);
            return new Vector(minX, minY);
        }
    }
    
    class Program
    {
        private static Vector[] vertices = new Vector[]
        {
            new Vector(-0.1f, -0.1f),
            new Vector(0.1f, -0.1f),
            new Vector(0f, 0.1f),
            new Vector(0.4f, 0.4f),
            new Vector(0.6f, 0.4f),
            new Vector(0.5f, 0.6f)
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
                //MoveDown();
                //Shrink();
                //Stretch();
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
            var min = vertices[0];
            var max = vertices[0];
            scale *= multiplier;
            for (int i = 0; i < vertices.Length/2; i++)
            {
                min = Vector.Min(min, vertices[i]);
                max = Vector.Max(max, vertices[i]);
            }
            center1 = (min + max) / 2;
            
            min = vertices[3];
            max = vertices[3];
            scale *= multiplier;
            for (int i = vertices.Length/2; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i]);
                max = Vector.Max(max, vertices[i]);
            }
            center2 = (min + max) / 2;
        }

        private static void ChangeDirection()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].x >= 1 && velocity.x > 0 || vertices[i].x <= -1 && velocity.x < 0)
                {
                    velocity.x *= -1;
                    break;
                }
                if (vertices[i].y >= 1 && velocity.y > 0 || vertices[i].y <= -1 && velocity.y < 0)
                {
                    velocity.y *= -1;
                    break;
                }
            }
        }

        private static void RotateMethod2()
        {
            float angle = 0.002f;

            Vector vector2 = vertices[0] - center1;
            vertices[0].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[0].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;
            vector2 = vertices[1] - center1;
            vertices[1].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[1].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;
            vector2 = vertices[2] - center1;
            vertices[2].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center1.x;
            vertices[2].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center1.y;

            vector2 = vertices[3] - center2;
            vertices[3].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[3].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
            
            vector2 = vertices[4] - center2;
            vertices[4].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[4].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
            
            vector2 = vertices[5] - center2;
            vertices[5].x = vector2.x * MathF.Cos(angle) + vector2.y * MathF.Sin(angle) + center2.x;
            vertices[5].y = vector2.y * MathF.Cos(angle) - vector2.x * MathF.Sin(angle) + center2.y;
        }
        
        private static void RotateMethod1(float angle)
        {
            
            float r = 0.707f;
            
            vertices[2].x = (float) (0.5 * Math.Sin(angle));
            vertices[2].y = (float) (0.5 * Math.Cos(angle));
        
            vertices[1].x = (float) (0.5 * Math.Sin(angle + Math.PI * 120 / 180));
            vertices[1].y = (float) (0.5 * Math.Cos(angle + Math.PI * 120 / 180));
        
            vertices[0].x = (float) (0.5 * Math.Sin(angle - Math.PI * 120 / 180));
            vertices[0].y = (float) (0.5 * Math.Cos(angle - Math.PI * 120 / 180));
        }
        
        private static void ScaleUp()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i < vertices.Length/2)
                {
                    vertices[i] = (vertices[i] - center1) * multiplier + center1;
                }
                else
                {
                    vertices[i] = (vertices[i] - center2) * multiplier + center2;
                }
            }
        }
        
        private static void Stretch()
        {
            vertices[0].y -= 0.0001f;
            vertices[1].y -= 0.0001f;
            vertices[2].y += 0.0001f;
            
            vertices[3].y -= 0.0001f;
            vertices[4].y -= 0.0001f;
            vertices[5].y += 0.0001f;
        }
        
        private static void MoveUp()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector(0, velocity.y);
            }
        }
        private static void MoveDown()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector(0, -0.001f);
            }
        }
        private static void MoveRight()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector(velocity.x, 0);
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
            glShaderSource(vertexShader, File.ReadAllText("shaders/screen-coordinates.vert"));
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
            //glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, sizeof(Vector), NULL);
            glEnableVertexAttribArray(0);
        }

        private static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vector* vertex = &vertices[0])
            {
                //glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
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


