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
    class Recall: MobilityAbility, IUpdateable, ILoadable
    {
        private float activated;
        private float activatedTimer = 4f;
        private bool activatedAbility;
        private bool cooldownbool = false;
        private float cooldownTimer;
        private Vector2 startPos;

        public Recall(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 6;
        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();


            if (keyState.IsKeyDown(Keys.V) && canShoot)
            {
                activatedAbility = true;
                canShoot = false;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkBlue;
                startPos = gameObject.transform.position;
            }

            if (activatedAbility)
            {
                activated += GameWorld.Instance.deltaTime;
            }

            if (activated >= activatedTimer)
            {          
                cooldownbool = true;
                activatedAbility = false;
                activated = 0;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
                gameObject.transform.position = startPos;
            }

            if (cooldownbool)
            {
                cooldownTimer += GameWorld.Instance.deltaTime;
            }

            if (cooldownTimer >= cooldown)
            {
                canShoot = true;
                cooldownTimer = 0;
            }
        }
    }
}