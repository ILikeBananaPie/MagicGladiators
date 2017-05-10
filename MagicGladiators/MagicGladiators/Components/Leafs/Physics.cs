using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Physics : Component, IUpdateable
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        private float breakFactor = 0.050F;
        private static int maxValue = int.MaxValue;

        public Physics(GameObject gameObject) : base(gameObject)
        {

        }

        public Vector2 GetVector(Vector2 origin, Vector2 target)
        {
            float x = 0;
            float y = 0;
            try
            {
                x = checked(origin.X - target.X);
            }
            catch (System.OverflowException)
            {
                throw new OverflowException("Overflow exception; you're way out of bounds");
            }
            try
            {
                y = checked(origin.Y - target.Y);
            }
            catch (System.OverflowException)
            {
                throw new OverflowException("Overflow exception; you're way out of bounds");
            }
            return origin - target;
        }

        public Vector2 physicsBreak(float breakFactor, Vector2 velocity)
        {
            float distanceTest = Vector2.Distance(velocity, Vector2.Zero);
            if (!(Vector2.Distance(velocity, Vector2.Zero) < 0.05F && Vector2.Distance(velocity, Vector2.Zero) > -0.05F))
            {
                Acceleration = breakFactor * -velocity;
                //velocityTest += accelerationTest;
                //accelerationTest = Vector2.Zero;
            }
            else
            {
                Acceleration = Vector2.Zero;
                Velocity = Vector2.Zero;
            }
            Velocity = UpdateVelocity(Acceleration, Velocity);
            return Acceleration;
        }

        public Vector2 UpdateVelocity(Vector2 acceleration, Vector2 velocity)
        {
            return velocity += acceleration;
        }

        public void Update()
        {
            Velocity = UpdateVelocity(Acceleration, Velocity);
            Acceleration = physicsBreak(breakFactor, Velocity);
        }
    }
}
