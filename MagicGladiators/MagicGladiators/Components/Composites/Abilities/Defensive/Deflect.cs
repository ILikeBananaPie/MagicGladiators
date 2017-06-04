using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators
{
    class Deflect : DefensiveAbility, IDrawable
    {


        private float timer;
        public bool activated { get; set; } = false;
        private float activationTime;

        private Vector2 oldVelocity;
        private Vector2 newVelocity;

        private GameObject effect;
        private Animator animator;
        private Texture2D sprite;
        private bool isLoaded = false;

        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };


        public Deflect(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 5;
            //LoadContent(GameWorld.Instance.Content);
        }

        private void CreateAnimations()
        {
            //SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 39, 39, 1, Vector2.Zero, sprite));

            animator.PlayAnimation("Idle");
        }

        public override void LoadContent(ContentManager content)
        {
            isLoaded = true;
            //animator = (Animator)gameObject.GetComponent("Animator");
            //sprite = content.Load<Texture2D>("Deflect");
            //effect.LoadContent(GameWorld.Instance.Content);
            //CreateAnimations();
        }

        public override void Update()
        {
            if (!isLoaded)
            {
                LoadContent(GameWorld.Instance.Content);
            }

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 30)
                {
                    activated = false;
                    activationTime = 0;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Deflect")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.SendRemoval("Deflect", gameObject.Id);
                            }
                        }
                    }
                }
            }



            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                activated = true;
                effect = new GameObject();
                effect.AddComponent(new SpriteRenderer(effect, "Deflect", 1));
                effect.AddComponent(new Collider(effect, true, true));
                effect.Tag = "Deflect";
                effect.Id = gameObject.Id;
                GameWorld.newObjects.Add(effect);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Deflect,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero);
                }
            }

            if (activated)
            {

                float radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;
                radius = radius - (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
                effect.transform.position = new Vector2(gameObject.transform.position.X - radius, gameObject.transform.position.Y - radius);

                if (GameWorld.Instance.client != null)
                {

                    GameWorld.Instance.client.SendProjectile("Deflect,Update", effect.transform.position, Vector2.Zero);
                }

                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (abilities.Exists(x => x == go.Tag) && gameObject.Id != go.Id)
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {
                            //SetVector(gameObject, go);
                        }
                    }
                }
            }
        }

        public static void SetVector(GameObject gameObject, GameObject go)
        {

            Vector2 player = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
            Vector2 other = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16);
            Physics physicsSpell = (go.GetComponent("Physics") as Physics);
            Physics physicsPlayer = (gameObject.GetComponent("Physics") as Physics);
            float L = Vector2.Distance(gameObject.transform.position, go.transform.position);
            float ex = (gameObject.transform.position.X - go.transform.position.X) / L;
            float ey = (gameObject.transform.position.Y - go.transform.position.Y) / L;

            float ox = -1 * ey;
            float oy = ex;
            double e1x = (0 * ex + 0 * ey) * ex;
            double e1y = (0 * ex + 0 * ey) * ey;
            if (gameObject.Tag == "Player")
            {
                e1x = (physicsPlayer.Velocity.X * ex + physicsPlayer.Velocity.Y * ey) * ex;
                e1y = (physicsPlayer.Velocity.X * ex + physicsPlayer.Velocity.Y * ey) * ey;
            }
            double e2x = (physicsSpell.Velocity.X * ex + physicsSpell.Velocity.Y * ey) * ex;
            double e2y = (physicsSpell.Velocity.X * ex + physicsSpell.Velocity.Y * ey) * ey;
            double o1x = (0 * ox + 0 * oy) * ox;
            double o1y = (0 * ox + 0 * oy) * oy;
            if (gameObject.Tag == "Player")
            {
                o1x = (physicsPlayer.Velocity.X * ox + physicsPlayer.Velocity.Y * oy) * ox;
                o1y = (physicsPlayer.Velocity.X * ox + physicsPlayer.Velocity.Y * oy) * oy;
            }
            double o2x = (physicsSpell.Velocity.X * ox + physicsSpell.Velocity.Y * oy) * ox;
            double o2y = (physicsSpell.Velocity.X * ox + physicsSpell.Velocity.Y * oy) * oy;
            int playerMass = 10;
            int spellMass = 1;
            if (gameObject.Tag == "Player")
            {
                playerMass = 1;
                spellMass = 10;
            }
            double vxs = (playerMass * e1x + spellMass * e2x) / (playerMass + spellMass);
            double vys = (playerMass * e1y + spellMass * e2y) / (playerMass + spellMass);
            //Velocity Ball 1 after Collision
            double vx1 = -e1x + 2 * vxs + o1x;
            double vy1 = -e1y + 2 * vys + o1y;
            //Velocity Ball 2 after Collision
            double vx2 = -e2x + 2 * vxs + o2x;
            double vy2 = -e2y + 2 * vys + o2y;
            if (gameObject.Tag == "Player")
            {
                (gameObject.GetComponent("Physics") as Physics).Velocity = new Vector2((float)vx1, (float)vy1);
                gameObject.transform.position += new Vector2((float)vx1, (float)vy1) * 2 ;
                /*
                Vector2 test = new Vector2((float)vx2, (float)vy2);
                (go.GetComponent("Physics") as Physics).Velocity = test;
                go.transform.position += test;
                */
            }
            else
            {
                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((float)vx2, (float)vy2);
                Vector2 temp = new Vector2((float)vx2, (float)vy2);
                temp.Normalize();
                if (go.Tag == "GravityWell")
                {
                    (go.GetComponent("Projectile") as Projectile).TestVector = temp * 2;
                }
                else (go.GetComponent("Projectile") as Projectile).TestVector = temp;
                go.transform.position += new Vector2((float)vx2, (float)vy2);
            }


            /*
            if (GameWorld.Instance.client != null)
            {
                GameWorld.Instance.client.Deflect(go.Id, go.Tag, go.transform.position, (go.GetComponent("Physics") as Physics).Velocity);
                Vector2 testVector = new Vector2((float)vx2, (float)vy2);
                testVector.Normalize();
                (go.GetComponent("Projectile") as Projectile).TestVector = testVector / 2;
            }
            */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*
            if (activated)
            {
                float radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 1.2F;
                radius = radius - (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
                effect.transform.position = new Vector2(gameObject.transform.position.X - radius, gameObject.transform.position.Y - radius);
                effect.Draw(spriteBatch);
            }
            */
        }
    }
}