using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Critter : Component, IUpdateable, ILoadable
    {
        private Animator animator;
        //private Random rnd = new Random();
        private float timer;
        private bool canMove = true;
        private float aliveTimer;

        public Critter(GameObject gameObject) : base(gameObject)
        {

        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("Critter", new Animation(1, 0, 0, 8, 8, 6, Vector2.Zero, spriteRenderer.Sprite));

            animator.PlayAnimation("Critter");

        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            CreateAnimations();
        }

        public void Update()
        {
            if (gameObject.Id == GameWorld.Instance.player.Id)
            {
                aliveTimer += GameWorld.Instance.deltaTime;
                if (aliveTimer > 5)
                {
                    aliveTimer = 0;
                    GameWorld.objectsToRemove.Add(gameObject);
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendRemoval(gameObject.Tag, gameObject.Id);
                    }
                }
                if (canMove)
                {
                    canMove = false;
                    int x = Critters.rnd.Next(-10, 10);
                    int y = Critters.rnd.Next(-10, 10);
                    Vector2 vector = new Vector2(x, y);
                    vector.Normalize();
                    (gameObject.GetComponent("Physics") as Physics).Acceleration = vector * 5;
                }
                else
                {
                    timer += GameWorld.Instance.deltaTime;
                    if (timer > 2)
                    {
                        canMove = true;
                        timer = 0;
                    }
                }
                gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendCritters(gameObject.Id, gameObject.Tag, gameObject.transform.position, "Update");
                }
            }
        }
    }
}
