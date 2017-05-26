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
    class Projectile : Component, IUpdateable, ICollisionEnter, ILoadable, ICollisionStay
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

        private bool boomerangReturn = false;
        private float boomerangTimer = 0;

        private Physics test;

        private Vector2 meteorVector;

        private float mineTimer;
        private float mineActivationTime = 5F;
        private bool deathMineActivated = false;

        private float travelDistance = 1000;
        private float distanceTravelled;

        private float Aoe = 50;

        private float abilityTimer = 0;

        private GameObject shooter;
        private Vector2 target;

        private float projectileSpeed;

        public Vector2 TestVector
        {
            get
            {
                return testVector;
            }

            set
            {
                testVector = value;
            }
        }

        public Projectile(GameObject gameObject, Vector2 position, Vector2 target, GameObject shooter) : base(gameObject)
        {
            //go = gameObject;
            this.shooter = shooter;
            projectileSpeed = GameWorld.Instance.player.ProjectileSpeed;
            originalPos = position;
            if (gameObject.Tag == "DeathMeteor")
            {
                meteorVector = (gameObject.GetComponent("Physics") as Physics).GetVector(target, position);
                meteorVector.Normalize();
            }
            if (gameObject.Tag == "DeathMine")
            {

            }


            //SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            //go.transform.position = new Vector2(position.X - spriteRenderer.Sprite.Width, position.Y - spriteRenderer.Sprite.Height);
            this.target = target;
            testVector = target - new Vector2(originalPos.X + 16, originalPos.Y + 16);
            if (gameObject.Tag == "UpNova")
            {
                testVector = new Vector2(0, 0.2f);
            }
            if (gameObject.Tag == "UpRightNova")
            {
                testVector = new Vector2(0.2f, 0.2f);
            }
            if (gameObject.Tag == "RightNova")
            {
                testVector = new Vector2(0.2f, 0);
            }
            if (gameObject.Tag == "DownRightNova")
            {
                testVector = new Vector2(0.2f, -0.2f);
            }
            if (gameObject.Tag == "DownNova")
            {
                testVector = new Vector2(0, -0.2f);
            }
            if (gameObject.Tag == "DownLeftNova")
            {
                testVector = new Vector2(-0.2f, -0.2f);
            }
            if (gameObject.Tag == "LeftNova")
            {
                testVector = new Vector2(-0.2f, 0);
            }
            if (gameObject.Tag == "UpLeftNova")
            {
                testVector = new Vector2(-0.2f, 0.2f);
            }
            testVector.Normalize();
            //target = target - originalPos;
            //target.Normalize();
            this.transform = gameObject.transform;


        }



        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("Mine", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Fireball", new Animation(1, 32, 3, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("HomingMissile", new Animation(1, 32, 2, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Drain", new Animation(1, 0, 3, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("DeathMeteor", new Animation(1, 32, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Chain", new Animation(1, 32, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Boomerang", new Animation(1, 0, 1, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            foreach (GameObject go in GameWorld.newObjects)
            {
                if (go.Tag.Contains("Nova"))
                {
                    animator.PlayAnimation("Fireball");
                    travelDistance = 200;
                }
            }
        
            if (gameObject.Tag == "Mine")
            {
                animator.PlayAnimation("Mine");
            }
            if (gameObject.Tag == "Fireball")
            {
                animator.PlayAnimation("Fireball");
                travelDistance = 200;
            }
            if (gameObject.Tag == "HomingMissile")
            {
                animator.PlayAnimation("HomingMissile");
                travelDistance = 1000;
            }
            if (gameObject.Tag == "Drain")
            {
                animator.PlayAnimation("Drain");
                travelDistance = 200;
            }
            if (gameObject.Tag == "DeathMeteor")
            {
                animator.PlayAnimation("DeathMeteor");
                travelDistance = 1500;
            }
            if (gameObject.Tag == "DeathMine")
            {
                animator.PlayAnimation("Mine");
            }
            if (gameObject.Tag == "Chain")
            {
                animator.PlayAnimation("Chain");
                travelDistance = 200;
            }
            if (gameObject.Tag == "Boomerang")
            {
                animator.PlayAnimation("Boomerang");
                travelDistance = 1000;
            }
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("ProjectileSheet");
            GameWorld.newObjects.Add(gameObject);
            //go.Tag = "Ability";

            CreateAnimations();
        }

        public void OnCollisionEnter(Collider other)
        {
            if (gameObject.Tag == "Boomerang" && other.gameObject.Tag == "Player" && boomerangReturn)
            {
                GameWorld.objectsToRemove.Add(gameObject);
            }
            if (other.gameObject.Tag == "Dummy" || other.gameObject.Tag == "Enemy" && gameObject.Tag != "DeathMine" || other.gameObject.Tag == "Pillar")
            {
                if (gameObject.Tag == "Drain")
                {
                    GameWorld.Instance.player.CurrentHealth += (GameWorld.Instance.player.GetComponent("Drain") as Drain).healing;
                }
                if (gameObject.Tag == "Chain" && other.gameObject.Tag != "Pillar")
                {
                    chainTarget = other.gameObject;
                    (chainTarget.GetComponent("Physics") as Physics).chainActivated = true;
                    chainActivated = true;
                }
                if (gameObject.Tag != "DeathMine" && other.gameObject.Tag != "Pillar")
                {
                    Push();
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
            if (gameObject.Tag == "DeathMine" && deathMineActivated && (other.gameObject.Tag == "Enemy" || other.gameObject.Tag == "Dummy"))
            {
                Push();
            }
        }

        public void Push()
        {
            foreach (Collider go in GameWorld.Instance.CircleColliders)
            {
                if (Vector2.Distance(go.gameObject.transform.position, gameObject.transform.position) < Aoe * shooter.AoeBonus)
                {
                    Vector2 vectorBetween = go.gameObject.transform.position - gameObject.transform.position;
                    vectorBetween.Normalize();
                    if (go.gameObject.Tag == "Player")
                    {
                        (go.gameObject.GetComponent("Player") as Player).isPushed(vectorBetween);
                    }
                    else if (go.gameObject.Tag == "Dummy" && gameObject.Tag != "Chain" && gameObject.Tag != "Pillar")
                    {
                        (go.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween, shooter);
                    }
                }
            }
            if (gameObject.Tag != "Chain" && gameObject.Tag != "Deflect" && gameObject.Tag != "Spellshield")
            {
                GameWorld.Instance.player.CurrentHealth += 10 * GameWorld.Instance.player.LifeSteal;
                GameWorld.objectsToRemove.Add(gameObject);
            }
        }



        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
   

            if (gameObject.Tag == "Boomerang")
            {
                if (Vector2.Distance(originalPos, gameObject.transform.position) > 100)
                {
                    boomerangReturn = true;

                }
                else (gameObject.GetComponent("Physics") as Physics).Acceleration += testVector / 10 * projectileSpeed;

                if (boomerangReturn)
                {

                    boomerangTimer += GameWorld.Instance.deltaTime;
                    Vector2 playerPos = GameWorld.Instance.player.transform.position;
                    Vector2 boomReturn = (gameObject.GetComponent("Physics") as Physics).GetVector((new Vector2(playerPos.X + 16, playerPos.Y + 16)), new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16));
                    boomReturn.Normalize();
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += boomReturn / 10 * projectileSpeed;
                    //(gameObject.GetComponent("Physics") as Physics).Velocity = (gameObject.GetComponent("Physics") as Physics).UpdateVelocity((gameObject.GetComponent("Physics") as Physics).Acceleration, (gameObject.GetComponent("Physics") as Physics).Velocity);

                }

                if (boomerangTimer >= 5)
                {
                    if (gameObject.Tag == "Boomerang")
                    {
                        //GameWorld.objectsToRemove.Add(gameObject);
                    }
                }
            }



            if (gameObject.Tag == "DeathMeteor")
            {
                (gameObject.GetComponent("Physics") as Physics).Acceleration += meteorVector / 10;
                abilityTimer += 0.001f;
            }



            if (gameObject.Tag == "DeathMine")
            {
                mineTimer += GameWorld.Instance.deltaTime;
                if (mineTimer > mineActivationTime)
                {
                    deathMineActivated = true;
                    (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Red;
                }
                //(gameObject.GetComponent("Physics") as Physics).Acceleration += meteorVector;
            }


            if (gameObject.Tag == "Fireball" || gameObject.Tag == "Drain" || gameObject.Tag == "Chain" || gameObject.Tag.Contains("Nova"))
            {
                if (gameObject.Tag == "Drain" || gameObject.Tag == "Chain")
                {
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 10) * projectileSpeed;
                }
                else (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 2) * projectileSpeed;

                if (distanceTravelled > travelDistance)
                {
                    if (gameObject.Tag == "Chain" && !chainActivated)
                    {
                        //GameWorld.objectsToRemove.Add(gameObject);
                    }
                    else if (gameObject.Tag != "Chain")
                    {
                        //GameWorld.objectsToRemove.Add(gameObject);
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
            if (gameObject.Tag == "Mine")
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
                            (gameObject.GetComponent("Physics") as Physics).Acceleration += (test / 15) * projectileSpeed;
                        }
                    }
                }
                else
                {
                    homingTimer += GameWorld.Instance.deltaTime;
                    Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(target, gameObject.transform.position);
                    test.Normalize();
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += (test / 10) * projectileSpeed;
                }
            }
            Vector2 oldPos = gameObject.transform.position;
            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;
            distanceTravelled += Vector2.Distance(oldPos, gameObject.transform.position);
            CheckDistance();

            if (abilityTimer > 2)
            {
                if (gameObject.Tag == "DeathMeteor" || gameObject.Tag.Contains("Nova"))
                {

                    //GameWorld.objectsToRemove.Add(gameObject);
                }
            }
        }

        public void CheckDistance()
        {
            if (distanceTravelled > travelDistance)
            {
                if (gameObject.Tag != "Chain")
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                }
                if (gameObject.Tag == "Chain" && !chainActivated)
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                }
            }
        }
    }
}
