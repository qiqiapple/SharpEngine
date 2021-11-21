using OpenGL;
using static OpenGL.Gl;
namespace SharpEngine
{
    public class Rectangle : Shape
    {
        public float width, height;
        //Indices[] indices = new Indices[]{new Indices(0),new Indices(1),new Indices(2),new Indices(0),new Indices(2),new Indices(3)};

        public Rectangle(Vertex[] vertices, Material material): base(new Vertex[4], material)
        {
            for (int i = 0; i < 3; i++)
            {
                vertices[i].position = this.vertices[i].position;
            }

            vertices[3].position = this.vertices[0].position + this.vertices[2].position - this.vertices[1].position;
            vertices[0].color = Color.Red;
            vertices[1].color = Color.Green;
            vertices[2].color = Color.Blue;
            vertices[3].color = Color.Yellow;

            this.width = (vertices[1].position - vertices[0].position).GetMagnitude();
            this.height = (vertices[1].position - vertices[2].position).GetMagnitude();
        }


        public Rectangle(float width, float height, Material material) : 
            base(new Vertex[4], material)
        {
            vertices[0] = new Vertex(new Vector(-width/2, -height/2), Color.Red);
            vertices[1] = new Vertex(new Vector(width/2, -height/2), Color.Green);
            vertices[2] = new Vertex(new Vector(width/2, height/2), Color.Blue);
            vertices[3] = new Vertex(new Vector(-width/2, height/2), Color.Yellow);
            this.width = width;
            this.height = height;
        }
        
        // public Rectangle(float width, float height, Material material) : base(CreateRectangle(width, height), material)
        // {
        //     this.width = width;
        //     this.height = height;
        // }

        static Vertex[] CreateRectangle(float width, float height) {
            const float scale = 0.1f;
            return new Vertex[] {
                new Vertex(new Vector(-width/2, -height/2), Color.Red), // LB
                new Vertex(new Vector(width/2, -height/2), Color.Green), // RB
                new Vertex(new Vector(-width/2, height/2), Color.Blue), // LT
                new Vertex(new Vector(width/2, -height/2), Color.Green), // RB
                new Vertex(new Vector(width/2, height/2), Color.Red), // RT
                new Vertex(new Vector(-width/2, height/2), Color.Blue) // LT
            };
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