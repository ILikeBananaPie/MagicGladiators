using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class HomingMissileBuilder: IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject(Vector2 position, object id)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector)
        {
            gameObject = new GameObject(2);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new HomingMissile(gameObject, position, targetVector));
            gameObject.Tag = "Ability";
            gameObject.AddComponent(new Collider(gameObject, true));
            gameObject.transform.position = position;
            gameObject.LoadContent(GameWorld.Instance.Content);
            //GameWorld.newObjects.Add(gameObject);

            gameObject.transform.position = position;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
