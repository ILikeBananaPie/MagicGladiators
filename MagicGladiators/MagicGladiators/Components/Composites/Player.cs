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
        private SpriteFont fontText;
        private bool testPush;
        private Vector2 testVector;
        private float testTimer;
        private bool canShoot;
        private SpriteRenderer sprite;

        private readonly Object thisLock = new Object();
        private UpdatePackage _updatePackage;
        public UpdatePackage updatePackage
        {
            get { lock (thisLock) { return _updatePackage; } }
            set { lock (thisLock) { _updatePackage = value; } }
        }
        private Physics phys;

        public Player(GameObject gameObject, Transform transform) : base(gameObject)
        {
            gameObject.Tag = "Player";
            gameObject.MaxHealth = 100;
            gameObject.CurrentHealth = gameObject.MaxHealth;
            this.transform = transform;
            sprite = (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer);
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
            phys = gameObject.GetComponent("Physics") as Physics;

            updatePackage = new UpdatePackage(transform.position);
            CreateAnimations();
        }

        public void OnAnimationDone(string animationName)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag != "Ability")
            {
                gameObject.CurrentHealth -= (other.gameObject.GetComponent("Dummy") as Dummy).Damage;
                Vector2 test = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;
                testVector = (gameObject.GetComponent("Physics") as Physics).GetVector(test, other.gameObject.transform.position);
                testVector.Normalize();
                testPush = true;
            }
        }

        public void OnCollisionExit(Collider other)
        {
        }

        public void isPushed(Vector2 vectorBetween)
        {
            testPush = true;
            testVector = vectorBetween;
        }

        public void Update()
        {
            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;

            if (testPush)
            {
                (gameObject.GetComponent("Physics") as Physics).Acceleration += testVector * 10;
                if (testTimer < 0.0025F)
                {
                    testTimer += GameWorld.Instance.deltaTime;
                }
                else
                {
                    testTimer = 0;
                    testPush = false;
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
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                canShoot = false;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                canShoot = true;
            }
            updatePackage.InfoUpdate(transform.position, phys.Velocity);

            if (keyState.IsKeyDown(Keys.Q) && canShoot)
            {
                canShoot = false;
                Director director = new Director(new HomingMissileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
               
            }
           
        }

        public void OnCollisionStay(Collider other)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            spriteBatch.DrawString(fontText, "Health: " + gameObject.CurrentHealth + "/" + gameObject.MaxHealth, new Vector2(0, 0), Color.Black);

            spriteBatch.DrawString(fontText, "PlayerX: " + gameObject.transform.position.X, new Vector2(0, 20), Color.Black);
            spriteBatch.DrawString(fontText, "PlayerY: " + gameObject.transform.position.Y, new Vector2(0, 40), Color.Black);
            spriteBatch.DrawString(fontText, "MouseX: " + mouse.X, new Vector2(0, 60), Color.Black);
            spriteBatch.DrawString(fontText, "MouseY: " + mouse.Y, new Vector2(0, 80), Color.Black);
        }
    }
}
