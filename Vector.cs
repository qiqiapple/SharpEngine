using System;

namespace SharpEngine
{
    public struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public static Vector operator +(Vector u, Vector v)
        {
            return new Vector(u.x + v.x, u.y + v.y, u.z + v.z);
        }
        
        public static Vector operator -(Vector u, Vector v)
        {
            return new Vector(u.x - v.x, u.y - v.y, u.z - v.z);
        }

        public static Vector Max(Vector a, Vector b)
        {
            float maxX = Math.Max(a.x, b.x);
            float maxY = Math.Max(a.y, b.y);
            return new Vector(maxX, maxY);
        }

        public static Vector Min(Vector a, Vector b)
        {
            float minX = Math.Min(a.x, b.x);
            float minY = Math.Min(a.y, b.y);
            return new Vector(minX, minY);
        }

        public static float operator *(Vector u, Vector v)
        {
            return u.x * v.x + u.y * v.y;
        }
    }
}