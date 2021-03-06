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
        private float speedFactor = 1;

        private float activationTime = 2;
        private float activationTimer;

        private float oldSpeed;

        public SpeedBoost(GameObject go) : base(go)
        {
            cooldown = 10;
            canShoot = true;
        }


        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
              
                canShoot = false;
              
                activated = true;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Green;
                Color color = Color.Green;
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendColor(gameObject.Id, "Enemy", color.R, color.G, color.B, color.A);
                }
            }
            if (activated)
            {
                if (!use)
                {
                    use = true;
                    gameObject.Speed = gameObject.Speed + speedFactor;
                  
                }
                activationTimer += GameWorld.Instance.deltaTime;
                if (activationTimer > activationTime)
                {
                   
                    gameObject.Speed = gameObject.Speed - speedFactor;
                    use = false;
                    activated = false;
                    activationTimer = 0;
                    (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
                    Color color = Color.White;
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendColor(gameObject.Id, "Enemy", color.R, color.G, color.B, color.A);
                    }
                }
            }
        }
    }
}
