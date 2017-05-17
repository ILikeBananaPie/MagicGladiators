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
        private bool canShoot = true;

        public Mine(GameObject gameObject, Vector2 position) : base(gameObject)
        {
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
            //KeyboardState keyState = Keyboard.GetState();
            //MouseState mouse = Mouse.GetState();

            //if (keyState.IsKeyDown(Keys.R) && canShoot)
            //{
            //    canShoot = false;
            //    Director director = new Director(new ProjectileBuilder());
            //    director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "Mine");
            //}
            //if (!canShoot)
            //{
            //    timer += GameWorld.Instance.deltaTime;
            //}
            //if (timer > 5)
            //{
            //    timer = 0;
            //    canShoot = true;
            //}
        }

     
    }
}
