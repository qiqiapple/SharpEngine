﻿using System;
namespace SharpEngine
{
    public class Circle: Shape
    {
        private static int partition = 36;
        public float Radius { get; private set; }

        public Circle(float radius, Vector position, Material material) : 
            base(new Vertex[partition+2], material)
        {
            Radius = radius;
            float theta = MathF.PI * 2 / partition;
            vertices[0].position = position;
            vertices[0].color = Color.Yellow;
            
            for (int i = 1; i < partition + 1; i++)
            {
                vertices[i].position.x = radius * MathF.Cos(theta * (i-1)) + position.x;
                vertices[i].position.y = radius * MathF.Sin(theta * (i-1)) + position.y;
                if(i % 9 < 3) vertices[i].color = Color.Red;
                else if(i %9 >=3 && i % 9 <6) vertices[i].color = Color.Green;
                else vertices[i].color =Color.Blue;
            }
            
            vertices[partition + 1] = vertices[1];
        }
    }
}