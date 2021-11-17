namespace SharpEngine
{
    public class Physics
    {
        private readonly Scene scene;

        public Physics(Scene scene)
        {
            this.scene = scene;
        }

        public void Update(float deltaTime)
        {
            var gravitationalAcceleration = Vector.Down * 9.819649f / 100f * 0f;
            for (int i = 0; i < this.scene.shapes.Count; i++)
            {
                Shape shape = this.scene.shapes[i];
                shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
                var acceleration = shape.linearForce * shape.MassInverse;
                acceleration += gravitationalAcceleration * shape.gravityScale;
                shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2; // p = p0+v0*t+a*t^2/2
                shape.velocity = shape.velocity + acceleration * deltaTime;
            }
        }
    }
}