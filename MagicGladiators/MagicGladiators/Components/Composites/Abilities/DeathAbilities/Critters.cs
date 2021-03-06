﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    class Critters : Ability, IDeathAbility
    {
        public static Random rnd = new Random();
        private int critterNumber;
        GameObject critter;

        public Critters(GameObject gameObject) : base(gameObject)
        {
            Name = "Critters";
            cooldown = 20;
        }

        

        public override void Update()
        {
            if (GameWorld.Instance.player.CurrentHealth > 0) { return; }

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                for (int i = 0; i < 4; i++)
                {
                    critter = new GameObject();
                    critter.AddComponent(new SpriteRenderer(critter, "Frog", 1));
                    critter.AddComponent(new Animator(critter));
                    critter.AddComponent(new Critter(critter));
                    critter.AddComponent(new Physics(critter));
                    critter.AddComponent(new Collider(critter, true, true));
                    critter.Tag = "Critter" + critterNumber.ToString();
                    critter.CurrentHealth = 1;
                    critter.MaxHealth = 1;
                    critterNumber++;
                    critter.Id = gameObject.Id;
                    int x = rnd.Next(-128, 128);
                    int y = rnd.Next(-128, 128);
                    critter.transform.position = new Vector2(mouse.Position.X + x, mouse.Position.Y + y);
                    GameWorld.newObjects.Add(critter);
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendCritters(gameObject.Id, critter.Tag, critter.transform.position, "Create");
                    }
                }
            }
        }
    }
}
