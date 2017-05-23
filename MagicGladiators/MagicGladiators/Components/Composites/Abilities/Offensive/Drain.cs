using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Drain : OffensiveAbility, IUpdateable
    {
        private bool canShoot = true;

        private float timer;

        private float cooldown = 5;

        public int damage { get; set; } = 10;
        public int healing { get; set; } = 5;

        public Drain(GameObject go, float cooldownTimer, bool canShoot) : base(go,cooldownTimer, canShoot)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (!canShoot)
            {
                timer += GameWorld.Instance.deltaTime;
                if (timer > cooldown)
                {
                    timer = 0;
                    canShoot = true;
                }
            }

            if (keyState.IsKeyDown(Keys.E) && canShoot)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "Drain");
            }
        }
    }
}
