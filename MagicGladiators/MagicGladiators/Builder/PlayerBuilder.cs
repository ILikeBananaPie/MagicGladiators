﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;


        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject();

            gameObject.Tag = "Player";

            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));
                     
            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Player(gameObject, gameObject.transform));

            gameObject.AddComponent(new Collider(gameObject, false));

            gameObject.AddComponent(new Physics(gameObject));

            gameObject.AddComponent(new RollingMeteor(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator));

            gameObject.AddComponent(new ShrinkMap(gameObject));

            //gameObject.AddComponent(new StoneArmour(gameObject));

            //gameObject.AddComponent(new Spellshield(gameObject));

            //gameObject.AddComponent(new Chain(gameObject));

            //gameObject.AddComponent(new Deflect(gameObject));

            //gameObject.AddComponent(new SpeedBoost(gameObject));

            //gameObject.AddComponent(new Drain(gameObject));

            //gameObject.AddComponent(new HomingMissile(gameObject, gameObject.transform.position, Vector2.Zero));

            // gameObject.LoadContent(GameWorld.Instance.Content);

            //gameObject.LoadContent(GameWorld.Instance.Content);

            //gameObject.AddComponent(new Boomerang(gameObject));
            gameObject.AddComponent(new Fireball(gameObject));

            //gameObject.AddComponent(new Recall(gameObject));




            MouseState mouse = Mouse.GetState();
            gameObject.transform.position = position;


            gameObject.AddComponent(new DeathMine(gameObject, gameObject.transform.position));
            //gameObject.AddComponent(new Charge(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator));

            //gameObject.AddComponent(new Blink(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator));

           // gameObject.AddComponent(new Nova(gameObject, gameObject.transform.position, Vector2.Zero));
            gameObject.transform.position = position;

            //gameObject.AddComponent(new Mine(gameObject, gameObject.transform.position));




        }

        public void BuildIcon(Vector2 position, string name, string text)
        {
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name, int value, string text)
        {
            throw new NotImplementedException();
        }

        public void BuildItem(Vector2 position, string[] stats)
        {
            throw new NotImplementedException();
        }

        public void BuildMapPart(Vector2 position, string name)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability, GameObject shooter, string id)
        {
            throw new NotImplementedException();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

    }
}
