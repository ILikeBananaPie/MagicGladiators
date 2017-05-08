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
    public enum DIRECTION { Front, Back, Left, Right };

    class Player : Component, IUpdateable, IAnimateable, ILoadable, ICollisionEnter, ICollisionExit, ICollisionStay, IDrawable
    {
        private Animator animator;

        private IStrategy strategy;

        private DIRECTION direction;

        private Transform transform;
        private bool CollisionTest;
        private SpriteFont fontText;
        private bool testPush;
        private Vector2 testVector;
        private float testTimer;
        private float testSpeed = 20;
        private bool canShoot;

        public Player(GameObject gameObject, Transform transform) : base(gameObject)
        {
            gameObject.Tag = "Player";
            this.transform = transform;

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

            animator.PlayAnimation("IdleFront");

            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            fontText = content.Load<SpriteFont>("fontText");

            CreateAnimations();
        }

        public void OnAnimationDone(string animationName)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag != "Ability")
            {
                CollisionTest = true;
                Vector2 test = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;
                double sin = test.X * other.CircleCollisionBox.Center.Y - other.CircleCollisionBox.Center.X * test.Y;
                double cos = test.X * other.CircleCollisionBox.Center.X + test.Y * other.CircleCollisionBox.Center.Y;

                double angle = Math.Atan2(sin, cos) * (180 / Math.PI);
                //other.gameObject.transform.position.X += Math.Cos(angle);
                //other.gameObject.transform.position.Y += Math.Sin(angle);

                Vector2 vectorBetween = other.gameObject.transform.position - test;
                //Vector2 playerPushVector = test - other.gameObject.transform.position;
                //playerPushVector.Normalize()
                vectorBetween.Normalize();
                //(other.gameObject.GetComponent("Dummy") as Dummy).isPushed(vectorBetween);
                testPush = true;
                testVector = vectorBetween;

                //other.gameObject.transform.position = other.gameObject.transform.position + vectorBetween;

                //other.gameObject.transform.position =  new Vector2(other.gameObject.transform.position.X + (float)Math.Cos(angle) * 50, other.gameObject.transform.position.Y + (float)Math.Sin(angle) * 50);
            }

        }

        public void OnCollisionExit(Collider other)
        {
            CollisionTest = false;
        }

        public void Update()
        {
            if (testPush)
            {
                if (testTimer < 1)
                {
                    testTimer += GameWorld.Instance.deltaTime;
                    gameObject.transform.position -= testVector * testSpeed;
                    if (testSpeed > 0)
                    {
                        testSpeed -= 0.5F;
                    }
                }
                else
                {
                    testTimer = 0;
                    testPush = false;
                    testSpeed = 20;
                }
            }

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.D))
            {
                if (!(strategy is Move))
                {
                    strategy = new Move(gameObject.transform, animator);
                }
            }
            else
            {
                strategy = new Idle(animator);
            }
            strategy.Execute(ref direction);

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && canShoot)
            {
                
                Director director = new Director(new ProjectileBuilder());
                //Vector2 mousePos = Vector2.Transform(mouse.Position, Matrix.Invert(GameWorld.Instance.vie))
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                canShoot = false;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                canShoot = true;
            }

        }

        public void OnCollisionStay(Collider other)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            spriteBatch.DrawString(fontText, "PlayerX: " + gameObject.transform.position.X, new Vector2(0, 10), Color.Black);
            spriteBatch.DrawString(fontText, "PlayerY: " + gameObject.transform.position.Y, new Vector2(0, 30), Color.Black);
            spriteBatch.DrawString(fontText, "MouseX: " + mouse.X, new Vector2(0, 50), Color.Black);
            spriteBatch.DrawString(fontText, "MouseY: " + mouse.Y, new Vector2(0, 70), Color.Black);



            if (CollisionTest)
            {
                spriteBatch.DrawString(fontText, "Collision Detected!", new Vector2(0, 0), Color.Black);
            }
        }
    }
}
