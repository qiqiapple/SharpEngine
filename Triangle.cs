namespace SharpEngine
{
    public class Triangle: Shape
    {
        public Triangle(float width, float height, Vector position) : base(new Vertex[3], new Vector(0.003f,0.002f))
        {
            vertices[0] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2), Color.Red);
            vertices[1] = new Vertex(new Vector(position.x + width / 2, position.y - height / 2), Color.Green);
            vertices[2] = new Vertex(new Vector(position.x, position.y + height / 2), Color.Blue);
        }
    }
}