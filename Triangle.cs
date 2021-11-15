namespace SharpEngine
{
    public class Triangle: Shape
    {
        public Triangle(Vertex[] vertices, Material material) : base(new Vertex[3],
            material)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                this.vertices[i].position = vertices[i].position;
                this.vertices[i].color = vertices[i].color;
            }
            
            
            
        }
        public Triangle(float width, float height, Vector position, Material material) : base(new Vertex[3],
            material)
        {
            vertices[0] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2), Color.Red);
            vertices[1] = new Vertex(new Vector(position.x + width / 2, position.y - height / 2), Color.Green);
            vertices[2] = new Vertex(new Vector(position.x, position.y + height / 2), Color.Blue);
        }
    }
}
