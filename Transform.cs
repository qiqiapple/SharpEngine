﻿namespace SharpEngine
{
    public class Transform
    {
        public Vector CurrentScale { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Matrix Matrix => Matrix.Translation((Position)) * Matrix.Rotation(Rotation)* Matrix.Scale(CurrentScale);
        //public Vector Forward => Matrix * Vector.Forward - Matrix * Vector.Zero;
        public Vector Forward => Matrix.Transform(Matrix, Vector.Forward, 1);
        //public Vector Backward => Matrix * Vector.Backward - Matrix *
        
        public Transform()
        {
            this.CurrentScale = new Vector(1f, 1f, 1f);
        }

        public void Scale(float multiplier)
        {
            CurrentScale *= multiplier;
        }
        
        

        public void Move(Vector direction)
        {
            this.Position += direction;
        }

        public void Rotate(float zAngle)
        {
            var rotation = this.Rotation;
            rotation.z += zAngle;
            this.Rotation = rotation;
        }
    }
}