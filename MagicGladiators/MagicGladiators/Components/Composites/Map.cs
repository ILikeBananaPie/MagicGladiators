using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Map : Component, ILoadable, ICollisionExit, ICollisionEnter, IUpdateable, ICollisionStay
    {
        private Animator animator;

        private IStrategy strategy;

        private Texture2D sprite;

        private List<GameObject> objects = new List<GameObject>();
        private List<GameObject> objectsToRemove = new List<GameObject>();
        private List<GameObject> newObjects = new List<GameObject>();

        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };

        public float LavaDamage { get; set; } = 0.2F;

        private float timer;

        public Map(GameObject gameObject) : base(gameObject)
        {
            gameObject.MaxHealth = 10000;
            gameObject.CurrentHealth = gameObject.MaxHealth;
        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            if (gameObject.Tag == "Lava")
            {
                animator.CreateAnimation("Idle", new Animation(1, 0, 0, 1920, 1080, 1, Vector2.Zero, spriteRenderer.Sprite));
            }
            else if (gameObject.Tag == "LavaSpot")
            {
                animator.CreateAnimation("Idle", new Animation(1, 0, 0, 120, 120, 1, Vector2.Zero, spriteRenderer.Sprite));
            }
            else if (gameObject.Tag == "Pillar")
            {
                animator.CreateAnimation("Idle", new Animation(1, 0, 0, 32, 32, 1, Vector2.Zero, spriteRenderer.Sprite));
            }
            else animator.CreateAnimation("Idle", new Animation(1, 0, 0, 600, 600, 1, Vector2.Zero, spriteRenderer.Sprite));

            animator.PlayAnimation("Idle");

            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            if (gameObject.Tag == "Lava")
            {
                sprite = content.Load<Texture2D>("LavaBackGround");
            }
            else if (gameObject.Tag == "LavaSpot")
            {
                sprite = content.Load<Texture2D>("LavaBackGround");
            }
            else if (gameObject.Tag == "Pillar")
            {
                sprite = content.Load<Texture2D>("Pillar");
            }
            else sprite = content.Load<Texture2D>("StandardMap600x600");
            CreateAnimations();
        }

        public void OnCollisionExit(Collider other)
        {
            if (gameObject.Tag == "Map")
            {
                newObjects.Add(other.gameObject);
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            if (gameObject.Tag == "Map")
            {
                objectsToRemove.Add(other.gameObject);
            }
            if (gameObject.Tag == "Pillar")
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag != "Map" && go.Tag != "Pillar" && go.Tag != "Lava" && gameObject.Tag != "Map" && gameObject.Tag != "Lava" && gameObject.Tag != "LavaSpot")
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 1.2F;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {

                            Physics physicsSpell = (go.GetComponent("Physics") as Physics);
                            Physics physicsPlayer = (gameObject.GetComponent("Physics") as Physics);

                            float L = Vector2.Distance(gameObject.transform.position, go.transform.position);
                            float ex = (gameObject.transform.position.X - go.transform.position.X) / L;
                            float ey = (gameObject.transform.position.Y - go.transform.position.Y) / L;

                            float ox = -1 * ey;
                            float oy = ex;

                            double e1x = (0 * ex + 0 * ey) * ex;
                            double e1y = (0 * ex + 0 * ey) * ey;
                            double e2x = (physicsSpell.Velocity.X * ex + physicsSpell.Velocity.Y * ey) * ex;
                            double e2y = (physicsSpell.Velocity.X * ex + physicsSpell.Velocity.Y * ey) * ey;

                            double o1x = (0 * ox + 0 * oy) * ox;
                            double o1y = (0 * ox + 0 * oy) * oy;
                            double o2x = (physicsSpell.Velocity.X * ox + physicsSpell.Velocity.Y * oy) * ox;
                            double o2y = (physicsSpell.Velocity.X * ox + physicsSpell.Velocity.Y * oy) * oy;

                            int playerMass = 10;
                            int spellMass = 1;
                            double vxs = (playerMass * e1x + spellMass * e2x) / (playerMass + spellMass);
                            double vys = (playerMass * e1y + spellMass * e2y) / (playerMass + spellMass);

                            //Velocity Ball 1 after Collision
                            double vx1 = -e1x + 2 * vxs + o1x;
                            double vy1 = -e1y + 2 * vys + o1y;
                            //Velocity Ball 2 after Collision
                            double vx2 = -e2x + 2 * vxs + o2x;
                            double vy2 = -e2y + 2 * vys + o2y;

                            Vector2 test = new Vector2((float)vx2, (float)vy2);
                            (go.GetComponent("Physics") as Physics).Velocity = test;
                            go.transform.position += test;
                            if (abilities.Exists(x => x == go.Tag) || go.Tag.Contains("Nova"))
                            {
                                test.Normalize();
                                (go.GetComponent("Projectile") as Projectile).TestVector = test * go.ProjectileSpeed;
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
            if (gameObject.Tag == "LavaSpot" && (other.gameObject.Tag == "Player" || other.gameObject.Tag == "Dummy") && other.gameObject.Tag != "Enemy")
            {
                float LavaRadius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
                float otherRadius = (other.gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;

                Vector2 thisCenter = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;
                Vector2 otherCenter = (other.gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;

                if (Vector2.Distance(thisCenter, otherCenter) < LavaRadius - otherRadius)
                {
                    if (!objects.Exists(x => x == other.gameObject))
                    {
                        newObjects.Add(other.gameObject);
                    }
                }
                else objectsToRemove.Add(other.gameObject);
            }
        }

        public void Update()
        {
            foreach (GameObject go in objects)
            {
                if (go.Tag == "Player" || go.Tag == "Enemy" || go.Tag == "Dummy")
                {
                    if (timer >= 0.25F)
                    {
                        timer = 0;
                        //go.CurrentHealth -= LavaDamage;
                    }
                    else
                    {
                        timer += GameWorld.Instance.deltaTime;
                    }
                    go.CurrentHealth -= LavaDamage * go.LavaResistance;
                }
            }
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            if (objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    objects.Remove(go);
                }
                objectsToRemove.Clear();
            }

            if (newObjects.Count > 0)
            {
                objects.AddRange(newObjects);
                newObjects.Clear();
            }
        }
    }
}
