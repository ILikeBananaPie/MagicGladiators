﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class SpeedBoost : MobilityAbility
    {
        private bool canUse = true;
        private bool activated = false;
        private bool use = false;
        private float cooldownTimer;
        private float cooldown = 5;

        private float speedFactor = 1;

        private float activationTime = 2;
        private float activationTimer;

        private float oldSpeed;

        public SpeedBoost(GameObject go) : base(go)
        {
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            cooldownTimer += GameWorld.Instance.deltaTime;

            if (cooldownTimer > cooldown)
            {
                canUse = true;
                cooldownTimer = 0;
                use = false;
            }

            if (keyState.IsKeyDown(Keys.X) && canShoot)
            {
                oldSpeed = Player.speed;
                canUse = false;
                activated = true;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkSlateGray;
                Color color = Color.DarkSlateGray;
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendColor(gameObject.Id, color.R, color.G, color.B, color.A);
                }
            }
            if (activated)
            {
                if (!use)
                {
                    use = true;
                    gameObject.Speed = gameObject.Speed + speedFactor;
                    //Player.speed = Player.speed + speedFactor;
                }
                activationTimer += GameWorld.Instance.deltaTime;
                if (activationTimer > activationTime)
                {
                    //Player.speed = oldSpeed;
                    gameObject.Speed -= speedFactor;
                    activated = false;
                    activationTimer = 0;
                    (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
                    Color color = Color.White;
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendColor(gameObject.Id, color.R, color.G, color.B, color.A);
                    }
                }
            }
        }
    }
}
