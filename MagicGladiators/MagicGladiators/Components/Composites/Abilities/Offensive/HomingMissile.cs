using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicGladiators.Components.Composites.Abilities;

namespace MagicGladiators
{
    class HomingMissile : OffensiveAbility, ILoadable, IUpdateable, ICollisionEnter
    {
        private Animator animator;
        private IStrategy strategy;
        private DIRECTION direction;

        private GameObject go;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;
        private Vector2 target;
        private GameObject player;
        private TimeSpan timer;
       
        
        



        public HomingMissile(GameObject gameObject, Vector2 position, Vector2 target) : base(gameObject)
        {
            go = gameObject;
            originalPos = position;
            this.target = target;

            testVector = target - originalPos;
            testVector.Normalize();
            this.transform = gameObject.transform;
            timer = new TimeSpan(0);

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

        public override void LoadContent(ContentManager content)
        {
            player = GameWorld.Instance.FindGameObjectWithTag("Enemy");
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");
            GameWorld.newObjects.Add(go);
            go.Tag = "Ability";

            CreateAnimations();
        }

        public override void Update()
        {
             
            go.transform.position += testVector * 5;
            animator.PlayAnimation("Shoot");        

           if (Vector2.Distance(originalPos, gameObject.transform.position) > 200 && !(strategy is FollowTarget))
            {
                strategy = new FollowTarget(gameObject.transform, gameObject.transform, animator);
                
               if( timer.Milliseconds == 20)
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                }
                
            }

            //else if (Vector2.Distance(originalPos, gameObject.transform.position) > 600)
            //{

            //    GameWorld.objectsToRemove.Add(gameObject);

            //}
            strategy.Execute(ref direction);
        
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag != "Player")
            {
                foreach (Collider go in GameWorld.Instance.CircleColliders)
                {
                    if (Vector2.Distance(go.gameObject.transform.position, gameObject.transform.position) < 100)
                    {
                       
                        Vector2 vectorBetween = go.gameObject.transform.position - gameObject.transform.position;
                        vectorBetween.Normalize();
                        //if (go.gameObject.Tag == "Player")
                        //{
                        //    (go.gameObject.GetComponent("Player") as Player).isPushed(vectorBetween);
                        //}
                        //else 
                        if (go.gameObject.Tag == "Dummy")
                        {
                            (go.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween);
                        }
                       
                    }
                }
                GameWorld.objectsToRemove.Add(gameObject);
            }
        }
    }
}
