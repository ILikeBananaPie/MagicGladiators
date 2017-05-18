using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators.Components.Composites.Abilities.DeathAbilities
{
    class DeathMine : Component, ILoadable, IUpdateable
    {
        private Transform transform;
        private Animator animator;
        private Physics physics;
        private bool activated;
        private bool rPressed = false;
        private IStrategy strategy;



        public DeathMine(GameObject go, Transform transform, Animator animator) : base(go)
        {
            this.transform = transform;
            this.animator = animator;
            this.physics = (transform.gameObject.GetComponent("Physics") as Physics);
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

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");
        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            Vector2 translation = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.R) && gameObject.CurrentHealth <= 0 && !rPressed)
            {
                rPressed = true;
                if (activated == true)
                {
                    activated = false;
                }
                else activated = true;

            }
            if (keyState.IsKeyUp(Keys.Q))
            {
                rPressed = false;
            }

            if (activated)
            {
               
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(mouse.Position.X, mouse.Position.Y), Vector2.Zero, "DeathMine");
                activated = false;
            }
        }
    }
}
