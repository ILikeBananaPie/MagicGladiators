﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class MapBuilder : IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject();
            gameObject.Tag = "Map";

            gameObject.AddComponent(new SpriteRenderer(gameObject, "StandardMap", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Map(gameObject));

            gameObject.AddComponent(new Collider(gameObject, false));

            //gameObject.LoadContent(GameWorld.Instance.Content);


            gameObject.transform.position = position;
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector)
        {
            throw new NotImplementedException();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}