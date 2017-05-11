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
        private SpriteRenderer sprite;


        public static Vector2 accelerationTest;
        public static Vector2 velocityTest;
        private float breakTest = 0.050F;
        
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
                CollisionTest = true;
                Vector2 test = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;

                //Vector2 vectorBetween = test - other.gameObject.transform.position;

                testVector = (gameObject.GetComponent("Physics") as Physics).GetVector(test, other.gameObject.transform.position);
                //GetVector(test, other.gameObject.transform.position);
                testVector.Normalize();
                //vectorBetween.Normalize();
                testPush = true;
                //testVector = vectorBetween;

            }
            
        }

        public void OnCollisionExit(Collider other)
        {
            CollisionTest = false;
        }

        public void isPushed(Vector2 vectorBetween)
        {
            testPush = true;
            testVector = vectorBetween;
        }

        public Vector2 GetVector(Vector2 origin, Vector2 target)
        {
            return origin - target;
        }

        public Vector2 physicsBreak(float breakFactor, Vector2 velocity)
        {
            if (!(Vector2.Distance(velocity, Vector2.Zero) > 0.05F && Vector2.Distance(velocity, Vector2.Zero) < -0.05F))
            {
                accelerationTest = breakFactor * -velocity;
                //velocityTest += accelerationTest;
                //accelerationTest = Vector2.Zero;
            }
            else
            {
                accelerationTest = Vector2.Zero;
                velocityTest = Vector2.Zero;
            }
            velocityTest = UpdateVelocity(accelerationTest, velocityTest);
            return accelerationTest;
        }

        public void updatePosition()
        {
            gameObject.transform.position += velocityTest;
        }

        public Vector2 UpdateVelocity(Vector2 acceleration, Vector2 velocity)
        {
            return velocity += acceleration;
        }

        public void Update()
        {
            //velocityTest = UpdateVelocity(accelerationTest, velocityTest);
            //accelerationTest = physicsBreak(breakTest, velocityTest);
            //updatePosition();

            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;
            
            if (testPush)
            {
                //accelerationTest = new Vector2((int)testVector.X, (int)testVector.Y);
                accelerationTest = testVector * 10;
                
                if (testTimer < 0.0025F)
                {
                    testTimer += GameWorld.Instance.deltaTime;
                    /*
                    gameObject.transform.position += testVector * testSpeed;
                    if (testSpeed > 0)
                    {
                        testSpeed--;
                    }
                    */
                }
                else
                {
                    testTimer = 0;
                    testPush = false;
                    testSpeed = 10;
                }
                (gameObject.GetComponent("Physics") as Physics).Velocity += accelerationTest;
                (gameObject.GetComponent("Physics") as Physics).Acceleration = Vector2.Zero;
                //velocityTest += accelerationTest;
                //accelerationTest = Vector2.Zero;
                //testPush = false;
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
            updatePackage.InfoUpdate(transform.position, phys.Velocity);
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
