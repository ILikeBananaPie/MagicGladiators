using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class IceField : Component
    {
        private bool canUse = true;
        private bool activated = false;
        private bool use = false;
        private float cooldownTimer;
        private float cooldown = 5;

        public bool test = false;

        private float speedFactor = 0.5f;

        private float activationTime = 2;
        private float activationTimer;

        private Vector2 testVector;

        private float oldSpeed;

        private Physics physics;

        public IceField(GameObject go)
        {
            
            
        }



        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            cooldownTimer += GameWorld.Instance.deltaTime;

            if (cooldownTimer > cooldown)
            {
                canUse = true;
                cooldownTimer = 0;
                use = false;
            }

            if (keyState.IsKeyDown(Keys.I) && !activated && !test)
            {
                foreach (var go in GameWorld.gameObjects)
                {
                    if (go.Tag != "Ability" && go.Tag != "Map" && go.Tag != "Pillar")
                    {

                        (go.GetComponent("Physics") as Physics).breakFactor = 0.001F;
                        //(go.GetComponent("Physics") as Physics).Ice = true;
                        //(go.GetComponent("Physics") as Physics).test = 0.9F;
                    }

                }

                canUse = false;
                activated = true;
                
                

            }
            if (activated)
            {
                if (!use)
                {
                    use = true;
                    foreach (var go in GameWorld.gameObjects)
                    {
                        if (go.Tag != "Ability")
                        {
                            
                        }

                    }


                }
                activationTimer += GameWorld.Instance.deltaTime;
                if (activationTimer > activationTime)
                {
                    foreach (var go in GameWorld.gameObjects)
                    {
                        if (go.Tag != "Ability" && go.Tag != "Map" && go.Tag != "Pillar")
                        {
                            (go.GetComponent("Physics") as Physics).breakFactor = 0.050F;
                            //(go.GetComponent("Physics") as Physics).test = 1;
                            //(go.GetComponent("Physics") as Physics).Ice = false;

                        }

                    }

                    activated = false;
                    activationTimer = 0;

                }
            }
        }
    }
}
