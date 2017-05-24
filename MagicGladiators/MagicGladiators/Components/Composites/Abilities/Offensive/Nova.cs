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
    class Nova : OffensiveAbility, IUpdateable, ILoadable
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
            cooldown = 5;
            testVector = target - originalPos;
            testVector.Normalize();
            this.transform = gameObject.transform;

        }

        public override void LoadContent(ContentManager content)
        { }
        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Z) && canShoot)
            {
                
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "UpNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y),Vector2.Zero, "UpRightNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "RightNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownRightNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "DownLeftNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "LeftNova");
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), Vector2.Zero, "UpLeftNova");

            }
            
        }
    }
}
