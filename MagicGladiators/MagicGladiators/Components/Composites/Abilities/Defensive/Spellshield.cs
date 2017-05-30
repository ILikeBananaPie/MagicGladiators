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
    class Spellshield : DefensiveAbility, IDrawable
    {
        
       
        private float timer;
        private bool activated = false;
        private float activationTime;
        private GameObject effect;
        private Animator animator;
        private Texture2D sprite;
        private bool isLoaded = false;


        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };


        public Spellshield(GameObject go) : base(go)
        {
            cooldown = 5;
            canShoot = true;

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
            //sprite = content.Load<Texture2D>("Spellshield");
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

            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Spellshield")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.SendRemoval("Spellshield", gameObject.Id);
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
                effect.AddComponent(new SpriteRenderer(effect, "Spellshield", 1));
                effect.Tag = "Spellshield";
                GameWorld.newObjects.Add(effect);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Spellshield,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero);
                }
            }

            if (activated)
            {

                float radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;
                radius = radius - (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
                effect.transform.position = new Vector2(gameObject.transform.position.X - radius, gameObject.transform.position.Y - radius);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Spellshield,Update", effect.transform.position, Vector2.Zero);
                }

                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (abilities.Exists(x => x == go.Tag))
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {
                            //GameWorld.objectsToRemove.Add(go);
                            if (GameWorld.Instance.client != null)
                            {
                                //GameWorld.Instance.client.SendRemoval(go.Tag, go.Id);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (activated)
            //{
            //    float radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2F;
            //    radius = radius - (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius;
            //    effect.transform.position = new Vector2(gameObject.transform.position.X - radius, gameObject.transform.position.Y - radius);
            //    effect.Draw(spriteBatch);
            //}
        }
    }
}
