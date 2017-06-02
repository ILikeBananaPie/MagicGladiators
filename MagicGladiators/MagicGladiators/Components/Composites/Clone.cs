using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Clone : Component, IUpdateable, ILoadable, ICollisionEnter
    {
        private Animator animator;
        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };

        public Clone(GameObject gameObject) : base(gameObject)
        {

        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("LightGreen", new Animation(1, 64, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Green", new Animation(1, 96, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Blue", new Animation(1, 96, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Red", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Orange", new Animation(1, 32, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Brown", new Animation(1, 0, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Yellow", new Animation(1, 64, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Purple", new Animation(1, 32, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));


            animator.PlayAnimation("LightGreen");

        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            CreateAnimations();
        }

        public void Update()
        {

            foreach (GameObject go in GameWorld.gameObjects)
            {
                if ((go.Tag == "Enemy" || go.Tag == "Player") && go.Id == gameObject.Id)
                {
                    if (go.cloneNumber == 1 && gameObject.cloneNumber == 2)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    }
                    if (go.cloneNumber == 1 && gameObject.cloneNumber == 3)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y + 64);
                    }
                    if (go.cloneNumber == 1 && gameObject.cloneNumber == 4)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y + 64);
                    }

                    if (go.cloneNumber == 2 && gameObject.cloneNumber == 1)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    }
                    if (go.cloneNumber == 2 && gameObject.cloneNumber == 3)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 2 && gameObject.cloneNumber == 4)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y + 64);
                    }

                    if (go.cloneNumber == 3 && gameObject.cloneNumber == 1)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 3 && gameObject.cloneNumber == 2)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 3 && gameObject.cloneNumber == 4)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    }

                    if (go.cloneNumber == 4 && gameObject.cloneNumber == 1)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 4 && gameObject.cloneNumber == 2)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 4 && gameObject.cloneNumber == 3)
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    }

                    //if (gameObject.Tag.Contains("1"))
                    //{
                    //    gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    //}
                    //if (gameObject.Tag.Contains("2"))
                    //{
                    //    gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    //}
                    //if (gameObject.Tag.Contains("3"))
                    //{
                    //    gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    //}
                    //if (gameObject.Tag.Contains("4"))
                    //{
                    //    gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y + 64);
                    //}
                }
                foreach (GameObject go2 in GameWorld.gameObjects)
                {
                    if (go2.Id == go.Id && go.Tag == "Player" && go2.Tag.Contains("Clone") && go.CurrentHealth < 0)
                    {
                        GameWorld.objectsToRemove.Add(go2);
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.SendRemoval(go2.Tag, go2.Id);
                        }
                    }
                }
            }
        }

        public void OnCollisionEnter(Collider other)
        {

        }
    }
}
