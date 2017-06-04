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
        

        public int damage { get; set; } = 10;
        public int healing { get; set; } = 5;

        public Drain(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 8;
            damage = 7;

        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

           

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "Drain", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendProjectile("Drain,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
            }
        }
    }
}
