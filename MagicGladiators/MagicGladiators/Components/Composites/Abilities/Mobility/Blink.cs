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
    class Blink : MobilityAbility, IUpdateable
    {
        private  bool on = false;
        private bool activated = false;

        private Animator animator;
        private GameObject go;
        private Transform transform;
        private Vector2 target;

        private Physics physics;
        public Blink(GameObject go, Transform transform, Animator animator) : base(go)
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

          
            if (keyState.IsKeyDown(Keys.Space) && !activated)
            {

                
                activated = true;
            }
            if (activated && keyState.IsKeyUp(Keys.Space))
            {
                target = new Vector2(mouse.Position.X, mouse.Position.Y);
                
                if (Vector2.Distance(go.transform.position, target) < 250)
                {
                    Vector2 oldPos = gameObject.transform.position;
                    
                    gameObject.transform.position = target;

                    float distance = Vector2.Distance(gameObject.transform.position, oldPos);
                    
                    activated = false;
                }
                if(Vector2.Distance(go.transform.position, target) > 250)
                {
                    Vector2 VectorBetween = (gameObject.GetComponent("Physics") as Physics).GetVector(target, gameObject.transform.position);
                    VectorBetween.Normalize();


                    Vector2 playerTemp = go.transform.position;
                    Vector2 oldPos = playerTemp;

                    //float test = Vector2.Distance(target, playerTemp);

                    while (Vector2.Distance(playerTemp, oldPos) < 250)
                    {
                        //float test2 = Vector2.Distance(target, playerTemp);
                        playerTemp += VectorBetween * 10;
                    }
                    gameObject.transform.position = playerTemp;
                    float test3 = Vector2.Distance(gameObject.transform.position, oldPos);

                    activated = false;


                }

               
               
                
            }
         
        }
    }
}
