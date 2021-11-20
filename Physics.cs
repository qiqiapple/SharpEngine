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
            for (int i = 0; i < this.scene.shapes.Count; i++)
            {
                Shape oriShape = this.scene.shapes[i];
                if (oriShape is Circle)
                {
                    //Circle shape = this.scene.shapes[i] as Circle;
                    Circle shape = oriShape as Circle;
                    ObjMovement(deltaTime, shape);
                    // collision detection
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            CircleCollision(shape, otherObj);
                            ObjMovement(deltaTime, otherObj);
                        }
                        if (other is Rectangle)
                        {
                            Rectangle otherObj = other as Rectangle;
                            CirRecCollision(shape, otherObj);
                            ObjMovement(deltaTime, otherObj);
                        }
                    }
                }

                if (oriShape is Rectangle)
                {
                    Rectangle shape = oriShape as Rectangle;
                    ObjMovement(deltaTime, shape);
                    for (int j = i+1; j < this.scene.shapes.Count; j++)
                    {
                        Shape other = this.scene.shapes[j];
                        if (other is Circle)
                        {
                            Circle otherObj = other as Circle;
                            CirRecCollision(otherObj, shape);
                            ObjMovement(deltaTime, otherObj);
                        }
                
                        if (other is Rectangle)
                        {
                            Rectangle otherObj = other as Rectangle;
                            RecRecCollision(shape, otherObj);
                            ObjMovement(deltaTime, otherObj);
                        }
                    }
                }
            }
        }

        private static void ObjMovement(float deltaTime, Shape obj)
        {
            var gravitationalAcceleration = Vector.Down * 9.819649f * 0f;
            if (obj is Rectangle)
            {
                Rectangle otherObj = obj as Rectangle;
                otherObj.Transform.Position = otherObj.Transform.Position + otherObj.velocity * deltaTime;
                var acceleration2 = otherObj.linearForce * otherObj.MassInverse;
                acceleration2 += gravitationalAcceleration * otherObj.gravityScale;
                otherObj.Transform.Position = otherObj.Transform.Position + acceleration2 * deltaTime * deltaTime / 2f;
                otherObj.velocity = otherObj.velocity + acceleration2 * deltaTime;
            }

            if (obj is Circle)
            {
                Circle otherObj = obj as Circle;
                otherObj.Transform.Position = otherObj.Transform.Position + otherObj.velocity * deltaTime;
                var acceleration2 = otherObj.linearForce * otherObj.MassInverse;
                acceleration2 += gravitationalAcceleration * otherObj.gravityScale;
                otherObj.Transform.Position = otherObj.Transform.Position + acceleration2 * deltaTime * deltaTime / 2f;
                otherObj.velocity = otherObj.velocity + acceleration2 * deltaTime;
            }
        }

        private static void CircleCollision(Circle shape, Circle other)
        {
            //Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlap = MathF.Pow(shape.Radius + other.Radius, 2) - deltaPosition.GetSquareMagnitude();
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
            //Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlapX = MathF.Pow(shape.Radius + other.width/2f, 2) - MathF.Pow(deltaPosition.x, 2);
            float squareOverlapY = MathF.Pow(shape.Radius + other.height/2f, 2) - MathF.Pow(deltaPosition.y, 2);
            
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
            //Vector deltaPosition = other.Transform.Position - shape.Transform.Position;
            Vector deltaPosition = other.GetCenter() - shape.GetCenter();
            float squareOverlapX = MathF.Pow(shape.width/2f + other.width/2f,2) - MathF.Pow(deltaPosition.x,2);
            float squareOverlapY = MathF.Pow(shape.height/2f + other.height/2f,2) - MathF.Pow(deltaPosition.y,2);
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