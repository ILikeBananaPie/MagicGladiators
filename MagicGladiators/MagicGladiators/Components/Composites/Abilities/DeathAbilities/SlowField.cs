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
    class SlowField : Component
    {
        private bool canUse = true;
        private bool activated = false;
        private bool use = false;
        private float cooldownTimer;
        private float cooldown = 5;

        private float speedFactor = 0.5f;

        private float activationTime = 2;
        private float activationTimer;

        private float oldSpeed;

        public SlowField(GameObject go)
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

            if (keyState.IsKeyDown(Keys.O) && !activated )
            {
                foreach (var go in GameWorld.gameObjects)
                {
                    if (go.Tag != "Ability")
                    {
                        oldSpeed = go.Speed;
                    }

                }
                
                canUse = false;
                activated = true;
                Color color = new Color();
                color.A = 20;
                
            }
            if (activated)
            {
                if (!use)
                {
                    use = true;
                    foreach (var go in GameWorld.gameObjects)
                    {
                        if(go.Tag != "Ability")
                        {
                            go.Speed = go.Speed - speedFactor;
                        }
                        
                    }
                    
                   
                }
                activationTimer += GameWorld.Instance.deltaTime;
                if (activationTimer > activationTime)
                {
                    foreach (var go in GameWorld.gameObjects)
                    {
                        if (go.Tag != "Ability")
                        {
                            go.Speed += speedFactor;
                        }

                    }
                    
                    activated = false;
                    activationTimer = 0;
                    
                }
            }
        }
    }
}
