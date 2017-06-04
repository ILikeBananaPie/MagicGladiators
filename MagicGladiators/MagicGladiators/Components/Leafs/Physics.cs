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
        private IceField iceField;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float breakFactor { get; set; } = 0.050F;
        public bool chainDeactivated { get; set; } = false;
        public bool chainActivated { get; set; } = false;
        private float timer;
        private Vector2 breaking = new Vector2(0.05F, 0.05F);
        private float testX;
        private float tesY;
        private Vector2 normalAcc;
        private Vector2 changedAcc;
        private bool correctVel = false;
        public bool Ice { get; set; } = false;
        private Vector2 MaxSpeedPositive = new Vector2(2F, 2F);
        private Vector2 MaxSpeedNegative = new Vector2(-2F, -2F);

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
                        normalAcc = 0.05F * -velocity;
                        Acceleration = 0.001F * -velocity;
                        //Acceleration = new Vector2(Acceleration.X + Acceleration.X * 10F, Acceleration.Y);

                    }
                    else
                    {
                        normalAcc = 0.05F * -velocity;
                        Acceleration = 0.004F * -velocity;
                    }
                }
                else if (chainActivated)
                {
                    normalAcc = 0.05F * -velocity;
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
                    else
                    {
                        normalAcc = 0.05F * -velocity;
                        Acceleration = 0.001F * -velocity;
                    }
                }
                else
                {
                    Acceleration = breakFactor * -velocity;
                    changedAcc = breakFactor * -velocity;
                    normalAcc = 0.05F * -velocity;
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
                    normalAcc = Vector2.Zero;
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
            Vector2 temp = Velocity;
            if (gameObject.Tag == "Player" || gameObject.Tag == "Enemy" || gameObject.Tag == "Dummy")
            {
                if (temp.X > MaxSpeedPositive.X && temp.X > 0)
                {
                    Velocity = new Vector2(MaxSpeedPositive.X, Velocity.Y);
                }
                if (temp.X < MaxSpeedNegative.X && temp.X < 0)
                {
                    Velocity = new Vector2(MaxSpeedNegative.X, Velocity.Y);
                }
                if (temp.Y > MaxSpeedPositive.Y && temp.Y > 0)
                {
                    Velocity = new Vector2(Velocity.X, MaxSpeedPositive.Y);
                }
                if (temp.Y < MaxSpeedNegative.Y && temp.Y < 0)
                {
                    Velocity = new Vector2(Velocity.X, MaxSpeedNegative.Y);
                }
            }
            return Acceleration;
        }

        public Vector2 UpdateVelocity(Vector2 acceleration, Vector2 velocity)
        {

            //Vector2 testAcc2 = velocity + this.testAcc2;

            //Vector2 testAcc = velocity + this.testAcc;

            /*
            if (this.normalAcc != Vector2.Zero && changedAcc != normalAcc && (gameObject.Tag == "Player" || gameObject.Tag == "Enemy" || gameObject.Tag == "Dummy"))
            {
                float tempX = changedAcc.X;
                float tempY = changedAcc.Y;
                if (changedAcc.X > normalAcc.X && changedAcc.X > 0)
                {
                    tempX = this.normalAcc.X;
                }
                if (changedAcc.Y > normalAcc.Y && changedAcc.Y > 0)
                {
                    tempY = this.normalAcc.Y;
                }
                if (changedAcc.X < normalAcc.X && changedAcc.X < 0)
                {
                    tempX = this.normalAcc.X;
                }
                if (changedAcc.Y < normalAcc.Y && changedAcc.Y < 0)
                {
                    tempY = this.normalAcc.Y;
                }

                return velocity += new Vector2(tempX, tempY);
            }
            else
            {
                return velocity += acceleration;
            }
            */
            if (Ice)
            {
                return velocity = acceleration;
            }
            else
            {
                return velocity += acceleration;
            }

        }

        public void Update()
        {

            Velocity = UpdateVelocity(Acceleration, Velocity);
            Acceleration = physicsBreak(breakFactor, Velocity);


        }
    }
}
