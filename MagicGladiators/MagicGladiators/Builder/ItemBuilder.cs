﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class ItemBuilder : IBuilder
    {
        private GameObject gameObject;


        public void BuildGameObject(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name, int value, string text)
        {
            throw new NotImplementedException();
        }

        public void BuildItem(Vector2 position, string[] stats)
        {
            gameObject = new GameObject();

            gameObject.AddComponent(new SpriteRenderer(gameObject, "ItemSheet2", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Item(gameObject, stats));

            gameObject.AddComponent(new Collider(gameObject, false, true));

            gameObject.LoadContent(GameWorld.Instance.Content);

            gameObject.Tag = "Icon";

            gameObject.transform.position = position;
        }

        public void BuildMapPart(Vector2 position, string name)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector)
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
