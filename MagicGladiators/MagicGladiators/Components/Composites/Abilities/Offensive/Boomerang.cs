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
        
        
        
        

        public Boomerang(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 5;
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
           
           

        }
    }
}
