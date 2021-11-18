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
            var gravitationalAcceleration = Vector.Down * 9.819649f * 0f;
            for (int i = 0; i < this.scene.shapes.Count; i++)
            {
                //Shape shape = this.scene.shapes[i];
                Circle shape = this.scene.shapes[i] as Circle;
                shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
                var acceleration = shape.linearForce * shape.MassInverse;
                acceleration += gravitationalAcceleration * shape.gravityScale;
                shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2; // p = p0+v0*t+a*t^2/2
                shape.velocity = shape.velocity + acceleration * deltaTime;
                
                // collision detection
                for (int j = i+1; j < this.scene.shapes.Count; j++)
                {
                    Circle other = this.scene.shapes[j] as Circle;
                    Vector deltaPosition = other.GetCenter() - shape.GetCenter();
                    bool collision = deltaPosition.GetMagnitude() < shape.Radius + other.Radius;
                    if (collision)
                    {
                        Vector collisionNormal = deltaPosition.Normalize();
                        Vector shapeVelocity = collisionNormal * Vector.Dot(shape.velocity, collisionNormal);
                        Vector otherVelocity = collisionNormal * Vector.Dot(other.velocity, collisionNormal);
                        Vector newVelocity = shapeVelocity*(shape.Mass-other.Mass)/(shape.Mass+other.Mass) + otherVelocity*other.Mass/(shape.Mass+other.Mass)*2f;
                        Vector newOtherVelocity = otherVelocity*2*shape.Mass/(shape.Mass+other.Mass) + shapeVelocity*(other.Mass-shape.Mass) / (shape.Mass+other.Mass);

                        shape.velocity += newVelocity - shapeVelocity;
                        other.velocity += newOtherVelocity - otherVelocity;
                    }
                }
            }
        }
    }
}