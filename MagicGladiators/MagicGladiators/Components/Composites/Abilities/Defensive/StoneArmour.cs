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
    class StoneArmour : DefensiveAbility, IUpdateable, ILoadable
    {

       
        private bool activated;
        private float timer;
        private float activationTime = 4;
        private float slowSpeed = 0.5f;
        private float resist = 0.5f;


        public StoneArmour(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 8;
        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
           
            if (keyState.IsKeyDown(key) && canShoot)
            {
                activated = true;
                canShoot = false;
                gameObject.Speed -= slowSpeed;
                gameObject.KnockBackResistance -= resist;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkSlateGray;
                Color color = Color.DarkSlateGray;
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendColor(gameObject.Id, "Enemy", color.R, color.G, color.B, color.A);
                }
            }
            if(activated)
            {
                timer += GameWorld.Instance.deltaTime;
                if (timer > activationTime)
                {
                    timer = 0;
                    activated = false;
                    gameObject.Speed += slowSpeed;
                    gameObject.KnockBackResistance += resist;
                    (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
                    Color color = Color.White;
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendColor(gameObject.Id, "Enemy", color.R, color.G, color.B, color.A);
                    }
                }
            }
        }
    }
}
