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
        private GameObject lastHit;

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

            Texture2D sprite = content.Load<Texture2D>("Dummy");

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
            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;

            if (testPush)
            {
                (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector * 5) * gameObject.KnockBackResistance;
                if (testTimer < 0.0025F)
                {
                    testTimer += GameWorld.Instance.deltaTime;
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

        public void isPushed(Vector2 vectorBetween, GameObject go)
        {
            if (go.GetComponent("Player") is Player)
            {
                (go.GetComponent("Player") as Player).GoldReward(3);
            }
            lastHit = go;
            testPush = true;
            testVector = vectorBetween;
        }

        public void UponDeath()
        {
            if (lastHit != null)
            {
                if (lastHit.GetComponent("Player") is Player)
                {
                    (lastHit.GetComponent("Player") as Player).GoldReward(20);
                }
            }
        }
    }
}
