using System;
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
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name)
        {
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name, int value)
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
            gameObject = new GameObject();
            gameObject.Tag = name;

            if (name == "Lava")
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "LavaBackGround", 1));
            }
            else if (name == "LavaSpot")
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "LavaSpot", 1));
            }
            else if (name == "Pillar")
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "Pillar", 1));
            }
            else gameObject.AddComponent(new SpriteRenderer(gameObject, "StandardMap600x600", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Map(gameObject));

            gameObject.AddComponent(new Collider(gameObject, false));

            //gameObject.LoadContent(GameWorld.Instance.Content);


            gameObject.transform.position = position;
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability, GameObject shooter)
        {
            throw new NotImplementedException();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
