using OpenGL;
namespace SharpEngine
{
    public class Rectangle: Shape
    {
        public Rectangle(float width, float height, Vector position) : base(new Vertex[4], new Vector(0.001f,0.002f))
        {
            vertices[0] = new Vertex(new Vector(position.x - width / 2, position.y - height / 2), Color.Red);
            vertices[1] = new Vertex(new Vector(position.x + width / 2, position.y - height / 2), Color.Green);
            vertices[2] = new Vertex(new Vector(position.x + width / 2, position.y + height / 2), Color.Blue);
            vertices[3] = new Vertex(new Vector(position.x - width / 2, position.y + height / 2), Color.Yellow);
        }
    }
}