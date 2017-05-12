using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators
{
    class Projectile : Component, IUpdateable, ICollisionEnter, ILoadable
    {
        private Animator animator; //test 
         
        private IStrategy strategy;

        private DIRECTION direction;

        private GameObject go;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;

        private Vector2 target;

        public Projectile(GameObject gameObject, Vector2 position, Vector2 target) : base(gameObject)
        {
            go = gameObject;
            originalPos = position;
            //SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            //go.transform.position = new Vector2(position.X - spriteRenderer.Sprite.Width, position.Y - spriteRenderer.Sprite.Height);
            this.target = target;
            testVector = target - originalPos;
            testVector.Normalize();
            //target = target - originalPos;
            //target.Normalize();
            this.transform = gameObject.transform;
        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");


            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleBack", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleLeft", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleRight", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkFront", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkBack", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkLeft", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkRight", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Shoot", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));

            animator.PlayAnimation("Shoot");

            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");
            GameWorld.newObjects.Add(go);
            go.Tag = "Ability";

            CreateAnimations();
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag != "Player")
            {
                foreach (Collider go in GameWorld.Instance.CircleColliders)
                {
                    if (Vector2.Distance(go.gameObject.transform.position, gameObject.transform.position) < 100)
                    {
                        //plz don't delete me
                        Vector2 vectorBetween = go.gameObject.transform.position - gameObject.transform.position;
                        vectorBetween.Normalize();
                        if (go.gameObject.Tag == "Player")
                        {
                            (go.gameObject.GetComponent("Player") as Player).isPushed(vectorBetween);
                        }
                        else if (go.gameObject.Tag == "Dummy")
                        {
                            (go.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween);
                        }
                    }
                }
                GameWorld.objectsToRemove.Add(gameObject);
            }
         /*   
            if (other.gameObject.Tag == "Dummy")
            {
                GameWorld.objectsToRemove.Add(gameObject);
                Vector2 test = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;

                Vector2 vectorBetween = other.gameObject.transform.position - test;
                Vector2 playerPushVector = test - other.gameObject.transform.position;
                playerPushVector.Normalize();
                vectorBetween.Normalize();
                (other.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween);
            }
           */ 
        }

        public void Update()
        {
            go.transform.position += testVector * 5;
            animator.PlayAnimation("Shoot");

            if (Vector2.Distance(originalPos, gameObject.transform.position) > 300)
            {
                GameWorld.objectsToRemove.Add(gameObject);
            }
        }
    }
}
