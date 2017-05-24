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

        public static int gold = 10000;
        public static float speed = 1;

        private Transform transform;
        private SpriteFont fontText;
        private bool testPush;
        private Vector2 testVector;
        private float testTimer;
        private SpriteRenderer sprite;
        private float regenTimer;

        private float timer;

        public static List<GameObject> items = new List<GameObject>();
        public static List<GameObject> abilities = new List<GameObject>();

        public static Vector2 testSpeed;

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
            animator.CreateAnimation("ChargeLeft", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("ChargeRight", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
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

        public void TakeDamage(float damage)
        {
            gameObject.CurrentHealth -= damage * gameObject.DamageResistance;
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.gameObject.Tag == "Dummy")
            {
                //gameObject.CurrentHealth -= (other.gameObject.GetComponent("Dummy") as Dummy).Damage;
                Vector2 test = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center;
                testVector = (gameObject.GetComponent("Physics") as Physics).GetVector(test, (other.gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Center);
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
            Vector2 oldPos = gameObject.transform.position;
            gameObject.transform.position += (gameObject.GetComponent("Physics") as Physics).Velocity;

            if (gameObject.CurrentHealth >= gameObject.MaxHealth)
            {
                gameObject.CurrentHealth = gameObject.MaxHealth;
            }
            else
            {
                if (regenTimer > 1)
                {
                    gameObject.CurrentHealth += gameObject.HealthRegen;
                    regenTimer = 0;
                }
                else regenTimer += GameWorld.Instance.deltaTime;
            }
            if (testPush)
            {
                (gameObject.GetComponent("Physics") as Physics).Acceleration += (testVector * 5) * gameObject.KnockBackResistance;
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

        
           
         
            updatePackage.InfoUpdate(transform.position, phys.Velocity);
         
           
        }

        public void OnCollisionStay(Collider other)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            spriteBatch.DrawString(fontText, "Health: " + gameObject.CurrentHealth.ToString(".00") + "/" + gameObject.MaxHealth.ToString(".00"), new Vector2(0, 0), Color.Black);

            spriteBatch.DrawString(fontText, "PlayerX: " + (int)gameObject.transform.position.X, new Vector2(0, 20), Color.Black);
            spriteBatch.DrawString(fontText, "PlayerY: " + (int)gameObject.transform.position.Y, new Vector2(0, 40), Color.Black);
            spriteBatch.DrawString(fontText, "MouseX: " + mouse.X, new Vector2(0, 60), Color.Black);
            spriteBatch.DrawString(fontText, "MouseY: " + mouse.Y, new Vector2(0, 80), Color.Black);
            spriteBatch.DrawString(fontText, "Gold: " + gold, new Vector2(0, 100), Color.Black);
            spriteBatch.DrawString(fontText, "speed: " + testSpeed, new Vector2(0, 160), Color.Black);
            string phase;
            if (GameWorld.buyPhase)
            {
                phase = "Buy Phase";
            }
            else phase = "Combat Phase";
            spriteBatch.DrawString(fontText, phase, new Vector2(0, 180), Color.Black);
            spriteBatch.DrawString(fontText, GameWorld.currentRound + " / " + GameWorld.numberOfRounds, new Vector2(0, 200), Color.Black);

        }

        public void UpdateStats()
        {
            gameObject.MaxHealth = 100;
            gameObject.Speed = 1;
            gameObject.DamageResistance = 1;
            gameObject.LavaResistance = 1;
            gameObject.KnockBackResistance = 1;
            gameObject.ProjectileSpeed = 1;
            gameObject.LifeSteal = 0;
            gameObject.CooldownReduction = 1;

            foreach (GameObject go in items)
            {
                Item item = (go.GetComponent("Item") as Item);
                gameObject.MaxHealth += item.Health;
                gameObject.DamageResistance += item.DamageResistance;
                gameObject.Speed += item.Speed;
                gameObject.LavaResistance += item.LavaResistance;
                gameObject.KnockBackResistance -= item.KnockBackResistance;
                gameObject.ProjectileSpeed += item.ProjectileSpeed;
                gameObject.LifeSteal += item.LifeSteal;
                gameObject.CooldownReduction -= item.CDR;
            }
        }
    }
}
