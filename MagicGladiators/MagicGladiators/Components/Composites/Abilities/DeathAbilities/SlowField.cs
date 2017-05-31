using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class SlowField : Ability, IDeathAbility
    {
        private bool activated = false;
        private bool use = false;

        private float speedFactor = 0.5f;

        private float activationTime = 5;
        private float activationTimer;
        string previous = "";

        public SlowField(GameObject gameObject) : base(gameObject)
        {
            Name = "SlowField";
            cooldown = 10;
        }

        public override void Update()
        {
            if (GameWorld.Instance.player.CurrentHealth > 0) { return; }

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                activated = true;
                Color color = Color.DarkGreen;
                foreach (var go in GameWorld.gameObjects)
                {
                    if (go.Tag == "Map")
                    {
                        (go.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkGreen;
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.SendColor("", "Map", color.R, color.G, color.B, color.A);
                        }
                    }
                    if (go.Tag == "Enemy")
                    {
                        if (GameWorld.Instance.client != null)
                        {
                            //GameWorld.Instance.client.SendSpeedDown(go.Id, -speedFactor);
                        }
                    }
                }
            }
            if (activated)
            {
                activationTimer += GameWorld.Instance.deltaTime;
                if (activationTimer > activationTime)
                {
                    Color color = Color.White;
                    foreach (var go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Map")
                        {
                            (go.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.SendColor("", "Map", color.R, color.G, color.B, color.A);
                            }
                        }
                        if (go.Tag == "Enemy")
                        {
                            if (GameWorld.Instance.client != null)
                            {
                                //GameWorld.Instance.client.SendSpeedUp(go.Id, speedFactor);
                            }
                        }

                    }
                    activated = false;
                    activationTimer = 0;
                }
            }
        }

        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }
    }
}
