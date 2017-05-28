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
    class Fireball : OffensiveAbility
    {
        public Fireball(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 3;
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && canShoot && !GameWorld.Instance.MouseOnIcon)
            {
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "Fireball", gameObject);
                canShoot = false;
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag.Contains("Clone"))
                    {
                        director.ConstructProjectile(new Vector2(go.transform.position.X, go.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "Fireball", go);
                    }
                }
            }



        }
    }
}
