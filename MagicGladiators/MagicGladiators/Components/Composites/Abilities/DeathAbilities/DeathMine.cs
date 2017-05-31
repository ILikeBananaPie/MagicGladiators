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
    class DeathMine : Ability, ILoadable, IDeathAbility
    {
        
        private Vector2 originalPos;
        private float timer;

        private Transform transform;
        private Animator animator;

        private Physics physics;

        private IStrategy strategy;
        private bool activated = false;

        public DeathMine(GameObject gameObject, Vector2 position) : base(gameObject)
        {
           
            this.transform = transform;
            this.animator = animator;
            Name = "DeathMine";
            cooldown = 10;
        }



        public void OnCollisionEnter(Collider other)
        {

        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");



            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleBack", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleLeft", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("IdleRight", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkFront", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkBack", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkLeft", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("WalkRight", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Shoot", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));

            animator.PlayAnimation("Shoot");

            strategy = new Idle(animator);
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            if (GameWorld.Instance.player.CurrentHealth > 0) { return; }

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(mouse.Position.X, mouse.Position.Y), Vector2.Zero, "DeathMine", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Id == gameObject.Id && go.Tag == "DeathMine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            GameWorld.Instance.client.SendRemoval("DeathMine", gameObject.Id);
                        }
                    }
                    GameWorld.Instance.client.SendProjectile("DeathMine,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
                else
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "DeathMine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                }
            }
        }
    }
}
