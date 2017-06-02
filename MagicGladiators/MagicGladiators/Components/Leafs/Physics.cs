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
        public bool chainDeactivated { get; set; } = false;
        public bool chainActivated { get; set; } = false;
        private float timer;
        private Vector2 breaking = new Vector2(0.05F, 0.05F);

        private float chainDeactivatedTimer = 0;
        private float chainActivatedTimer = 0;

        public Physics(GameObject gameObject) : base(gameObject)
        {

        }

        public Vector2 GetVector(Vector2 origin, Vector2 target)
        {
            return origin - target;
        }

        public Vector2 physicsBreak(float breakFactor, Vector2 velocity)
        {
            if (gameObject.Tag == "Dummy")
            {

            }
            float distanceTest = Vector2.Distance(velocity, Vector2.Zero);
            if (!(Vector2.Distance(velocity, Vector2.Zero) < 0.005F && Vector2.Distance(velocity, Vector2.Zero) > -0.005F) && gameObject.Id == GameWorld.Instance.player.Id)
            {
                if (gameObject.Tag != "Player" && gameObject.Tag != "Enemy")
                {

                }
                if (gameObject.Tag == "HomingMissile" || gameObject.Tag == "Boomerang")
                {
                    if (gameObject.Tag == "Boomerang")
                    {
                        Acceleration = 0.001F * -velocity;
                        //Acceleration = new Vector2(Acceleration.X + Acceleration.X * 10F, Acceleration.Y);

                    }
                    else Acceleration = 0.004F * -velocity;
                }
                else if (chainActivated)
                {
                    Acceleration = 0.001F * -velocity;
                }
                else if (chainDeactivated)
                {
                    timer += GameWorld.Instance.deltaTime;
                    if (timer > 0.5F)
                    {
                        chainDeactivated = false;
                        timer = 0;
                    }
                    else Acceleration = 0.001F * -velocity;
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
                if (gameObject.Id == GameWorld.Instance.player.Id)
                {
                    Acceleration = Vector2.Zero;
                    Velocity = Vector2.Zero;
                }

            }
            if (chainActivated)
            {
                chainActivatedTimer += GameWorld.Instance.deltaTime;
                if (chainActivatedTimer > 2)
                {
                    chainActivatedTimer = 0;
                    chainActivated = false;
                }
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
