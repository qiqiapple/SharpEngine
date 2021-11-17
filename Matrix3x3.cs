namespace SharpEngine
{
    public struct Matrix3x3
    {
        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;

        public Matrix3x3(float m11, float m12, float m13, 
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }
        public static Matrix3x3 Identity => new Matrix3x3(1, 0, 0,
                                                          0, 1, 0,
                                                          0, 0, 1);
        public static Matrix3x3 Zero => new Matrix3x3(0, 0, 0,
                                                      0, 0, 0,
                                                      0, 0, 0);

        public static Matrix3x3 DotDivide(Matrix3x3 a, float f)
        {
            a.m11 /= f; a.m12 /= f; a.m13 /= f;
            a.m21 /= f; a.m22 /= f; a.m23 /= f;
            a.m31 /= f; a.m32 /= f; a.m33 /= f;
            return a;
        }
        
        public static Vector operator *(Matrix3x3 m, Vector v)
        {
            return new Vector(m.m11 * v.x + m.m12 * v.y + m.m13 * v.z,
                               m.m21 * v.x + m.m22 * v.y + m.m23 * v.z,
                                 m.m31 * v.x + m.m32 * v.y + m.m33 * v.z);
        }
        
        public static float Determinant(Matrix3x3 m)
        {
            return m.m11 * m.m22 * m.m33 + m.m12 * m.m23 * m.m31 + m.m13 * m.m21 * m.m32 - 
                   m.m11 * m.m23 * m.m32 - m.m12 * m.m21 * m.m33 - m.m13 * m.m22 * m.m31;
        }

        public static Matrix3x3 Adjoint(Matrix3x3 a)
        {
            var b = Identity;
            b.m11 = (a.m22 * a.m33 - a.m32 * a.m23);
            b.m12 = (a.m12 * a.m33 - a.m32 * a.m13) * -1;
            b.m13 = (a.m12 * a.m23 - a.m22 * a.m13);
        
            b.m21 = (a.m21 * a.m33 - a.m31 * a.m23) * -1;
            b.m22 = (a.m11 * a.m33 - a.m31 * a.m13);
            b.m23 = (a.m11 * a.m23 - a.m21 * a.m13) * -1;
        
            b.m31 = (a.m21 * a.m32 - a.m22 * a.m31);
            b.m32 = (a.m11 * a.m32 - a.m31 * a.m12) * -1;
            b.m33 = (a.m11 * a.m22 - a.m12 * a.m21);
            return b;
        }

        public static Matrix3x3 Inverse(Matrix3x3 a)
        {
            var aDeterminant = Determinant(a);
            if (aDeterminant == 0)
            {
                return Zero;
            }
            var aAdjoint = Adjoint(a);
            var result = DotDivide(aAdjoint, aDeterminant);
            return result;
        }
    }

}