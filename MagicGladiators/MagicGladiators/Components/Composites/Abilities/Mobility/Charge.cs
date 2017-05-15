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
        

        private Animator animator;
        private float timer;
        private GameObject gameObject;
        private Transform transform;
        private Vector2 originalPos;
        private Vector2 testVector;
        private int speed;
        private Vector2 target;
        private int chargeTimer;

        private Physics physics;
        public Charge(GameObject go, Transform transform, Animator animator) : base(go)
        {
            go = gameObject;
            this.animator = animator;
            //this.speed = 10;
            this.target = target;
            // testVector = target - originalPos;
            //testVector.Normalize();
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

            timer += (float)GameWorld.Instance.deltaTime;
            if (timer > 1)
            {
                
                timer = 0;
                chargeTimer++;
                on = false;
            }
            Vector2 translation = Vector2.Zero;
          

            //activate aim and move in a charge

            if (keyState.IsKeyDown(Keys.Space) && !on)
            {
                target = new Vector2(mouse.Position.X, mouse.Position.Y);
                on = true;
            }
            else if ( keyState.IsKeyUp(Keys.Space) && on && (chargeTimer % 2 == 0))
            {
               
               if(transform.position.Y - mouse.Position.Y > 0)
                {
                    physics.Acceleration += new Vector2(0, -1.25F);
                    if(transform.position.Y == mouse.Position.Y)
                    {
                        physics.Acceleration += new Vector2(0, -1.25F);
                    }
                }

                else if (transform.position.Y - mouse.Position.Y < 0)
                {
                    physics.Acceleration += new Vector2(0, 1.25F);
                    if (transform.position.Y == mouse.Position.Y)
                    {
                        physics.Acceleration += new Vector2(0, 1.25F);
                    }
                }
                if (transform.position.X - mouse.Position.X > 0)
                {
                    physics.Acceleration += new Vector2(-1.25f, 0);
                    if (transform.position.X == mouse.Position.X)
                    {
                        physics.Acceleration += new Vector2(-1.25f, 0);
                    }
                }
                else if (transform.position.X - mouse.Position.X < 0)
                {
                    physics.Acceleration += new Vector2(1.25f, 0);
                    if (transform.position.X == mouse.Position.X)
                    {
                        physics.Acceleration += new Vector2(1.25f, 0);
                    }
                }
                
               
            }
            
               
                
                

                
                
            }

           

        }

    }








    


