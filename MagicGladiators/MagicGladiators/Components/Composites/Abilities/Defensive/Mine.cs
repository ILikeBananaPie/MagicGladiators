using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Mine : DefensiveAbility, ILoadable, IUpdateable, ICollisionEnter
    {
        private GameObject go;
        private Vector2 originalPos;
        private float timer;
      

        public Mine(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            canShoot = true;
            cooldown = 5;
            go = gameObject;
            originalPos = position;

        }

        

        public void OnCollisionEnter(Collider other)
        {

        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot && gameObject.CurrentHealth >= 0)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(gameObject.transform.position, Vector2.Zero, "Mine", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Mine,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
            }
            
        }

     
    }
}
