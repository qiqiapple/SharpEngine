using System;

namespace SharpEngine
{
    public struct Vector
    {
        public float x, y, z;
        public static Vector Forward => new Vector(0, 1);
        public static Vector Backward => new Vector(0, -1);
        public static Vector Right => new Vector(1, 0);
        public static Vector Left => new Vector(-1, 0);
        public static Vector Zero => new Vector(0,  0);

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

        public static float Dot(Vector u, Vector v)
        {
            return u.x * v.x + u.y * v.y;
        }

        public float GetMagnitude()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }

        public Vector Normalize()
        {
            var magnitude = GetMagnitude();
            return magnitude > 0 ? this/ GetMagnitude() : this;
        }

        public static float GetAngle(Vector a, Vector b)
        {
            //return MathF.Acos(Dot(a.Normalize(),b.Normalize())) * 180f / MathF.PI;
            return MathF.Acos(Dot(a.Normalize(),b.Normalize()));
        }
    }
}