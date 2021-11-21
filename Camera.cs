using System;
using GLFW;

namespace SharpEngine
{
    public class Camera
    {
        public Transform Transform { get; }
        public Matrix View => this.Transform.Matrix;
        public Matrix Projection { get; private set; }
        public Camera()
        {
            this.Transform = new Transform();
            this.Projection = Matrix.Perspective(90f, 1f, 0.1f, 100f);
        }

        public void ZoomInOut(float viewScale)
        {
            this.Transform.Scale(viewScale);
        }

        public void CameraControl(Window window, float fixedDeltaTime)
        {
            float viewScale = 1f;
           
            if (window.GetKey(Keys.M))
            {
                viewScale *= 0.99f;
                this.ZoomInOut(viewScale);
            }

            if (window.GetKey(Keys.N))
            {
                viewScale *= 1.01f;
                this.ZoomInOut(viewScale);
            }

            if (window.GetKey(Keys.A))
            {
                Vector moveVec = Vector.Right * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }

            if (window.GetKey(Keys.D))
            {
                Vector moveVec = Vector.Left * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }

            if (window.GetKey(Keys.W))
            {
                Vector moveVec = Vector.Down * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }
            
            if (window.GetKey(Keys.S))
            {
                Vector moveVec = Vector.Up * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }
            
            if (window.GetKey(Keys.K))
            {
                Vector moveVec = new Vector(0f, 0f, 1f) * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }
            if (window.GetKey(Keys.L))
            {
                Vector moveVec = new Vector(0f, 0f, -1f) * fixedDeltaTime;
                this.Transform.Move(moveVec);
            }

        }
    }
}