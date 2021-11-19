using GLFW;

namespace SharpEngine
{
    public class Camera
    {
        public Matrix viewMatrix;
        public Camera()
        {
            viewMatrix = Matrix.Identity;
        }

        public void ZoomInOut(float viewScale)
        {
            viewMatrix *= Matrix.Scale(new Vector(viewScale, viewScale,viewScale));
        }

        public void MoveCamera(Vector vector)
        {
            viewMatrix *= Matrix.Translation(vector);
        }

        public void CameraControl(Window window, Material material)
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
                Vector moveVec = new Vector(0.01f, 0, 0);
                this.MoveCamera(moveVec);
            }

            if (window.GetKey(Keys.D))
            {
                Vector moveVec = new Vector(-0.01f, 0, 0);
                this.MoveCamera(moveVec);
            }

            if (window.GetKey(Keys.W))
            {
                Vector moveVec = new Vector(0, -0.01f, 0);
                this.MoveCamera(moveVec);
            }

            if (window.GetKey(Keys.S))
            {
                Vector moveVec = new Vector(0, 0.01f, 0);
                this.MoveCamera(moveVec);
            }

            material.SetViewTransform(this.viewMatrix);
        }
    }
}