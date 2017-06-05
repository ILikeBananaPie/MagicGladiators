﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Enemy:Component, ILoadable, IUpdateable
    {
        private Transform trnsfrm;
        public Vector2 velocity { get; set; }
        private Animator animator;
        //private Vector2 velocity;
        private GameObject lastHit;

        //public static Vector2 accelerationTest;
        //public static Vector2 velocityTest;
        //private float breakTest = 0.050F;

        private readonly Object thisLock = new Object();
        private UpdatePackage _updatePackage;
        public UpdatePackage updatePackage
        {
            get { lock (thisLock) { return _updatePackage; } }
            set { lock (thisLock) { _updatePackage = value; } }
        }

        public Enemy/*Number 1*/(GameObject gameObject) : base(gameObject)
        {
            updatePackage = new UpdatePackage(Vector2.Zero);
            animator = (Animator)gameObject.GetComponent("Animator");
            CreateAnimations();
        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("LightGreen", new Animation(1, 64, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Green", new Animation(1, 96, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Blue", new Animation(1, 96, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Red", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Orange", new Animation(1, 32, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Brown", new Animation(1, 0, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Yellow", new Animation(1, 64, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Purple", new Animation(1, 32, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));


            //animator.PlayAnimation("LightGreen");

        }

        public void LoadContent(ContentManager content)
        {
            trnsfrm = gameObject.GetComponent("Transform") as Transform;
        }

        public void TakeDamage(float damage)
        {
            gameObject.CurrentHealth -= damage * gameObject.DamageResistance;
        }

        public void Update()
        {
            /*
            if (!(Vector2.Distance(velocityTest, Vector2.Zero) > 0.05F && Vector2.Distance(velocityTest, Vector2.Zero) < -0.05F))
            {
                accelerationTest = breakTest * -velocityTest;
                velocityTest += accelerationTest;
                //accelerationTest = Vector2.Zero;

            }
            if (Vector2.Distance(velocityTest, Vector2.Zero) < 0.05F && Vector2.Distance(velocityTest, Vector2.Zero) > -0.05F)
            {
                velocityTest = Vector2.Zero;

            }
            */
            trnsfrm.position += velocity;
            //updatePackage.InfoUpdate(trnsfrm.position, velocity);
        }

        public void UpdateEnemyInfo(UpdatePackage package)
        {
            this.trnsfrm.position = package.position;
            this.velocity = package.velocity;
        }

        public void Hit(GameObject go)
        {
            lastHit = go;
        }

        public void UponDeath()
        {
            //if (lastHit != null)
            //{
            //    if (lastHit.GetComponent("Player") is Player)
            //    {
            //        (lastHit.GetComponent("Player") as Player).GoldReward(20);
            //    }
            //}
        }
    }
}
