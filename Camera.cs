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
    }
}