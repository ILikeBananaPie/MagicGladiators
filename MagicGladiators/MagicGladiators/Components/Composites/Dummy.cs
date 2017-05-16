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
        private int testSpeed = 10;

        public float Damage { get; private set; } = 5F;
        public bool isAlive { get; set; } = true;


        public Dummy(GameObject gameObject) : base(gameObject)
        {
            gameObject.Tag = "Dummy";
            gameObject.MaxHealth = 100;
            gameObject.CurrentHealth = gameObject.MaxHealth;

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
            if (other.gameObject.Tag == "Player")
            {
                (other.gameObject.GetComponent("Player") as Player).TakeDamage(Damage);
            }
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
                    gameObject.transform.position += testVector * testSpeed;
                    if (testSpeed > 0)
                    {
                        testSpeed--;
                    }
                }
                else
                {
                    testTimer = 0;
                    testPush = false;
                    testSpeed = 10;
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
