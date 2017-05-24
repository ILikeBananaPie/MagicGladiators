﻿using System;
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
    class ShrinkMap : Component
    {
        private bool used = false;

        public ShrinkMap(GameObject gameObject) : base(gameObject)
        {
        }

        public void Update()
        {

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Z) && !used)
            {
                used = true;
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag == "Map")
                    {
                        (go.GetComponent("SpriteRenderer") as SpriteRenderer).Scale -= 0.1F;
                        (go.GetComponent("Collider") as Collider).Scale -= 0.1F;
                        SpriteRenderer sprite = (go.GetComponent("SpriteRenderer") as SpriteRenderer);
                        go.transform.position = new Vector2(640 - (sprite.Sprite.Width * sprite.Scale) / 2, 360 - (sprite.Sprite.Height * sprite.Scale) / 2);
                    }
                }
            }
        }
    }
}