using System;
using System.Diagnostics;

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
                // Circle shape = this.scene.shapes[i] as Circle;
                // CirMovement(deltaTime, shape, gravitationalAcceleration);
                // for (int j = i+1; j < this.scene.shapes.Count; j++)
                // {
                //     Circle other = this.scene.shapes[j] as Circle;
                //     CirCirCollision(shape, other);
                // }
                
                Shape oriShape = this.scene.shapes[i];
                if (oriShape is Circle)
                {
                    Circle shape = oriShape as Circle;
                    CirMovement(deltaTime, shape, gravitationalAcceleration);
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            CirCirCollision(shape, otherObj);
                        }
                        if (other is Rectangle)
                        {
                            Rectangle otherObj = other as Rectangle;
                            CirRecCollision(shape, otherObj);
                        }
                    }
                }
                
                if (oriShape is Rectangle)
                {
                    Rectangle shape = oriShape as Rectangle;
                    RecMovement(deltaTime, shape, gravitationalAcceleration);
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            CirRecCollision(otherObj, shape);
                        }
                
                        if (other is Rectangle)
                        {
                            Rectangle otherObj = other as Rectangle;
                            RecRecCollision(shape, otherObj);
                        }
                    }
                }
            }
        }

        private static void CirMovement(float deltaTime, Circle shape, Vector gravitationalAcceleration)
        {
            shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
            var acceleration = shape.linearForce * shape.MassInverse;
            acceleration += gravitationalAcceleration * shape.gravityScale;
            shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2f;
            shape.velocity = shape.velocity + acceleration * deltaTime;
        }
        
        private static void RecMovement(float deltaTime, Rectangle shape, Vector gravitationalAcceleration)
        {
            shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
            var acceleration = shape.linearForce * shape.MassInverse;
            acceleration += gravitationalAcceleration * shape.gravityScale;
            shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2f;
            shape.velocity = shape.velocity + acceleration * deltaTime;
        }

        private static void CirCirCollision(Circle shape, Circle other)
        {
            Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            //Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlap = (shape.Radius+other.Radius)*(shape.Radius+other.Radius) - deltaPosition.GetSquareMagnitude();
            if (squareOverlap > 0)
            {
                float overlap = MathF.Sqrt(squareOverlap);
                float totalMass = shape.Mass + other.Mass;
                Vector collisionNormal = deltaPosition.Normalize();
                shape.Transform.Position -= overlap * other.Mass / totalMass * collisionNormal;
                other.Transform.Position += overlap * shape.Mass / totalMass * collisionNormal;
                
                Vector shapeVelocity = Vector.Dot(shape.velocity, collisionNormal) * collisionNormal;
                Vector otherVelocity = Vector.Dot(other.velocity, collisionNormal) * collisionNormal;

                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = (-1) * shape.Mass / other.Mass * velocityChange;
                //Vector otherVelocityChange = 2 * shape.Mass / totalMass * (shapeVelocity - otherVelocity);
                
                 AssertPhysicalCorrectness(shape.Mass,shape.velocity,shape.velocity + velocityChange, 
                     other.Mass,other.velocity,other.velocity + otherVelocityChange);
            
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;

                //v1' = (m1-m2)/(m1+m2) * v1 + 2*m2/(m1+m2) * v2
                //v2' = 2*m1/(m1+m2) * v1 + (m1-m2)/(m1+m2) * v2
            }
        }
        
        private static void CirRecCollision(Circle shape, Rectangle other)
        {
            Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            //Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlapX = (shape.Radius+other.width/2f)*(shape.Radius+other.width/2f) - deltaPosition.x*deltaPosition.x;
            float squareOverlapY = (shape.Radius+other.height/2f)*(shape.Radius+other.height/2f) - deltaPosition.y*deltaPosition.y;
            
            if (squareOverlapX > 0 && squareOverlapY > 0)
            {
                float overlapX = MathF.Sqrt(squareOverlapX);
                float overlapY = MathF.Sqrt(squareOverlapY);
                float totalMass = shape.Mass + other.Mass;
                Vector collisionNormal = deltaPosition.Normalize();

                float overlap = overlapX < overlapY ? overlapX : overlapY;
                shape.Transform.Position -= overlap * other.Mass / totalMass * collisionNormal;
                other.Transform.Position += overlap * shape.Mass / totalMass * collisionNormal;
                
                Vector shapeVelocity = Vector.Dot(shape.velocity, collisionNormal) * collisionNormal;
                Vector otherVelocity = Vector.Dot(other.velocity, collisionNormal) * collisionNormal;
                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = (-1) * shape.Mass / other.Mass * velocityChange;
                AssertPhysicalCorrectness(shape.Mass,shape.velocity,shape.velocity + velocityChange, 
                    other.Mass,other.velocity,other.velocity + otherVelocityChange);
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;
            }
        }
        private static void RecRecCollision(Rectangle shape, Rectangle other)
        {
            Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            //Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlapX = (shape.width/2f+other.width/2f)*(shape.width/2f+other.width/2f) - deltaPosition.x*deltaPosition.x;
            float squareOverlapY = (shape.height/2f+other.height/2f)*(shape.height/2f+other.height/2f) - deltaPosition.y*deltaPosition.y;
            if (squareOverlapX > 0 && squareOverlapY > 0)
            {
                float overlapX = MathF.Sqrt(squareOverlapX);
                float overlapY = MathF.Sqrt(squareOverlapY);
                float totalMass = shape.Mass + other.Mass;
                Vector collisionNormal = deltaPosition.Normalize();
                float overlap = overlapX < overlapY ? overlapX : overlapY;
                shape.Transform.Position -= overlap * other.Mass / totalMass * collisionNormal;
                other.Transform.Position += overlap * shape.Mass / totalMass * collisionNormal;
                Vector shapeVelocity = collisionNormal * Vector.Dot(shape.velocity, collisionNormal);
                Vector otherVelocity = collisionNormal * Vector.Dot(other.velocity, collisionNormal);
                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = shape.Mass / other.Mass * (-1) * velocityChange;
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;
            }
        }
        
        private static Vector CalculateMomentum(float m, Vector v)
        {
            return m * v;
        }
        private static Vector CalculateTotalMomentum(float m1, Vector v1, float m2, Vector v2)
        {
            return CalculateMomentum(m1, v1) + CalculateMomentum(m2, v2);
        }
        private static float CalculateKineticEnergy(float m, Vector v)
        {
            return 0.5f * m * v.GetSquareMagnitude();
        }
        private static float CalculateTotalKineticEnergy(float m1, Vector v1, float m2, Vector v2)
        {
            return CalculateKineticEnergy(m1, v1) + CalculateKineticEnergy(m2, v2);
        }
        private static void AssertPreservationOfMomentum(float m1, Vector v1, Vector v1_, float m2, Vector v2, Vector v2_)
        {
            Vector oldMomentum = CalculateTotalMomentum(m1, v1, m2, v2);
            Vector newMomentum = CalculateTotalMomentum(m1, v1_, m2, v2_);
            float tolerance = 0.0001f;
            Debug.Assert((oldMomentum - newMomentum).GetMagnitude() < tolerance,
                $"Momentum not preserved. Old: {oldMomentum}, New: {newMomentum}");
        }
        private static void AssertPreservationOfKineticEnergy(float m1, Vector v1, Vector v1_, float m2, Vector v2, Vector v2_)
        {
            float oldTotalKineticEnergy = CalculateTotalKineticEnergy(m1, v1, m2, v2);
            float newTotalKineticEnergy = CalculateTotalKineticEnergy(m1, v1_, m2, v2_);
            float tolerance = 0.0001f;
            Debug.Assert(Math.Abs(oldTotalKineticEnergy - newTotalKineticEnergy) < tolerance, 
                $"Kinetic energy not preserved. Old kinetic energy: {oldTotalKineticEnergy}, Mew kinetic energy: {newTotalKineticEnergy}");
        }

        private static void AssertPhysicalCorrectness(float m1, Vector v1, Vector v1_, float m2, Vector v2, Vector v2_)
        {
            AssertPreservationOfMomentum(m1, v1, v1_, m2, v2, v2_);
            AssertPreservationOfKineticEnergy(m1, v1, v1_, m2, v2, v2_);
        }
    }
}