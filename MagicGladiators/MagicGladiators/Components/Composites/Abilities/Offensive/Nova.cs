using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class Nova : OffensiveAbility, IUpdateable
    {
       
        private Animator animator;
        private IStrategy strategy;
        private DIRECTION direction;

        private GameObject go;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;
        private Vector2 target;
        private GameObject player;
        private float timer;

        

        public Nova(GameObject gameObject, Vector2 position, Vector2 target) : base(gameObject)
        {
            canShoot = true;
            go = gameObject;
            originalPos = position;
            this.target = target;
            cooldown = 15;
            testVector = target - originalPos;
            testVector.Normalize();
            this.transform = gameObject.transform;
            damage = 5;

        }

       
        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "UpNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "UpRightNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "RightNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownRightNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownLeftNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "LeftNova", new GameObject(), gameObject.Id);
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "UpLeftNova", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("UpNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("UpRightNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("RightNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("DownNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("DownLeftNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("LeftNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("DownRightNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    GameWorld.Instance.client.SendProjectile("UpLeftNova,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));

                }

            }
            
        }
    }
}
