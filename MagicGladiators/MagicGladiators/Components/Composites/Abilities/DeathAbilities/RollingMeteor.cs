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
    class RollingMeteor : Ability, ILoadable, IDeathAbility
    {
        private float movementSpeed = 200;

        private IStrategy strategy;

        private Transform transform;

        private Animator animator;

        private Physics physics;

        private Random rnd;


        private bool activated;
        private bool mousePressedBool = false;
        private bool mouseReleasedBool = true;
        private bool qPressed = false;

        private bool redoStartPoint;

        private Vector2 pointA;
        private Vector2 pointB;

        public static DIRECTION direction
        { get; private set; }

        public RollingMeteor(GameObject go, Transform transform, Animator animator) : base(go)
        {
            this.transform = transform;
            this.animator = animator;
            this.physics = (transform.gameObject.GetComponent("Physics") as Physics);
            Name = "RollingMeteor";
            cooldown = 25;
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
            animator = (Animator)gameObject.GetComponent("Animator");

            Texture2D sprite = content.Load<Texture2D>("Player");

        }

        public override void Update()
        {
            if (GameWorld.Instance.player.CurrentHealth > 0) { return; }

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            Vector2 translation = Vector2.Zero;

            if (keyState.IsKeyDown(key) && !qPressed && canShoot)
            {
                qPressed = true;
                if (activated == true)
                {
                    activated = false;
                }
                else activated = true;

            }
            if (keyState.IsKeyUp(Keys.Q))
            {
                qPressed = false;
            }
            if (activated && mouse.LeftButton == ButtonState.Pressed && !mousePressedBool)
            {
                mousePressedBool = true;
                mouseReleasedBool = false;
                pointA = new Vector2(mouse.Position.X, mouse.Position.Y);
            }

            if (activated && mouse.LeftButton == ButtonState.Released && !mouseReleasedBool)
            {
                canShoot = false;
                pointB = new Vector2(mouse.Position.X, mouse.Position.Y);

                Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(pointB, pointA);
                test.Normalize();
                pointA = pointA - test * 1000;
                CorrectStartPoint();
                
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(pointA, pointB, "DeathMeteor", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Id == gameObject.Id && go.Tag == "DeathMeteor")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            GameWorld.Instance.client.SendRemoval("DeathMeteor", gameObject.Id);
                        }
                    }
                    GameWorld.Instance.client.SendProjectile("DeathMeteor,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
                else
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "DeathMeteor")
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                }
                mousePressedBool = false;
                mouseReleasedBool = true;
                activated = false;
            }
        }
        public void CorrectStartPoint()
        {
            Vector2 test = (gameObject.GetComponent("Physics") as Physics).GetVector(pointB, pointA);
            test.Normalize();
            while (pointA.X > GameWorld.Instance.Window.ClientBounds.Width || pointA.X < 0 || pointA.Y > GameWorld.Instance.Window.ClientBounds.Height || pointA.Y < 0)
            {
                pointA += test * 100;
            }
        }
    }
}
