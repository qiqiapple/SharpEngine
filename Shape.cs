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
        private Matrix transform = Matrix.Identity;
        private uint vertexArray;
        private uint vertexBuffer;
        public Vector direction;
        
        public float CurrentScale { get; private set; }
        public Material material;
        
        public Shape(Vertex[] vertices, Material material)
        {
            this.vertices = vertices;
            this.material = material;
            this.CurrentScale = 1f;
            this.direction = new Vector(0.003f, 0.002f);
            LoadShapeIntoBuffer();
        }
        
        //public virtual unsafe void LoadShapeIntoBuffer()
        void LoadShapeIntoBuffer()
        {
            // load the vertices into a buffer
            vertexArray = glGenVertexArray();
            vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);

            // glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            // glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), NULL);
            // glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector));
            glVertexAttribPointer(0, 3, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex),nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex),nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glBindVertexArray(0);
        }

        public Vector GetMinBounds()
        {
            
            // var min = this.vertices[0].position;
            // for (int i = 0; i < this.vertices.Length; i++)
            // {
            //     min = Vector.Min(min, this.vertices[i].position);
            // }
            var min = this.transform * vertices[0].position;
            for (int i = 1; i < this.vertices.Length; i++)
            {
                min = Vector.Min(min, this.transform * this.vertices[i].position);
            }

            return min;
        }
        
        public Vector GetMaxBounds()
        {
            // var max = this.vertices[0].position;
            // for (int i = 0; i < this.vertices.Length; i++)
            // {
            //     max = Vector.Max(max, this.vertices[i].position);
            // }
            var max = this.transform * vertices[0].position;
            for (int i = 1; i < this.vertices.Length; i++)
            {
                max = Vector.Max(max, this.transform * this.vertices[i].position);
            }
            
            return max;
        }

        public Vector GetCenter()
        {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public void Scale(float multiplier)
        {
            // var offset = GetCenter();
            // Matrix matrix = Matrix.Scale(new Vector(multiplier,multiplier));
            // for (int i = 0; i < this.vertices.Length; i++)
            // {
            //     //this.vertices[i].position = (this.vertices[i].position - center) * multiplier + center;
            //     this.vertices[i].position = matrix * (this.vertices[i].position - offset) + offset;
            // }

            Move(GetCenter()*(-1));
            this.transform = Matrix.Scale(new Vector(multiplier,multiplier)) * this.transform;
            Move(GetCenter());
            this.CurrentScale *= multiplier;
        }
        
        public void Rotate()
        {
            float angle = 0.003f;
            var offset = GetCenter();
            // Vector rotationX = new Vector(MathF.Cos(angle), MathF.Sin(angle));
            // Vector rotationY = new Vector(-MathF.Sin(angle), MathF.Cos(angle));
            // for (int i = 0; i < this.vertices.Length; i++)
            // {
            //     Vector shift = this.vertices[i].position - offset;
            //     this.vertices[i].position = new Vector(Vector.Dot(rotationX, shift), Vector.Dot(rotationY,shift)) + offset;
            // }
            Matrix matrix = Matrix.Rotate(angle);
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position = matrix * (this.vertices[i].position - offset) + offset;
            }
        }
        
        // public void Move()
        // {
        //     for (int i = 0; i < this.vertices.Length; i++)
        //     {
        //         this.vertices[i].position += this.velocity;         
        //     }
        // }

        public void Move()
        {
           this.transform = Matrix.Translation(direction) * this.transform;
        }
        
        public void Move(Vector direction)
        {
            this.transform = Matrix.Translation(direction) * this.transform;
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

        //public virtual unsafe void Render()
        public unsafe void Render()
        {
            this.material.Use();
            this.material.SetTransform(this.transform);
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
            fixed (Vertex* vertex = &this.vertices[0])
            {
                //Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_STATIC_DRAW);
                glBufferData(GL_ARRAY_BUFFER, Marshal.SizeOf<Vertex>() * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, this.vertices.Length);
            //Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, 0, this.vertices.Length);
            //Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 0, this.vertices.Length);
            glBindVertexArray(0);
        }
        

    }
}