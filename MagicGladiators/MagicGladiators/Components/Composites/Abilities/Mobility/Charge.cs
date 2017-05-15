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
        //dsjkfhsdkjf

        private Animator animator; //test 

        private IStrategy strategy;

        private DIRECTION direction;

        private GameObject go;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;

        private Vector2 target;


        private Physics physics;
        public Charge(GameObject go, Transform transform, Animator animator) : base(go)
        {

            this.animator = animator;
            this.transform = transform;
            this.physics = (transform.gameObject.GetComponent("Physics") as Physics);
        }

        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();



            Vector2 translation = originalPos;


            //activate aim and move in a charge

            if (keyState.IsKeyDown(Keys.Space) && !on)
            {
                on = true;
            }
            else if ( keyState.IsKeyDown(Keys.Space) && on)
            {
                on = false;
            }
            if(on && mouse.LeftButton == ButtonState.Pressed)
            {
                physics.Acceleration += new Vector2(1.25F, 0);
            }
               // physics.Acceleration += new Vector2(1.25F, 0);
                /*if ()
                {

                }
                */
            }

            //animator.PlayAnimation("ChargeRight");

        }

    }








    


