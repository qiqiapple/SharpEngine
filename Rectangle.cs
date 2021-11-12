using OpenGL;
using static OpenGL.Gl;
namespace SharpEngine
{
    public class Rectangle : Shape
    {
        //Indices[] indices = new Indices[]{new Indices(0),new Indices(1),new Indices(2),new Indices(0),new Indices(2),new Indices(3)};
    
        public Rectangle(float width, float height, Vector position) : base(new Vertex[4], new Vector(0.001f,0.002f))
        {
            vertices[0] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2), Color.Red);
            vertices[1] = new Vertex(new Vector(position.x + width / 2, position.y - height / 2), Color.Green);
            vertices[2] = new Vertex(new Vector(position.x + width / 2, position.y + height / 2), Color.Blue);
            vertices[3] = new Vertex(new Vector(position.x - width / 2, position.y + height / 2), Color.Yellow);
        }
        
        // public override unsafe void Render()
        // {
        //     fixed (Vertex* vertex = &this.vertices[0])
        //     {
        //         Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_STATIC_DRAW);
        //     }
        //
        //     fixed (Indices* indice = &this.indices[0])
        //     {
        //         Gl.glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(Indices)*this.indices.Length, indice, Gl.GL_STATIC_DRAW);
        //         Gl.glDrawElements(Gl.GL_TRIANGLES,indices.Length,GL_UNSIGNED_INT,indice);
        //     }
        //     Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, this.vertices.Length);
        //     
        // }
        
        // public override unsafe void LoadShapeIntoBuffer()
        // {
        //     var vertexArray = glGenVertexArray();
        //     glBindVertexArray(vertexArray);
        //     var vertexBuffer = glGenBuffer();
        //     glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
        //     var EBO = glGenBuffer();
        //     glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
        //
        //     glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), NULL);
        //     glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector));
        //     glEnableVertexAttribArray(0);
        //     glEnableVertexAttribArray(1);
        // }

    }
}