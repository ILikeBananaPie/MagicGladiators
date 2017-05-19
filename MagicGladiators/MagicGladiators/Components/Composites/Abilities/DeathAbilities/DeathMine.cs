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
    class DeathMine : Component, ILoadable
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
           // this.physics = (transform.gameObject.GetComponent("Physics") as Physics);

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

        public void LoadContent(ContentManager content)
        {

        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.R) && !activated)
            {
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(mouse.Position.X, mouse.Position.Y), Vector2.Zero, "DeathMine");
                activated = false;

                activated = true;
                
                    
                
            }
            if (keyState.IsKeyUp(Keys.R))
            {
                activated = false;
            }
            /*if (timer > 5)
            {
                timer = 0;
                canShoot = true;
            }*/
        }
    }
}
