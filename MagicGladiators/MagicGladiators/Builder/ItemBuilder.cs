using System;
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

        public void BuildItem(Vector2 position, string[] stats)
        {
            gameObject = new GameObject();

            gameObject.AddComponent(new SpriteRenderer(gameObject, "ItemSheet", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Item(gameObject, stats));

            gameObject.AddComponent(new Collider(gameObject, false));

            gameObject.LoadContent(GameWorld.Instance.Content);

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
