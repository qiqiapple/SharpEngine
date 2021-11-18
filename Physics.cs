using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
                Shape oriShape = this.scene.shapes[i];
                if (oriShape is Circle)
                {
                    //Circle shape = this.scene.shapes[i] as Circle;
                    Circle shape = oriShape as Circle;
                    shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
                    var acceleration = shape.linearForce * shape.MassInverse;
                    acceleration += gravitationalAcceleration * shape.gravityScale;
                    shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2; // p = p0+v0*t+a*t^2/2
                    shape.velocity = shape.velocity + acceleration * deltaTime;
                
                    // collision detection
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        //Circle other = this.scene.shapes[j] as Circle;
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            CircleCollision(shape, otherObj);
                        }

                        if (other is Rectangle)
                        {
                            Rectangle otherObj = other as Rectangle;
                            RecCollision(shape, otherObj);
                        }
                    }
                }

                if (oriShape is Rectangle)
                {
                    Rectangle shape = oriShape as Rectangle;
                    shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
                    var acceleration = shape.linearForce * shape.MassInverse;
                    acceleration += gravitationalAcceleration * shape.gravityScale;
                    shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2;
                    shape.velocity = shape.velocity + acceleration * deltaTime;
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            RecCollision(otherObj, shape);
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

        private static void CircleCollision(Circle shape, Circle other)
        {
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlap = deltaPosition.GetSquareMagnitude() - MathF.Pow(shape.Radius + other.Radius, 2);
            if (squareOverlap < 0)
            {
                float overlap = MathF.Sqrt(-squareOverlap);
                Vector collisionNormal = deltaPosition.Normalize();
                Vector shapeVelocity = collisionNormal * Vector.Dot(shape.velocity, collisionNormal);
                Vector otherVelocity = collisionNormal * Vector.Dot(other.velocity, collisionNormal);
                float totalMass = shape.Mass + other.Mass;
                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = shape.Mass / other.Mass * (-1) * velocityChange;
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;
                shape.Transform.Position -= overlap * other.Mass / totalMass * collisionNormal;
                other.Transform.Position += overlap * shape.Mass / totalMass * collisionNormal;

                //v1' = (m1-m2)/(m1+m2) * v1 + 2*m2/(m1+m2) * v2
                //v2' = 2*m1/(m1+m2) * v1 + (m1-m2)/(m1+m2) * v2
            }
        }
        
        private static void RecCollision(Circle shape, Rectangle other)
        {
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            
            float squareOverlapX = MathF.Pow(deltaPosition.x,2) - MathF.Pow(shape.Radius + other.width,2);
            float squareOverlapY = MathF.Pow(deltaPosition.y,2) - MathF.Pow(shape.Radius + other.height,2);
            if (squareOverlapX<0 && squareOverlapY<0)
            {
                float overlapX = MathF.Sqrt(-squareOverlapX);
                float overlapY = MathF.Sqrt(-squareOverlapY);
                Vector collisionNormal = deltaPosition.Normalize();
                Vector shapeVelocity = collisionNormal * Vector.Dot(shape.velocity, collisionNormal);
                Vector otherVelocity = collisionNormal * Vector.Dot(other.velocity, collisionNormal);
                float totalMass = shape.Mass + other.Mass;
                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = shape.Mass / other.Mass * (-1) * velocityChange;
                //AssertPhysicalCorrectness(shape.Mass,shape.velocity,other.Mass,other.velocity, 
                //    shape.Mass,shape.velocity+velocityChange,other.Mass,other.velocity+otherVelocity);
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;
                shape.Transform.Position -= new Vector(overlapX,overlapY) * other.Mass / totalMass;
                other.Transform.Position += new Vector(overlapX,overlapY) * shape.Mass / totalMass;
            }
        }
        private static void RecRecCollision(Rectangle shape, Rectangle other)
        {
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlapX = MathF.Pow(deltaPosition.x,2) - MathF.Pow(shape.width + other.width,2);
            float squareOverlapY = MathF.Pow(deltaPosition.y,2) - MathF.Pow(shape.height + other.height,2);
            if (squareOverlapX<0 && squareOverlapY<0)
            {
                float overlapX = MathF.Sqrt(-squareOverlapX);
                float overlapY = MathF.Sqrt(-squareOverlapY);
                Vector collisionNormal = deltaPosition.Normalize();
                Vector shapeVelocity = collisionNormal * Vector.Dot(shape.velocity, collisionNormal);
                Vector otherVelocity = collisionNormal * Vector.Dot(other.velocity, collisionNormal);
                float totalMass = shape.Mass + other.Mass;
                Vector velocityChange = 2 * other.Mass / totalMass * (otherVelocity - shapeVelocity);
                Vector otherVelocityChange = shape.Mass / other.Mass * (-1) * velocityChange;
                shape.velocity += velocityChange;
                other.velocity += otherVelocityChange;
                shape.Transform.Position -= new Vector(overlapX,overlapY) * other.Mass / totalMass;
                other.Transform.Position += new Vector(overlapX,overlapY) * shape.Mass / totalMass;
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
        private static void AssertPreservationOfMomentum(float m1, Vector v1, float m2, Vector v2, float n1, Vector u1,
            float n2, Vector u2)
        {
            Vector oldMomentum = CalculateTotalMomentum(m1, v1, m2, v2);
            Vector newMomentum = CalculateTotalMomentum(n1, u1, n2, u2);
            float tolerance = 0.0000000001f;
            Debug.Assert((oldMomentum - newMomentum).GetSquareMagnitude() < tolerance,
                $"Momentum not preserved. Old momentum: {oldMomentum}, New momentum: {newMomentum}");
        }
        private static void AssertPreservationOfKineticEnergy(float m1, Vector v1, float m2, Vector v2, float n1,
            Vector u1, float n2, Vector u2)
        {
            float oldTotalKineticEnergy = CalculateTotalKineticEnergy(m1, v1, m2, v2);
            float newTotalKineticEnergy = CalculateTotalKineticEnergy(n1, u1, n2, u2);
            float tolerance = 0.00001f;
            Debug.Assert(Math.Abs(oldTotalKineticEnergy - newTotalKineticEnergy) < tolerance, 
                $"Kinetic energy not preserved. Old kinetic energy: {oldTotalKineticEnergy}, " +
                $"Mew kinetic energy: {newTotalKineticEnergy}");
        }

        private static void AssertPhysicalCorrectness(float m1, Vector v1, float m2, Vector v2, float n1, Vector u1,
            float n2, Vector u2)
        {
            AssertPreservationOfMomentum(m1, v1, m2, v2, n1, u1, n2, u2);
            AssertPreservationOfKineticEnergy(m1, v1, m2, v2, n1, u1, n2, u2);
        }
    }
}