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
    class Deflect : DefensiveAbility
    {
        private float cooldown = 5;
        private bool canUse = true;
        private float timer;
        private bool activated = false;
        private float activationTime;

        public Deflect(GameObject go) : base(go)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (!canUse)
            {
                timer += GameWorld.Instance.deltaTime;
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                }
                if (timer > cooldown)
                {
                    canUse = true;
                    timer = 0;
                }
            }

            if (keyState.IsKeyDown(Keys.R) && canUse)
            {
                canUse = false;
                activated = true;
            }

            if (activated)
            {
                foreach (GameObject go in GameWorld.gameObjects)
                {
                    if (go.Tag == "HomingMissile" || go.Tag == "Fireball")
                    {
                        Circle playerCircle = new Circle();
                        playerCircle.Center = gameObject.transform.position;
                        playerCircle.Radius = (gameObject.GetComponent("Collider") as Collider).CircleCollisionBox.Radius * 2;

                        if (playerCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                        {
                            (go.GetComponent("Physics") as Physics).Velocity = (go.GetComponent("Physics") as Physics).Velocity * -1;
                        }
                    }
                }
            }
        }
    }
}