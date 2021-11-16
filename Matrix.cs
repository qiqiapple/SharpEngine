

using System;

namespace SharpEngine
{
    public struct Matrix
    {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;
        
        // generated using `ctorf`:
        public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, 
            float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13; this.m14 = m14;
            this.m21 = m21; this.m22 = m22; this.m23 = m23; this.m24 = m24;
            this.m31 = m31; this.m32 = m32; this.m33 = m33; this.m34 = m34;
            this.m41 = m41; this.m42 = m42; this.m43 = m43; this.m44 = m44;
        }

        public static Matrix Identity => new Matrix(1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, 1, 0,
                                                    0, 0, 0, 1);

        public static Vector operator *(Matrix m, Vector v)
        {
            return new Vector(m.m11 * v.x + m.m12 * v.y + m.m13 * v.z + m.m14 * 1,
                              m.m21 * v.x + m.m22 * v.y + m.m23 * v.z + m.m24 * 1,
                              m.m31 * v.x + m.m32 * v.y + m.m33 * v.z + m.m34 * 1);
        }
        
        public static Vector Transform(Matrix m, Vector v, float w=1f)
        {
            return new Vector(m.m11 * v.x + m.m12 * v.y + m.m13 * v.z + m.m14 * w,
                m.m21 * v.x + m.m22 * v.y + m.m23 * v.z + m.m24 * w,
                m.m31 * v.x + m.m32 * v.y + m.m33 * v.z + m.m34 * w);
        }
        
        public static Matrix operator *(Matrix m, Matrix n)
        {
            return new Matrix(
                m.m11 * n.m11 + m.m12 * n.m21 + m.m13 * n.m31 + m.m14 * n.m41,
                m.m11 * n.m12 + m.m12 * n.m22 + m.m13 * n.m32 + m.m14 * n.m42,
                m.m11 * n.m13 + m.m12 * n.m23 + m.m13 * n.m33 + m.m14 * n.m43,
                m.m11 * n.m14 + m.m12 * n.m24 + m.m13 * n.m34 + m.m14 * n.m44,
                
                m.m21 * n.m11 + m.m22 * n.m21 + m.m23 * n.m31 + m.m24 * n.m41,
                m.m21 * n.m12 + m.m22 * n.m22 + m.m23 * n.m32 + m.m24 * n.m42,
                m.m21 * n.m13 + m.m22 * n.m23 + m.m23 * n.m33 + m.m24 * n.m43,
                m.m21 * n.m14 + m.m22 * n.m24 + m.m23 * n.m34 + m.m24 * n.m44,
                
                m.m31 * n.m11 + m.m32 * n.m21 + m.m33 * n.m31 + m.m34 * n.m41,
                m.m31 * n.m12 + m.m32 * n.m22 + m.m33 * n.m32 + m.m34 * n.m42,
                m.m31 * n.m13 + m.m32 * n.m23 + m.m33 * n.m33 + m.m34 * n.m43,
                m.m31 * n.m14 + m.m32 * n.m24 + m.m33 * n.m34 + m.m34 * n.m44,
                
                m.m41 * n.m11 + m.m42 * n.m21 + m.m43 * n.m31 + m.m44 * n.m41,
                m.m41 * n.m12 + m.m42 * n.m22 + m.m43 * n.m32 + m.m44 * n.m42,
                m.m41 * n.m13 + m.m42 * n.m23 + m.m43 * n.m33 + m.m44 * n.m43,
                m.m41 * n.m14 + m.m42 * n.m24 + m.m43 * n.m34 + m.m44 * n.m44);
        }

        public static Matrix Translation(Vector translation)
        {
            var result = Identity;
            result.m14 = translation.x;
            result.m24 = translation.y;
            result.m34 = translation.z;
            return result;
        }

        public static Matrix Scale(Vector scaleVector)
        {
            var result = Identity;
            result.m11 = scaleVector.x;
            result.m22 = scaleVector.y;
            result.m33 = scaleVector.z;
            return result;
        }

        private static Matrix RotationX(float theta)
        {
            var result = Identity;
            result.m22 = MathF.Cos(theta);
            result.m23 = -MathF.Sin(theta);
            result.m32 = MathF.Sin(theta);
            result.m33 = MathF.Cos(theta);
            return result;
        }

        private static Matrix RotationY(float theta)
        {
            var result = Identity;
            result.m11 = MathF.Cos(theta);
            result.m13 = MathF.Sin(theta);
            result.m31 = -MathF.Sin(theta);
            result.m33 = MathF.Cos(theta);
            return result;
        }

        private static Matrix RotationZ(float theta)
        {
            var result = Identity;
            result.m11 = MathF.Cos(theta);
            result.m12 = -MathF.Sin(theta);
            result.m21 = MathF.Sin(theta);
            result.m22 = MathF.Cos(theta);
            return result;
        }

        public static Matrix Rotation(Vector rotation)
        {
            return RotationZ(rotation.z) * RotationY(rotation.y) * RotationX(rotation.x);
        }
    }
}