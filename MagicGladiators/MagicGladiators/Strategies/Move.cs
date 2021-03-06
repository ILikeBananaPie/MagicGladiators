﻿using Microsoft.Xna.Framework;
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

        private Physics physics;

        private GameObject player;

        private Vector2 testVector;

         public static DIRECTION direction
        { get; private set; }


        public Move(Transform transform, Animator animator)
        {
            this.transform = transform;
            this.player = transform.gameObject;
            this.animator = animator;
            this.physics = (transform.gameObject.GetComponent("Physics") as Physics);
        }

        public void Execute(ref DIRECTION currentDirection)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector2 translation = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
                //physics.Acceleration += new Vector2(0, -0.25F * player.Speed);
                testVector = new Vector2(-10, -10);
                testVector.Normalize();
                testVector = new Vector2(0, testVector.Y);
                currentDirection = Back;
            }
            if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
               
                testVector = new Vector2(-10, -10);
                testVector.Normalize();
                testVector = new Vector2(testVector.X, 0);
              
            }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.D))
            {
                
                testVector = new Vector2(10, 10);
                testVector.Normalize();
                testVector = new Vector2(0, testVector.Y);
              
            }
            if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.W))
            {
               
                testVector = new Vector2(10, 10);
                testVector.Normalize();
                testVector = new Vector2(testVector.X, 0);
              
            }
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
               
                testVector = new Vector2(-10, -10);
                testVector.Normalize();
               
            }
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.D) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.A))
            {
                
                testVector = new Vector2(10, -10);
                testVector.Normalize();
               
            }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.A) && keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.D))
            {
               
                testVector = new Vector2(-10, 10);
                testVector.Normalize();
              
            }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.D) && keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.A))
            {
                
                testVector = new Vector2(10, 10);
                testVector.Normalize();
            
            }

            Player.testSpeed = physics.Acceleration;

            if ((transform.gameObject.GetComponent("Physics") as Physics).chainActivated || (transform.gameObject.GetComponent("Physics") as Physics).chainDeactivated)
            {
                //do nothing
                //physics.Acceleration += (testVector / 5 * player.Speed) / 2;
            } else if (GameWorld.gameState == GameState.ingame)
            {
                physics.Acceleration += testVector / 5 * player.Speed;
            }



            direction = currentDirection;

           // animator.PlayAnimation("Walk" + currentDirection);
        }
    }
}
