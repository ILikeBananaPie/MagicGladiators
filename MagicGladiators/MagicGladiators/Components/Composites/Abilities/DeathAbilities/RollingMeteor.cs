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
    class RollingMeteor : Component, ILoadable, IUpdateable
    {
        private float movementSpeed = 200;

        private IStrategy strategy;

        private Transform transform;

        private Animator animator;

        private Physics physics;

        private Random rnd;

        public static DIRECTION direction
        { get; private set; }

        public RollingMeteor(GameObject go, Transform transform, Animator animator) : base(go)
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

        //dhfgjsdhgfjsgdfjhsgdhdgsfs
        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");

        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector2 translation = Vector2.Zero;



            if (keyState.IsKeyDown(Keys.I) && gameObject.CurrentHealth <= 0)
            {


                physics.Acceleration += new Vector2(0, -0.25F);

            }
            if (keyState.IsKeyDown(Keys.J))
            {

                physics.Acceleration += new Vector2(-0.25F, 0);


            }
            if (keyState.IsKeyDown(Keys.K))
            {

                physics.Acceleration += new Vector2(0, 0.25F);


            }
            if (keyState.IsKeyDown(Keys.L))
            {

                physics.Acceleration += new Vector2(0.25F, 0);


            }


        }
    }
}
