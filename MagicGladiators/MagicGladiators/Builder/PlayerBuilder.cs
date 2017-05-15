using Microsoft.Xna.Framework;
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

        public void BuildGameObject(Vector2 position, object id)
        {
            gameObject = new GameObject(id);

            gameObject.Tag = "Player";
                            
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));
                       
            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Player(gameObject, gameObject.transform));

            gameObject.AddComponent(new Collider(gameObject, false));

            gameObject.AddComponent(new Physics(gameObject));

            // gameObject.LoadContent(GameWorld.Instance.Content);

            MouseState mouse = Mouse.GetState();

            gameObject.AddComponent(new Charge(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator));

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
