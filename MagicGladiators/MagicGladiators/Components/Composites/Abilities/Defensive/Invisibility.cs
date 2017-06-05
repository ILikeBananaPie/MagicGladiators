using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class Invisibility : Ability
    {
        private bool activated = false;
        private float activationTime;

        public Invisibility(GameObject gameObject) : base(gameObject)
        {
            canShoot = true;
            cooldown = 10;
        }

      

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                activated = true;
                gameObject.IsInvisible = true;
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendInvisibility(gameObject.Id, true);
                }
            }

            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                    gameObject.IsInvisible = false;
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendInvisibility(gameObject.Id, false);
                    }
                }
            }
        }
    }
}
