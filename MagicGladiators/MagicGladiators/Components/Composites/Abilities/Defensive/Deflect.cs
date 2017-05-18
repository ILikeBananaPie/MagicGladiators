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
        private float cooldown = 1;
        private bool canUse = true;
        private float timer;
        private bool activated = false;
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
            effect = new GameObject();
            effect.AddComponent(new SpriteRenderer(effect, "Deflect", 1));
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
            effect.LoadContent(GameWorld.Instance.Content);
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
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                }
            }

            if (!canUse)
            {
                timer += GameWorld.Instance.deltaTime;
                if (timer > cooldown)
                {
                    canUse = true;
                    timer = 0;
                }
            }

            if (keyState.IsKeyDown(Keys.F) && canUse)
            {
                canUse = false;
                activated = true;
            }

            if (activated)
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (abilities.Exists(x => x == go.Tag))
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 1.2F;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {
                            Vector2 player = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                            Vector2 other = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16);
                            activated = false;

                            if (player.X - other.X > player.Y - other.Y && other.X < player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X * -1, (go.GetComponent("Projectile") as Projectile).TestVector.Y);
                            }
                            if (player.X - other.X < player.Y - other.Y && other.X < player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X, (go.GetComponent("Projectile") as Projectile).TestVector.Y * -1);
                            }
                            if (player.X - other.X > player.Y - other.Y && other.X > player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X, (go.GetComponent("Projectile") as Projectile).TestVector.Y * -1);
                            }
                            if (player.X - other.X < player.Y - other.Y && other.X > player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X * -1, (go.GetComponent("Projectile") as Projectile).TestVector.Y);
                            }
                            if (player.X - other.X > player.Y - other.Y && other.X < player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X, (go.GetComponent("Projectile") as Projectile).TestVector.Y) * -1;
                            }
                            if (player.X - other.X < player.Y - other.Y && other.X < player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X * -1, (go.GetComponent("Projectile") as Projectile).TestVector.Y);
                            }
                            if (player.X - other.X > player.Y - other.Y && other.X > player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X, (go.GetComponent("Projectile") as Projectile).TestVector.Y) * -1;
                            }
                            if (player.X - other.X < player.Y - other.Y && other.X > player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2((go.GetComponent("Projectile") as Projectile).TestVector.X * -1, (go.GetComponent("Projectile") as Projectile).TestVector.Y);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (activated)
            {
                float radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 1.2F;
                radius = radius - (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
                effect.transform.position = new Vector2(gameObject.transform.position.X - radius, gameObject.transform.position.Y - radius);
                effect.Draw(spriteBatch);
            }
        }
    }
}