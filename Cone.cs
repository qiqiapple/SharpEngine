using System;

namespace SharpEngine
{
    public class Cone: Shape
    {
        public float Radius;
        private static int numberOfSegments = 8;
        public Cone(float radius, Material material) : 
            base(new Vertex[numberOfSegments+2], material)
        {
            this.Radius = radius;
            float theta = MathF.PI * 2 / numberOfSegments;
            vertices[0].position = new Vector(0, 3f) * Radius;
            vertices[0].color = Color.Yellow;
            
            for (int i = 1; i < numberOfSegments + 1; i++)
            {
                vertices[i].position.x = Radius * MathF.Cos(theta * (i-1));
                vertices[i].position.y = Radius * MathF.Sin(theta * (i-1));
                if(i % 3 == 0) vertices[i].color = Color.Red;
                if(i % 3 == 1) vertices[i].color = Color.Green;
                if(i % 3 == 2) vertices[i].color = Color.Blue;
            }
            vertices[numberOfSegments + 1] = vertices[1];
        }
    }
}