using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class DummyBuilder : IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject(Vector2 position, object id)
        {
            gameObject = new GameObject(id);

            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));

            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Dummy(gameObject));

            //gameObject.AddComponent(new Collider(gameObject)

            gameObject.transform.position = position;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
