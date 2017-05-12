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
    class Map : Component, ILoadable, ICollisionExit, ICollisionEnter, IUpdateable
    {
        private Animator animator;

        private IStrategy strategy;

        private List<GameObject> objects = new List<GameObject>();
        private List<GameObject> objectsToRemove = new List<GameObject>();
        private List<GameObject> newObjects = new List<GameObject>();


        private GameObject go;
        private Transform transform;


        public Map(GameObject gameObject) : base(gameObject)
        {
        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 0, 0, 1, Vector2.Zero, spriteRenderer.Sprite));

            animator.PlayAnimation("Idle");

            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("StandardMap");
            GameWorld.newObjects.Add(go);

            CreateAnimations();
        }

        public void OnCollisionExit(Collider other)
        {
            objectsToRemove.Add(other.gameObject);
        }

        public void OnCollisionEnter(Collider other)
        {
            newObjects.Add(other.gameObject);
        }

        public void Update()
        {
            foreach (GameObject go in objects)
            {
                go.
            }
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
