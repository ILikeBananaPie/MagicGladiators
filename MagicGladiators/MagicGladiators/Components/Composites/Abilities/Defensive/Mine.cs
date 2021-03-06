﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Mine : DefensiveAbility, IUpdateable, ICollisionEnter
    {
        private GameObject go;
        private Vector2 originalPos;
        private float timer;
      

        public Mine(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            canShoot = true;
            cooldown = 10;
            go = gameObject;
            originalPos = position;

        }

        

        public void OnCollisionEnter(Collider other)
        {

        }

      

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot && gameObject.CurrentHealth >= 0)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(gameObject.transform.position, Vector2.Zero, "Mine", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Id == gameObject.Id && go.Tag == "Mine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            GameWorld.Instance.client.SendRemoval("Mine", gameObject.Id);
                        }
                    }
                    GameWorld.Instance.client.SendProjectile("Mine,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
                else
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "DeathMine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                }
            }
            
        }
    }
}
