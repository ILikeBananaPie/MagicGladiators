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
                Player.accelerationTest += new Vector2(0, -0.25F);
                currentDirection = Back;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                Player.accelerationTest += new Vector2(-0.25F, 0);

                currentDirection = Left;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector2(0, 1);
                Player.accelerationTest += new Vector2(0, 0.25F);

                currentDirection = Front;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 0);
                Player.accelerationTest += new Vector2(0.25F, 0);

                currentDirection = Right;
            }
            
          
            
            direction = currentDirection;
            //transform.position += 
            //gameObject.transform.position += velocityTest;

            //transform.Translate(translation * movementSpeed * GameWorld.Instance.deltaTime);

             animator.PlayAnimation("Walk" + currentDirection);


         }
         

/*
        private float movementSpeed = 5;

        private float maxSpeed = 30;

        private Transform transform;

        private Vector2 velocity;

        private Animator animator;

        private Vector2 acceleration;

        private Vector2 velMax;
        private Vector2 velMin;


        public static DIRECTION direction { get; private set; }


        public Move(Transform transform, Animator animator)
        {
            this.transform = transform;
            this.animator = animator;
        }



        public void Execute(ref DIRECTION currentDirection)
        {
            KeyboardState keyState = Keyboard.GetState();


            Transform.playerPosition += velocity;

            if (keyState.IsKeyDown(Keys.W))
            {

                Transform.playerPosition.Y += velocity.Y * GameWorld.Instance.deltaTime;
                velocity.Y += acceleration.Y * GameWorld.Instance.deltaTime;
                velocity.Y++;

                currentDirection = Back;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                Transform.playerPosition.X += velocity.X * GameWorld.Instance.deltaTime;
                velocity.X -= acceleration.X * GameWorld.Instance.deltaTime;
                velocity.X--;

                currentDirection = Left;
            }
            if (keyState.IsKeyDown(Keys.S))
            {

                Transform.playerPosition.Y += velocity.Y * GameWorld.Instance.deltaTime;
                velocity.Y -= acceleration.Y * GameWorld.Instance.deltaTime;
                velocity.Y--;

                currentDirection = Front;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                Transform.playerPosition.X += velocity.X * GameWorld.Instance.deltaTime;
                velocity.X += acceleration.X * GameWorld.Instance.deltaTime;
                velocity.X++;

                currentDirection = Right;

                direction = currentDirection;
                transform.Translate(velocity * movementSpeed * GameWorld.Instance.deltaTime);

                animator.PlayAnimation("Walk" + currentDirection);

            }
        }*/
    }
    
}
