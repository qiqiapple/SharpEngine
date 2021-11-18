using System;
namespace SharpEngine
{
    public class Circle: Shape
    {
        public float Radius => 0.1f * Transform.CurrentScale.x;
        private static int partition = 16;

        public Circle(Vector position, Material material) : 
            base(new Vertex[partition+2], material)
        {
            float theta = MathF.PI * 2 / partition;
            vertices[0].position = position;
            vertices[0].color = Color.Yellow;
            
            for (int i = 1; i < partition + 1; i++)
            {
                vertices[i].position.x = Radius * MathF.Cos(theta * (i-1)) + position.x;
                vertices[i].position.y = Radius * MathF.Sin(theta * (i-1)) + position.y;
                if(i % 9 < 3) vertices[i].color = Color.Red;
                else if(i %9 >=3 && i % 9 <6) vertices[i].color = Color.Green;
                else vertices[i].color =Color.Blue;
            }
            vertices[partition + 1] = vertices[1];
        }

        public Circle(Material material) : base(CreateCircle(), material){ }
        static Vertex[] CreateCircle()
        {
            const int numberOfSegments = 8;
            const int verticesPerSegment = 3;
            const float scale = 0.1f;
            Vertex[] result = new Vertex[numberOfSegments*verticesPerSegment];
            const float circleRadians = MathF.PI * 2;
            var oldAngle = 0f;
            for (int i = 0; i < numberOfSegments; i++) {
                int currentVertex = i * verticesPerSegment;
                var newAngle = circleRadians / numberOfSegments * (i + 1);
                result[currentVertex++] = new Vertex(new Vector(), Color.Blue);
                result[currentVertex++] = new Vertex(new Vector(MathF.Cos(oldAngle), MathF.Sin(oldAngle))*scale, Color.Green);
                result[currentVertex] = new Vertex(new Vector(MathF.Cos(newAngle), MathF.Sin(newAngle))*scale, Color.Red);
                oldAngle = newAngle;
            }
            return result;
        }
    }
}