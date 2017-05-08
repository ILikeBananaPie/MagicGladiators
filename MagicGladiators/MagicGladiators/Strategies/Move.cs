using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MagicGladiators.DIRECTION;

namespace MagicGladiators
{
    class Move : IStrategy
    {
        private float movementSpeed = 200;

        private Transform transform;

        private Animator animator;

        public static DIRECTION direction { get; private set; }


        public Move(Transform transform, Animator animator)
        {
            this.transform = transform;
            this.animator = animator;
        }

        public void Execute(ref DIRECTION currentDirection)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector2 translation = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector2(0, -1);
                Player.accelerationTest += new Vector2(0, -0.5F);
                currentDirection = Back;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                Player.accelerationTest += new Vector2(-0.5F, 0);

                currentDirection = Left;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector2(0, 1);
                Player.accelerationTest += new Vector2(0, 0.5F);

                currentDirection = Front;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 0);
                Player.accelerationTest += new Vector2(0.5F, 0);

                currentDirection = Right;
            }
            /*
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, -1);
                Vector2 test = new Vector2(-0.5F, -0.5F);
                test.Normalize();
                Player.accelerationTest += test;
            }
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, -1);
                Vector2 test = new Vector2(0.5F, -0.5F);
                test.Normalize();
                Player.accelerationTest += test;
            }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 1);
                Vector2 test = new Vector2(-0.5F, 0.5F);
                test.Normalize();
                Player.accelerationTest += test;
            }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 1);
                Vector2 test = new Vector2(0.5F, 0.5F);
                test.Normalize();
                Player.accelerationTest += test;
            }
            */
            direction = currentDirection;
            //transform.position += 
            //gameObject.transform.position += velocityTest;

            //transform.Translate(translation * movementSpeed * GameWorld.Instance.deltaTime);

            animator.PlayAnimation("Walk" + currentDirection);


        }

        /*

        private float movementSpeed = 200;

        private float maxSpeed = 1;

        private Transform transform;

        private Vector2 velocity;

        private Animator animator;

        private Vector2 acceleration;

        public static DIRECTION direction { get; private set; }


        public Move(Transform transform, Animator animator)
        {
            this.transform = transform;
            this.animator = animator;
        }

        public void accelerate(Vector2 acceleration)
        {

            if (this.velocity.X > this.maxSpeed)
                this.velocity.X = this.maxSpeed;
            if (this.velocity.X < -1 * this.maxSpeed)
                this.velocity.X = -1 * this.maxSpeed; // the same for y

            if (this.velocity.Y > this.maxSpeed)
                this.velocity.Y = this.maxSpeed;
            if (this.velocity.Y < -1 * this.maxSpeed)
                this.velocity.Y = -1 * this.maxSpeed; // the same for y
        }

        public void Execute(ref DIRECTION currentDirection)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector2 velocity = Transform.playerPosition;

            if (keyState.IsKeyDown(Keys.W))
            {
                // translation.Y = 7;
                Transform.playerPosition += velocity * GameWorld.Instance.deltaTime;
                velocity -= acceleration * GameWorld.Instance.deltaTime;
                acceleration.Y += 0.5f;
                currentDirection = Back;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                Transform.playerPosition += velocity * GameWorld.Instance.deltaTime;
                velocity -= acceleration * GameWorld.Instance.deltaTime;
                acceleration.X -= 0.5f;
                currentDirection = Left;
            }
            if (keyState.IsKeyDown(Keys.S))
            {

                Transform.playerPosition += velocity * GameWorld.Instance.deltaTime;
                velocity += acceleration * GameWorld.Instance.deltaTime;
                acceleration.Y -= 0.5f;
                currentDirection = Front;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                Transform.playerPosition += velocity * GameWorld.Instance.deltaTime;
                velocity += acceleration * GameWorld.Instance.deltaTime;
                acceleration.X += 0.5f;
                currentDirection = Right;
            }
     
          */
        }

}
