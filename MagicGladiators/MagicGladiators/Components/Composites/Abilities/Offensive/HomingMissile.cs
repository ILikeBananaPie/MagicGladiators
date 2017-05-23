using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MagicGladiators
{
    class HomingMissile : OffensiveAbility, IUpdateable, ILoadable
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

        

        public HomingMissile(GameObject gameObject, Vector2 position, Vector2 target) : base(gameObject)
        {
            
            go = gameObject;
            originalPos = position;
            this.target = target;
            this.timer = cooldownTimer;
            testVector = target - originalPos;
            testVector.Normalize();
            this.transform = gameObject.transform;
            cooldown = 5;
            
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Q) && canShoot)
            {
                
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "HomingMissile");
            }
          
        }

       

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }
    }
}
