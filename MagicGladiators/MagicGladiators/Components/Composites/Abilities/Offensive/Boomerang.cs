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
    class Boomerang : OffensiveAbility
    {
        private bool canShoot = true;
        private float cooldown = 2f;
        private float cooldowntimer;
        
        
        

        public Boomerang(GameObject go, float cooldownTimer, bool canShoot) : base(go, cooldownTimer, canShoot)
        {

        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            
            if (keyState.IsKeyDown(Keys.G)&& canShoot == true)
            {
                canShoot = false;
                Director dir = new Director(new ProjectileBuilder());
                dir.ConstructProjectile(gameObject.transform.position, new Vector2(mouse.Position.X, mouse.Position.Y), "Boomerang");


            }
            if (!canShoot)
            {
                cooldowntimer += GameWorld.Instance.deltaTime;
            }
            if(cooldowntimer >= cooldown)
            {
                canShoot = true;
                cooldowntimer = 0;
            }

        }
    }
}
