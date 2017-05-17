using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class Projectile : Component, IUpdateable, ICollisionEnter, ILoadable
    {
        private Animator animator; //test 

        private IStrategy strategy;

        private DIRECTION direction;

        //private GameObject go;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;

        private bool chainActivated;

        private float homingTimer;
        private float distance;
        private Vector2 bestTarget;

        private GameObject chainTarget;
        private float chainTimer;

        private Physics test;

        private Vector2 meteorVector;

        private float mineTimer;

        private Vector2 target;

        public Projectile(GameObject gameObject, Vector2 position, Vector2 target) : base(gameObject)
        {
            //go = gameObject;
            originalPos = position;
            if (gameObject.Tag == "DeathMeteor")
            {
                meteorVector = (gameObject.GetComponent("Physics") as Physics).GetVector(target, position);
                meteorVector.Normalize();
            }

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
            GameWorld.newObjects.Add(gameObject);
            //go.Tag = "Ability";

            CreateAnimations();
        }

        public void OnCollisionEnter(Collider other)
        {

            if (other.gameObject.Tag != "Player" && other.gameObject.Tag != "Map")
            {
                if (gameObject.Tag == "Drain")
                {
                    GameWorld.Instance.player.CurrentHealth += (GameWorld.Instance.player.GetComponent("Drain") as Drain).healing;
                }
                if (gameObject.Tag == "Chain")
                {
                    chainTarget = other.gameObject;
                    (chainTarget.GetComponent("Physics") as Physics).chainActivated = true;
                    chainActivated = true;
                }
                foreach (Collider go in GameWorld.Instance.CircleColliders)
                {
                    if (Vector2.Distance(go.gameObject.transform.position, gameObject.transform.position) < 100)
                    {
                        //plz don't delete me
                        Vector2 vectorBetween = go.gameObject.transform.position - gameObject.transform.position;
                        vectorBetween.Normalize();
                        if (go.gameObject.Tag == "Player")
                        {
                            // (go.gameObject.GetComponent("Player") as Player).isPushed(vectorBetween);
                        }
                        else if (go.gameObject.Tag == "Dummy" && gameObject.Tag != "Chain")
                        {
                            (go.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween);
                        }
                    }
                }
                if (gameObject.Tag != "Chain")
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                }
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
            KeyboardState keyState = Keyboard.GetState();


            if (gameObject.Tag == "DeathMeteor")
            {
                (gameObject.GetComponent("Physics") as Physics).Acceleration += meteorVector / 10;
            }

            if (gameObject.Tag == "Fireball" || gameObject.Tag == "Drain" || gameObject.Tag == "Chain")
            {
                if (gameObject.Tag == "Drain" || gameObject.Tag == "Chain")
                {
                    gameObject.transform.position += testVector * 2;
                }
                else gameObject.transform.position += testVector * 5;

                animator.PlayAnimation("Shoot");

                if (Vector2.Distance(originalPos, gameObject.transform.position) > 300)
                {
                    if (gameObject.Tag == "Chain" && !chainActivated)
                    {
                        GameWorld.objectsToRemove.Add(gameObject);
                    }
                    else if (gameObject.Tag != "Chain")
                    {
                        GameWorld.objectsToRemove.Add(gameObject);
                    }

                }
            }

            if (chainActivated)
            {
                chainTimer += GameWorld.Instance.deltaTime;
                gameObject.transform.position = chainTarget.transform.position;
                Vector2 pull = (gameObject.GetComponent("Physics") as Physics).GetVector(GameWorld.Instance.player.transform.position, chainTarget.transform.position);
                pull.Normalize();
                (GameWorld.Instance.player.GetComponent("Physics") as Physics).Acceleration -= pull / 10;
                if (chainTarget.Tag == "Dummy" || chainTarget.Tag == "Enemy")
                {
                    (chainTarget.GetComponent("Physics") as Physics).Acceleration += pull / 10;
                }
                if (keyState.IsKeyDown(Keys.T) || chainTimer > 2 || Vector2.Distance(chainTarget.transform.position, GameWorld.Instance.player.transform.position) < 20)
                {
                    chainActivated = false;
                    (chainTarget.GetComponent("Physics") as Physics).chainDeactivated = true;
                    (chainTarget.GetComponent("Physics") as Physics).chainActivated = false;
                    GameWorld.objectsToRemove.Add(gameObject);
                }
            }
            if(gameObject.Tag == "Mine")
            {
             
            }

            if (gameObject.Tag == "HomingMissile")
            {
                if (homingTimer > 1)
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (Vector2.Distance(gameObject.transform.position, go.transform.position) < 10000 && (go.Tag == "Dummy" || go.Tag == "Enemy"))
                        {
                            distance = Vector2.Distance(gameObject.transform.position, go.transform.position);
                            bestTarget = go.transform.position;
                            foreach (GameObject item in GameWorld.gameObjects)
                            {
                                if (Vector2.Distance(gameObject.transform.position, item.transform.position) < distance && (item.Tag == "Dummy" || item.Tag == "Enemy"))
                                {
                                    distance = Vector2.Distance(gameObject.transform.position, item.transform.position);
                                    bestTarget = item.transform.position;
                                }
                            }

                            Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(bestTarget, gameObject.transform.position);
                            test.Normalize();
                            (gameObject.GetComponent("Physics") as Physics).Acceleration += test / 15;
                        }
                    }
                }
                else
                {
                    homingTimer += GameWorld.Instance.deltaTime;
                    Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(target, gameObject.transform.position);
                    test.Normalize();
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += test / 10;
                }
            }

            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;
        }

    }
}
