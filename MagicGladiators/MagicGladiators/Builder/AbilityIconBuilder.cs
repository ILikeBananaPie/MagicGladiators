﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class AbilityIconBuilder : IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name, int value, string text)
        {
            gameObject = new GameObject();

            gameObject.AddComponent(new SpriteRenderer(gameObject, "SpellSheet3", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new AbilityIcon(gameObject, name, value, text));

            gameObject.LoadContent(GameWorld.Instance.Content);
            
            gameObject.transform.position = position;

            gameObject.AddComponent(new Collider(gameObject, false, true));

            gameObject.Tag = "AbilityIcon";

        }

        public void BuildItem(Vector2 position, string[] stats)
        {
            throw new NotImplementedException();
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
