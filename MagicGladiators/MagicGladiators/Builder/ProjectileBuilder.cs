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
            gameObject = new GameObject();
            gameObject.AddComponent(new SpriteRenderer(gameObject, "ProjectileSheet", 1));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.Tag = ability;
            gameObject.Id = id;
            gameObject.AddComponent(new Physics(gameObject));
            gameObject.AddComponent(new Projectile(gameObject, position, targetVector, shooter));
            gameObject.AddComponent(new Collider(gameObject, true));
            gameObject.transform.position = position;
            gameObject.LoadContent(GameWorld.Instance.Content);
            //GameWorld.newObjects.Add(gameObject);

            gameObject.CurrentHealth = 100;
            gameObject.transform.position = position;

        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
