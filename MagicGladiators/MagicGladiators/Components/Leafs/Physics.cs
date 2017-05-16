﻿using Microsoft.Xna.Framework;
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

        public Physics(GameObject gameObject) : base(gameObject)
        {

        }

        public Vector2 GetVector(Vector2 origin, Vector2 target)
        {
            return origin - target;
        }

        public Vector2 physicsBreak(float breakFactor, Vector2 velocity)
        {

            float distanceTest = Vector2.Distance(velocity, Vector2.Zero);
            if (!(Vector2.Distance(velocity, Vector2.Zero) < 0.05F && Vector2.Distance(velocity, Vector2.Zero) > -0.05F))
            {
                if (gameObject.Tag == "HomingMissile")
                {
                    Acceleration = 0.001F * -velocity;
                }
                else
                {
                    Acceleration = breakFactor * -velocity;
                }

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
