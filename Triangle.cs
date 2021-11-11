using OpenGL;

namespace SharpEngine
{
    public class Triangle
    {
        public Vertex[] vertices;
        public float CurrentScale { get; private set; }

        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            this.CurrentScale = 1f;
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

        public void Move(Vector direction)
        {
            for (int i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position += direction;
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
    }
}