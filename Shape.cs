using System;
using System.Runtime.InteropServices;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Shape
    {
        public Vertex[] vertices;
        private uint vertexArray;
        private uint vertexBuffer;
        public Vector direction;
        public Transform Transform { get; }
        public Material material;
        private float mass = 1;
        private float massInverse = 1;

        public float Mass
        {
            get => this.mass;
            set
            {
                this.mass = value;
                this.massInverse = float.IsPositiveInfinity(value) ? 0f : 1f / value;
            }
        }

        public float MassInverse => this.massInverse;
        public float gravityScale = 1f;
        public Vector velocity;
        public Vector linearForce;

        public Shape(Vertex[] vertices, Material material)
        {
            this.vertices = vertices;
            this.material = material;
            this.direction = new Vector(0.01f, 0.01f);
            LoadShapeIntoBuffer();
            this.Transform = new Transform();
        }

        public Vector GetMinBounds()
        {
            var matrix = this.Transform.Matrix;
            var min = matrix * this.vertices[0].position;
            for (int i = 1; i < this.vertices.Length; i++)
            {
                min = Vector.Min(min, matrix * this.vertices[i].position);
            }
            return min;
        }
        
        public Vector GetMaxBounds()
        {
            var matrix = this.Transform.Matrix;
            var max = matrix * this.vertices[0].position;
            for (int i = 1; i < this.vertices.Length; i++)
            {
                max = Vector.Max(max, matrix * this.vertices[i].position);
            }
            return max;
        }

        public Vector GetCenter()
        {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public void Bounce(float ratio)
        {
            if (this.GetMaxBounds().x>=ratio && direction.x>0 || this.GetMinBounds().x<=-ratio && direction.x<0)
            {
                direction.x *= -1;
            }
            if (this.GetMaxBounds().y>=1 && direction.y>0 || this.GetMinBounds().y<=-1 && direction.y<0)
            {
                direction.y *= -1;
            }
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                vertices[i].color = color;
            }
        }
        
        //public virtual unsafe void LoadShapeIntoBuffer()
        unsafe void LoadShapeIntoBuffer()
        {
            // load the vertices into a buffer
            vertexArray = glGenVertexArray();
            vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            // fixed (Vertex* vertex = &this.vertices[0])
            // {
            //     //Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_STATIC_DRAW);
            //     glBufferData(GL_ARRAY_BUFFER, Marshal.SizeOf<Vertex>() * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            // }

            // glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            // glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), NULL);
            // glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector));
            glVertexAttribPointer(0, 3, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex),nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex),nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glBindVertexArray(0);
        }

        //public virtual unsafe void Render()
        public unsafe void Render(Camera camera, float aspectRatio)
        {
            this.material.Use();
            this.material.SetTransform(this.Transform.Matrix);
            this.material.SetView(camera.View);
            this.material.SetProjection(camera.Projection);
            this.material.SetAspectRatio(aspectRatio);
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
            fixed (Vertex* vertex = &this.vertices[0])
            {
                //Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_STATIC_DRAW);
                glBufferData(GL_ARRAY_BUFFER, Marshal.SizeOf<Vertex>() * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }

            //Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, this.vertices.Length);
            //Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, 0, this.vertices.Length);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 0, this.vertices.Length);
            glBindVertexArray(0);
        }
    }
}