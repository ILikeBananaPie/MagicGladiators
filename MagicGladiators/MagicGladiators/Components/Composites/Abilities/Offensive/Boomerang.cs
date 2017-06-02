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
            damage = 7;

        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            
            if (keyState.IsKeyDown(key)&& canShoot == true)
            {
                canShoot = false;
                Director dir = new Director(new ProjectileBuilder());
                dir.ConstructProjectile(gameObject.transform.position, new Vector2(mouse.Position.X, mouse.Position.Y), "Boomerang", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Boomerang,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }

            }
           
           

        }
    }
}
