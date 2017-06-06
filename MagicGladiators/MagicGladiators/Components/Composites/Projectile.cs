using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MagicGladiators
{
    class Projectile : Component, IUpdateable, ICollisionEnter, ILoadable, ICollisionStay
    {
        private Animator animator; //test 

        private IStrategy strategy;

        private DIRECTION direction;
        private GameObject targetHit;

        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;

        public static bool chainActivated;
        private bool getScore = false;

        private float homingTimer;
        private float distance;
        private Vector2 bestTarget;

        public GameObject chainTarget { get; set; }
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

        public bool deflectTest { get; set; } = false;
        public string deflectName { get; set; } = "";
        private float Aoe = 50;

        private float abilityTimer = 0;

        private GameObject shooter;

        private Vector2 target;

        public static List<string> testName { get; set; } = new List<string>();

        public static List<string> testID { get; set; } = new List<string>();

        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };

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


            this.target = target;
            if (gameObject.Tag.Contains("Firewave"))
            {
                testVector = target;
            }
            else testVector = target - new Vector2(originalPos.X + 16, originalPos.Y + 16);

            if (gameObject.Tag == "UpNova")
            {
                testVector = new Vector2(0, -0.2f);
            }
            if (gameObject.Tag == "UpRightNova")
            {
                testVector = new Vector2(0.2f, -0.2f);
            }
            if (gameObject.Tag == "RightNova")
            {
                testVector = new Vector2(0.2f, 0);
            }
            if (gameObject.Tag == "DownRightNova")
            {
                testVector = new Vector2(0.2f, 0.2f);
            }
            if (gameObject.Tag == "DownNova")
            {
                testVector = new Vector2(0, 0.2f);
            }
            if (gameObject.Tag == "DownLeftNova")
            {
                testVector = new Vector2(-0.2f, 0.2f);
            }
            if (gameObject.Tag == "LeftNova")
            {
                testVector = new Vector2(-0.2f, 0);
            }
            if (gameObject.Tag == "UpLeftNova")
            {
                testVector = new Vector2(-0.2f, -0.2f);
            }
            testVector.Normalize();

            this.transform = gameObject.transform;
            (gameObject.GetComponent("Physics") as Physics).Velocity = testVector;

        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("Mine", new Animation(1, 0, 3, 32, 32, 8, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Fireball", new Animation(3, 128, 0, 32, 32, 8, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("HomingMissile", new Animation(1, 128, 3, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Drain", new Animation(4, 32, 0, 32, 32, 8, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("DeathMeteor", new Animation(3, 128, 0, 32, 32, 5, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Chain", new Animation(2, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Boomerang", new Animation(4, 96, 0, 32, 32, 15, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("GravityWell", new Animation(4, 64, 0, 32, 32, 7, Vector2.Zero, spriteRenderer.Sprite));

            animator.CreateAnimation("FirewaveTopBottom", new Animation(3, 200, 0, 200, 100, 1, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("FirewaveLeftRight", new Animation(3, 0, 0, 100, 200, 1, Vector2.Zero, spriteRenderer.Sprite));

            foreach (GameObject go in GameWorld.newObjects)
            {
                if (go.Tag.Contains("Nova"))
                {
                    animator.PlayAnimation("Fireball");
                    travelDistance = 200;
                }
            }

            if (gameObject.Tag.Contains("Mine"))
            {
                animator.PlayAnimation("Mine");
            }
            if (gameObject.Tag.Contains("Fireball"))
            {
                animator.PlayAnimation("Fireball");
                travelDistance = 200;
            }
            if (gameObject.Tag.Contains("HomingMissile"))
            {
                animator.PlayAnimation("HomingMissile");
                travelDistance = 2800;
            }
            if (gameObject.Tag.Contains("Drain"))
            {
                animator.PlayAnimation("Drain");
                travelDistance = 200;
            }
            if (gameObject.Tag == "DeathMeteor")
            {

                animator.PlayAnimation("Fireball");
                travelDistance = 1500;
            }
            if (gameObject.Tag == "DeathMine")
            {
                animator.PlayAnimation("Mine");
            }
            if (gameObject.Tag.Contains("Chain"))
            {
                animator.PlayAnimation("Chain");
                travelDistance = 200;
            }
            if (gameObject.Tag == "GravityWell")
            {
                animator.PlayAnimation("GravityWell");
                travelDistance = 400;
            }
            if (gameObject.Tag == "Boomerang")
            {
                animator.PlayAnimation("Boomerang");
                travelDistance = 4000;
            }
            if (gameObject.Tag.Contains("Firewave"))
            {
                if (gameObject.Tag.Contains("Top"))
                {
                    animator.PlayAnimation("FirewaveTopBottom");
                }
                else animator.PlayAnimation("FirewaveLeftRight");
                travelDistance = 1380;
            }
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite;
            if (gameObject.Tag.Contains("Firewave"))
            {
                sprite = content.Load<Texture2D>("Firewave");
            }
            else sprite = content.Load<Texture2D>("ProjectileSheet");

            GameWorld.newObjects.Add(gameObject);


            CreateAnimations();
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag.Contains("Critter"))
            {
                GameWorld.objectsToRemove.Add(gameObject);
                if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                {
                    GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                }
                return;
            }
            if (gameObject.Tag.Contains("Boomerang") && other.gameObject.Tag == "Player" && boomerangReturn)
            {
                GameWorld.objectsToRemove.Add(gameObject);
                if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                {
                    GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                }
            }
            if ((other.gameObject.Tag == "Dummy" || other.gameObject.Tag == "Enemy" || other.gameObject.Tag == "Pillar" || other.gameObject.Tag.Contains("Clone") || other.gameObject.Tag.Contains("Critter")) && gameObject.Tag != "DeathMine" && other.gameObject.Tag != "Deflect")
            {
                if (gameObject.Tag == "Drain")
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Player" && gameObject.Id == go.Id)
                        {
                            go.CurrentHealth += (GameWorld.Instance.player.GetComponent("Drain") as Drain).healing;
                        }
                    }
                }
                if (gameObject.Tag == "Chain" && gameObject.Id == GameWorld.Instance.player.Id && !chainActivated)
                {
                    chainTarget = other.gameObject;

                    chainActivated = true;
                }
                if (gameObject.Tag != "DeathMine" && gameObject.Tag != "Chain" && other.gameObject.Tag != "Deflect" && other.gameObject.Tag != "Spellshield")
                {
                    GameObject player = new GameObject();
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Player")
                        {
                            player = go;
                        }
                    }

                    if ((gameObject.Id != other.gameObject.Id || other.gameObject.Tag == "Dummy") && !other.gameObject.Tag.Contains("Critter") && !other.gameObject.Tag.Contains("Firewave"))
                    {
                        if (other.gameObject.Tag != "Player")
                        {
                            targetHit = other.gameObject;
                            if (!GameWorld.buyPhase)
                            {
                                string ability;
                                if (gameObject.Tag.Contains("Nova"))
                                {
                                    ability = "Nova";
                                }
                                else if (gameObject.Tag.Contains("Fireball"))
                                {
                                    ability = "Fireball";
                                }
                                else ability = gameObject.Tag;
                                if (!getScore && gameObject.Id == GameWorld.Instance.player.Id && !gameObject.Tag.Contains("Firewave") && gameObject.Tag != "DeathMeteor" && gameObject.Tag != "DeathMine")
                                {
                                    getScore = true;
                                    (GameWorld.Instance.player.GetComponent("Player") as Player).GoldReward(4);
                                    GameWorld.Instance.player.DamageDone += (GameWorld.Instance.player.GetComponent(ability) as Ability).damage * other.gameObject.DamageResistance;
                                    GameWorld.Instance.player.TotalScore += (int)((GameWorld.Instance.player.GetComponent(ability) as Ability).damage * other.gameObject.DamageResistance);
                                    if (GameWorld.Instance.client != null)
                                    {
                                        GameWorld.Instance.client.SendScore(GameWorld.Instance.player.Id, GameWorld.Instance.player.kills, GameWorld.Instance.player.DamageDone, GameWorld.Instance.player.TotalScore);
                                    }
                                }
                            }
                            Push();
                        }
                    }
                }
            }

            if (other.gameObject.Tag == "Spellshield" && !gameObject.Tag.Contains("Firewave") && gameObject.Tag != "DeathMine" && gameObject.Tag != "DeathMeteor")
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag == "Spellshield" && go.Id != gameObject.Id)
                    {
                        GameWorld.objectsToRemove.Add(gameObject);
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                        }
                    }
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
            if (gameObject.Tag == "DeathMine" && deathMineActivated && (other.gameObject.Tag == "Enemy" || other.gameObject.Tag == "Dummy") && other.gameObject.Tag != "Deflect")
            {
                if (other.gameObject.Tag != "Player")
                {
                    targetHit = other.gameObject;
                    Push();
                }
            }
        }

        public void Push()
        {
            foreach (GameObject go in GameWorld.gameObjects)
            {
                if (Vector2.Distance(new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16), new Vector2(targetHit.transform.position.X + 16, targetHit.transform.position.Y + 16)) < Aoe * shooter.AoeBonus && (go.Tag == "Enemy" || go.Tag == "Player" || go.Tag == "Dummy"))
                {
                    Vector2 vectorBetween;
                    if (targetHit.transform.position == go.transform.position)
                    {
                        vectorBetween = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16) - new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                        vectorBetween.Normalize();
                    }
                    else
                    {
                        vectorBetween = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16) - new Vector2(targetHit.transform.position.X + 16, targetHit.transform.position.Y + 16);
                        vectorBetween.Normalize();
                    }


                    if (go.Tag == "Player")
                    {
                        (go.GetComponent("Player") as Player).isPushed(vectorBetween);
                    }
                    else if (go.Tag == "Enemy")
                    {
                        if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                        {
                            vectorBetween = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16) - new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                            vectorBetween.Normalize();
                            string ability;
                            if (gameObject.Tag.Contains("Nova"))
                            {
                                ability = "Nova";
                            }
                            else if (gameObject.Tag.Contains("Fireball"))
                            {
                                ability = "Fireball";
                            }
                            else ability = gameObject.Tag;
                            if (gameObject.Tag == "DeathMeteor" || gameObject.Tag == "DeathMine" || gameObject.Tag == "RollingMeteor" || gameObject.Tag.Contains("Firewave"))
                            {
                                GameWorld.Instance.client.SendPush(go.Id, vectorBetween, 0);
                            }
                            else
                            {
                                if (gameObject.Tag == "HomingMissile")
                                {
                                    Debug.WriteLine("Homing pushing" + DateTime.Now);
                                }
                                GameWorld.Instance.client.SendPush(go.Id, vectorBetween, (GameWorld.Instance.player.GetComponent(ability) as Ability).damage);
                                if (!GameWorld.buyPhase)
                                {

                                    //if (GameWorld.Instance.client != null)
                                    //{
                                    //    GameWorld.Instance.client.SendScore(GameWorld.Instance.player.Id, GameWorld.Instance.player.kills, GameWorld.Instance.player.DamageDone, GameWorld.Instance.player.TotalScore);
                                    //}
                                }
                            }
                        }
                    }
                    else if (go.Tag == "Dummy" && (!gameObject.Tag.Contains("Chain") && gameObject.Tag != "Pillar"))
                    {
                        (go.GetComponent("Dummy") as Dummy).isPushed(vectorBetween, shooter);
                    }
                    else if (go.Tag == "Enemy" && gameObject.Tag != "Chain" && gameObject.Tag != "Pillar")
                    {
                        (go.GetComponent("Enemy") as Enemy).Hit(GameWorld.Instance.player);
                    }
                }
            }
            if (!gameObject.Tag.Contains("Chain") && !gameObject.Tag.Contains("Deflect") && !gameObject.Tag.Contains("SpellShield") && !gameObject.Tag.Contains("Firewave"))
            {
                GameWorld.Instance.player.CurrentHealth += 10 * GameWorld.Instance.player.LifeSteal;
                GameWorld.objectsToRemove.Add(gameObject);
                if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                {
                    GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                }
            }
        }

        public void FirewavePush(GameObject go, string id)
        {
            Vector2 vector = target;
            Circle circle = (go.GetComponent("Collider") as Collider).CircleCollisionBox;
            if (gameObject.Tag == "FirewaveTopBottom")
            {
                if (circle.Center.X < gameObject.transform.position.X)
                {
                    vector = new Vector2(-1, 0);
                }
                if (circle.Center.X > gameObject.transform.position.X + 200)
                {
                    vector = new Vector2(1, 0);
                }

                if (circle.Center.Y < gameObject.transform.position.Y)
                {
                    vector = new Vector2(0, -1);
                }
                if (circle.Center.Y > gameObject.transform.position.Y + 100)
                {
                    vector = new Vector2(0, 1);
                }
            }
            if (gameObject.Tag == "FirewaveLeftRight")
            {
                if (circle.Center.Y < gameObject.transform.position.Y)
                {
                    vector = new Vector2(0, -1);
                }
                if (circle.Center.Y > gameObject.transform.position.Y + 200)
                {
                    vector = new Vector2(0, 1);
                }

                if (circle.Center.X < gameObject.transform.position.X)
                {
                    vector = new Vector2(-1, 0);
                }
                if (circle.Center.X > gameObject.transform.position.X + 100)
                {
                    vector = new Vector2(1, 0);
                }

            }
            (go.GetComponent("Physics") as Physics).Acceleration += vector;
        }

        public bool intersects(Circle cir, Rectangle rec)
        {
            Vector2 circleDistance;
            float cornorDistance;
            circleDistance.X = Math.Abs(cir.Center.X - (rec.X + rec.Width / 2));
            circleDistance.Y = Math.Abs(cir.Center.Y - (rec.Y + rec.Height / 2));

            if (circleDistance.X > (rec.Width / 2 + cir.Radius)) return false;
            if (circleDistance.Y > (rec.Height / 2 + cir.Radius)) return false;

            if (circleDistance.X <= (rec.Width / 2)) return true;
            if (circleDistance.Y <= (rec.Height / 2)) return true;

            cornorDistance = (int)(circleDistance.X - rec.Width / 2) * (int)(circleDistance.X - rec.Width / 2) + (int)(circleDistance.Y - rec.Height / 2) * (int)(circleDistance.Y - rec.Height / 2);

            return (cornorDistance <= ((int)cir.Radius ^ 2));
        }

        public void Update()
        {
            if (gameObject.Tag.Contains("Firewave"))
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag == "Player" || go.Tag == "Dummy")
                    {
                        if (intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox, (gameObject.GetComponent("Collider") as Collider).CollisionBox))
                        {
                            FirewavePush(go, go.Id);
                        }
                    }
                }
            }

            if (gameObject.Id == GameWorld.Instance.player.Id)
            {
                foreach (GameObject other in GameWorld.gameObjects)
                {
                    if (other.Tag == "Deflect" && other.Id != gameObject.Id)
                    {
                        GameObject temp = new GameObject();

                        foreach (GameObject go in GameWorld.gameObjects)
                        {
                            if (go.Tag == "Enemy" && go.Id == other.Id)
                            {
                                temp = go;
                            }
                            if (temp.Tag == "Enemy")
                            {

                                Circle playerCircle = new Circle();
                                playerCircle.Center = new Vector2(temp.transform.position.X + 16, temp.transform.position.Y + 16);
                                playerCircle.Radius = (temp.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;
                                if (playerCircle.Intersects((gameObject.GetComponent("Collider") as Collider).CircleCollisionBox))
                                {
                                    Deflect.SetVector(temp, gameObject);
                                    temp = new GameObject();
                                }
                            }
                        }
                    }
                }

                KeyboardState keyState = Keyboard.GetState();

                if (gameObject.Tag.Contains("Boomerang"))
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

                if (gameObject.Tag == "GravityWell")
                {
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += (TestVector / 4) * projectileSpeed;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        Vector2 pull = (gameObject.GetComponent("Physics") as Physics).GetVector(gameObject.transform.position, go.transform.position);
                        pull.Normalize();

                        if (go.Tag == "Dummy" || go.Tag == "Enemy")
                        {
                            if (Vector2.Distance(gameObject.transform.position, go.transform.position) < 300)
                            {
                                (go.GetComponent("Physics") as Physics).Acceleration += pull / 10;
                                if (GameWorld.Instance.client != null && go.Tag == "Enemy")
                                {
                                    GameWorld.Instance.client.SendEnemyAcceleration(go.Id, pull / 10);
                                }
                            }
                        }
                    }
                }

                if (gameObject.Tag == "DeathMine")
                {
                    mineTimer += GameWorld.Instance.deltaTime;
                    if (mineTimer > mineActivationTime)
                    {
                        deathMineActivated = true;
                        (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Red;
                        Color color = Color.Red;
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.SendColor(gameObject.Id, gameObject.Tag, color.R, color.G, color.B, color.A);
                        }
                    }
                }

                if ((gameObject.Tag.Contains("Fireball") || gameObject.Tag.Contains("Drain") || gameObject.Tag.Contains("Chain") || gameObject.Tag.Contains("Nova")) && !gameObject.Tag.Contains("Firewave"))
                {
                    if (gameObject.Tag.Contains("Drain") || gameObject.Tag.Contains("Chain"))
                    {
                        (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 4) * projectileSpeed;
                    }
                    else
                    {
                        (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 2) * projectileSpeed;
                    }
                    if (distanceTravelled > travelDistance)
                    {
                        if (gameObject.Tag.Contains("Chain") && !chainActivated)
                        {
                            //GameWorld.objectsToRemove.Add(gameObject);
                        }
                        else if (gameObject.Tag != "Chain")
                        {
                            //GameWorld.objectsToRemove.Add(gameObject);
                        }
                    }
                }

                if (gameObject.Tag.Contains("Firewave"))
                {
                    (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 15);
                }

                if (chainActivated)
                {
                    if (chainTarget != null)
                    {
                        chainTimer += GameWorld.Instance.deltaTime;
                        gameObject.transform.position = chainTarget.transform.position;
                        Vector2 pull = (gameObject.GetComponent("Physics") as Physics).GetVector(GameWorld.Instance.player.transform.position, chainTarget.transform.position);
                        pull.Normalize();
                        (GameWorld.Instance.player.GetComponent("Physics") as Physics).Acceleration -= pull / 10;
                        if (chainTarget.Tag == "Enemy")
                        {

                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.Chain(chainTarget.Id, pull / 10);
                            }
                        }
                        if (keyState.IsKeyDown(Keys.T) || chainTimer > 2 || Vector2.Distance(new Vector2(chainTarget.transform.position.X + 16, chainTarget.transform.position.Y + 16), new Vector2(GameWorld.Instance.player.transform.position.X + 16, GameWorld.Instance.player.transform.position.Y + 16)) < 35)
                        {
                            chainActivated = false;

                            GameWorld.objectsToRemove.Add(gameObject);

                            if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                            {
                                GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                                if (chainTarget.Tag == "Enemy")
                                {
                                    GameWorld.Instance.client.ChainRemove(chainTarget.Id);
                                }
                            }
                        }
                    }
                }

                if (gameObject.Tag == "HomingMissile")
                {
                    if (homingTimer > 0.5F)
                    {
                        //if (!GameWorld.gameObjects.Exists(x => x.Tag == "Enemy") && !GameWorld.gameObjects.Exists(x => x.Tag == "Dummy"))
                        //{
                        //    GameWorld.objectsToRemove.Add(gameObject);
                        //    if (GameWorld.Instance.client != null)
                        //    {
                        //        GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                        //    }
                        //}
                        foreach (GameObject go in GameWorld.gameObjects)
                        {
                            if (Vector2.Distance(gameObject.transform.position, go.transform.position) < 300 && (go.Tag == "Dummy" || go.Tag == "Enemy"))
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
                                if (bestTarget != null)
                                {

                                }
                                Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(bestTarget, gameObject.transform.position);
                                test.Normalize();
                                (gameObject.GetComponent("Physics") as Physics).Acceleration += (test / 5) * projectileSpeed;
                                break;
                                //if (go.Tag == "Enemy")
                                //{
                                //    (gameObject.GetComponent("Physics") as Physics).Acceleration += (test / 10) * projectileSpeed;
                                //    break;
                                //}
                                //else
                                //{
                                //    (gameObject.GetComponent("Physics") as Physics).Acceleration += (test / 10) * projectileSpeed;
                                //    break;
                                //}

                            }
                            if (Vector2.Distance(gameObject.transform.position, go.transform.position) > 300 && (go.Tag == "Dummy" || go.Tag == "Enemy"))
                            {
                                //Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(target, gameObject.transform.position);
                                //test.Normalize();
                                (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector / 10) * projectileSpeed;
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
                if (GameWorld.Instance.client != null)
                {
                    if (!gameObject.Tag.Contains("Enemy"))
                    {
                        GameWorld.Instance.client.SendProjectile(gameObject.Tag + ",Update", gameObject.transform.position, (gameObject.GetComponent("Physics") as Physics).Velocity);
                    }
                }

                if (abilityTimer > 2)
                {
                    if (gameObject.Tag == "DeathMeteor" || gameObject.Tag.Contains("Nova"))
                    {

                        //GameWorld.objectsToRemove.Add(gameObject);
                    }
                }
            }
        }

        public void CheckDistance()
        {
            if (distanceTravelled > travelDistance)
            {
                if (!gameObject.Tag.Contains("Chain"))
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                    if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                    {
                        GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                    }
                }
                if (gameObject.Tag.Contains("Chain") && !chainActivated)
                {
                    GameWorld.objectsToRemove.Add(gameObject);
                    if (GameWorld.Instance.client != null && gameObject.Id == GameWorld.Instance.player.Id)
                    {
                        GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                    }
                }
            }
        }
    }
}
