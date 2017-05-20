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
        }

        public void OnCollisionStay(Collider other)
        {
            if (gameObject.Tag == "LavaSpot" && (other.gameObject.Tag == "Player" || other.gameObject.Tag == "Dummy" || other.gameObject.Tag == "Enemy"))
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
