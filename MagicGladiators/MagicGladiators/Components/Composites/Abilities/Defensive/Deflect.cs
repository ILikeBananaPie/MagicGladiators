using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Deflect : DefensiveAbility
    {
        private float cooldown = 1;
        private bool canUse = true;
        private float timer;
        private bool activated = false;
        private float activationTime;

        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile" };


        public Deflect(GameObject go) : base(go)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (!canUse)
            {
                timer += GameWorld.Instance.deltaTime;
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                }
                if (timer > cooldown)
                {
                    canUse = true;
                    timer = 0;
                }
            }

            if (keyState.IsKeyDown(Keys.F) && canUse)
            {
                canUse = false;
                activated = true;
            }

            if (activated)
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (abilities.Exists(x => x == go.Tag))
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = gameObject.transform.position;
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 1.2F;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {
                            Vector2 player = new Vector2(gameObject.transform.position.X + 16, gameObject.transform.position.Y + 16);
                            Vector2 other = new Vector2(go.transform.position.X + 16, go.transform.position.Y + 16);
                            activated = false;
                            if (player.X - other.X > player.Y - other.Y && other.X < player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;
                            }
                            if (player.X - other.X < player.Y - other.Y && other.X < player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X > player.Y - other.Y && other.X > player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X < player.Y - other.Y && other.X > player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X > player.Y - other.Y && other.X < player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X < player.Y - other.Y && other.X < player.X && other.Y < player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X > player.Y - other.Y && other.X > player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X, (go.GetComponent("Physics") as Physics).Velocity.Y * -1);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;

                            }
                            if (player.X - other.X < player.Y - other.Y && other.X > player.X && other.Y > player.Y)
                            {
                                (go.GetComponent("Physics") as Physics).Velocity = new Vector2((go.GetComponent("Physics") as Physics).Velocity.X * -1, (go.GetComponent("Physics") as Physics).Velocity.Y);
                                //(go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * 1.1F;
                            }
                        }
                    }
                }
            }
        }
    }
}