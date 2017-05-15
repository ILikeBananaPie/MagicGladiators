using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace MagicGladiators
{
    class Charge : MobilityAbility, IUpdateable
    {

        private bool on = false;
        private bool activated = false;

        private Animator animator;
        private float timer;
        private GameObject go;
        private Transform transform;
        private Vector2 target;
        private float chargeTimer;

        private Vector2 test;

        private Physics physics;
        public Charge(GameObject go, Transform transform, Animator animator) : base(go)
        {
            this.go = go;
            this.animator = animator;      
            this.transform = transform;
            this.physics = (transform.gameObject.GetComponent("Physics") as Physics);
        }

        public override void LoadContent(ContentManager content)
        {
          
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();


            Vector2 translation = Vector2.Zero;

            //activate aim and move in a charge
            if (keyState.IsKeyDown(Keys.Space) && !on && !activated)
            {
                

                activated = true;
            }
            if(activated && keyState.IsKeyUp(Keys.Space))
            {
                target = new Vector2(mouse.Position.X, mouse.Position.Y);
                test = physics.GetVector(target, go.transform.position);
                test.Normalize();
                on = true;
            }
            if (on)
            {
                if (chargeTimer < 0.25)
                {
                    physics.Acceleration += test;
                    chargeTimer += (float)GameWorld.Instance.deltaTime;
                    
                }
                timer += (float)GameWorld.Instance.deltaTime;
                if (timer > 2)
                {
                    timer = 0;
                    chargeTimer = 0;
                    on = false;
                    activated = false;
                }
            }

            
        }
    }
}