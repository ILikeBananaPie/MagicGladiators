using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MagicGladiators.DIRECTION;

namespace MagicGladiators
{
    class Dummy : Component, ILoadable, IUpdateable, ICollisionEnter, ICollisionExit
    {
        private Animator animator;

        private IStrategy strategy;

        private DIRECTION direction;
        private bool testPush;
        private Vector2 testVector;
        private float testTimer;

        public Dummy(GameObject gameObject) : base(gameObject)
        {
            gameObject.Tag = "Ball";

        }

        public void LoadContent(ContentManager content)
        {
            

            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");

            direction = Front;
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, sprite));
           
            
            animator.PlayAnimation("IdleFront");

            strategy = new Idle(animator);
        }
        
        public void OnCollisionEnter(Collider other)
        {
            
        }

        public void OnCollisionExit(Collider other)
        {
            
        }

        public void Update()
        {
            if (testPush)
            {
                if (testTimer < 1)
                {
                    testTimer += GameWorld.Instance.deltaTime;
                    gameObject.transform.position += testVector / 10;
                }
                else
                {
                    testTimer = 0;
                    testPush = false;
                }
            }

            if(!(strategy is Idle))
            {
                strategy = new Idle(animator);
            }
            strategy.Execute(ref direction);
        }

        public void isPushed(Vector2 vectorBetween)
        {
            testPush = true;
            testVector = vectorBetween;
        }

      
    }
}
