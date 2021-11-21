using System;
using System.Runtime.CompilerServices;

namespace SharpEngine
{
    public class Circle: Shape
    {
        public float Radius; // => 0.1f * Transform.CurrentScale.x;
        private static int numberOfSegments = 8;

        public Circle(float radius, Material material) : 
            base(new Vertex[numberOfSegments+2], material)
        {
            this.Radius = radius;
            float theta = MathF.PI * 2 / numberOfSegments;
            vertices[0].color = Color.Yellow;
            
            for (int i = 1; i < numberOfSegments + 1; i++)
            {
                vertices[i].position.x = Radius * MathF.Cos(theta * (i-1));
                vertices[i].position.y = Radius * MathF.Sin(theta * (i-1));
                // if(i % 9 < 3) vertices[i].color = Color.Red;
                // else if(i %9 >=3 && i % 9 <6) vertices[i].color = Color.Green;
                // else vertices[i].color =Color.Blue;
                if(i % 3 == 0) vertices[i].color = Color.Red;
                if(i % 3 == 1) vertices[i].color = Color.Green;
                if(i % 3 == 2) vertices[i].color = Color.Blue;
            }
            vertices[numberOfSegments + 1] = vertices[1];
        }

        // public Circle(float radius, Material material) : base(CreateCircle(radius), material)
        // {
        //     this.Radius = radius;
        // }
        static Vertex[] CreateCircle(float radius)
        {
            const int numberOfSegments = 8;
            const int verticesPerSegment = 3;
            Vertex[] result = new Vertex[numberOfSegments*verticesPerSegment];
            const float circleRadians = MathF.PI * 2;
            var oldAngle = 0f;
            for (int i = 0; i < numberOfSegments; i++) {
                int currentVertex = i * verticesPerSegment;
                var newAngle = circleRadians / numberOfSegments * (i + 1);
                result[currentVertex++] = new Vertex(new Vector(), Color.Blue);
                result[currentVertex++] = new Vertex(new Vector(MathF.Cos(oldAngle), MathF.Sin(oldAngle))*radius, Color.Green);
                result[currentVertex] = new Vertex(new Vector(MathF.Cos(newAngle), MathF.Sin(newAngle))*radius, Color.Red);
                oldAngle = newAngle;
            }
            return result;
        }
    }
}