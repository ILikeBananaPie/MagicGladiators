using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class ProjectileBuilder : IBuilder
    {
        private GameObject gameObject;
        //private GameObject target;

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

        public void BuildItem(Vector2 position, string[] stats)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector)
        {
            gameObject = new GameObject();
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Projectile(gameObject, position, targetVector));
            gameObject.Tag = ability;
            gameObject.AddComponent(new Collider(gameObject, true));
            gameObject.AddComponent(new Physics(gameObject));
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
