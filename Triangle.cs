using System;
using OpenGL;
using static OpenGL.Gl;
namespace SharpEngine
{
    public class Triangle
    {
        public Vertex[] vertices;
        public Vector velocity;
        public float CurrentScale { get; private set; }
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            this.CurrentScale = 1f;
            this.velocity = new Vector(0.003f, 0.002f);
            LoadTriangleIntoBuffer();
        }

        public Vector GetMinBounds()
        {
            var min = this.vertices[0].position;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                min = Vector.Min(min, this.vertices[i].position);
            }
            return min;
        }
        
        public Vector GetMaxBounds()
        {
            var max = this.vertices[0].position;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                max = Vector.Max(max, this.vertices[i].position);
            }
            return max;
        }

        public Vector GetCenter()
        {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public void Scale(float multiplier)
        {
            var center = GetCenter();
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position = (this.vertices[i].position - center) * multiplier + center;
            }

            this.CurrentScale *= multiplier;
        }
        
        public void Rotate()
        {
            float angle = 0.003f;
            var center = GetCenter();
            Vector rotation1 = new Vector(MathF.Cos(angle), MathF.Sin(angle));
            Vector rotation2 = new Vector(-MathF.Sin(angle), MathF.Cos(angle));
            for (int i = 0; i < this.vertices.Length; i++)
            {
                Vector vector2 = this.vertices[i].position - center;
                this.vertices[i].position = new Vector(rotation1 * vector2, rotation2 * vector2) + center;
            }
        }
        
        public void Move()
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position += this.velocity;
            }
        }

        public void Move(Vector direction)
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position += direction;
            }
        }

        public void Bounce()
        {
            if (this.GetMaxBounds().x >= 1 && this.velocity.x>0 || this.GetMinBounds().x<=-1 && this.velocity.x<0)
            {
                this.velocity.x *= -1;
            }
            if (this.GetMaxBounds().y >= 1 && this.velocity.y>0 || this.GetMinBounds().y<=-1 && this.velocity.y<0)
            {
                this.velocity.y *= -1;
            }
        }

        public unsafe void Render()
        {
            fixed (Vertex* vertex = &this.vertices[0])
            {
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_STATIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, this.vertices.Length);
        }
        
        public unsafe void LoadTriangleIntoBuffer()
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
    }
}