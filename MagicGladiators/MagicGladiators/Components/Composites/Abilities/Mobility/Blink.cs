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
       

        private Vector2 playerTemp;


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
                
                if (Vector2.Distance(go.transform.position, target) < 500)
                {
                    
                    gameObject.transform.position = target;
                    
                    activated = false;
                }
                if(Vector2.Distance(go.transform.position, target) > 500)
                {
                    playerTemp = go.transform.position;
                    Vector2 normalizedVectorBetween = (gameObject.GetComponent("Physics") as Physics).GetVector(playerTemp, target);
                    normalizedVectorBetween.Normalize();

                    while (Vector2.Distance(go.transform.position, target) > 500)
                    {
                        playerTemp += normalizedVectorBetween * 1000;
                    }
                    

                    activated = false;


                }

               
               
                
            }
         
        }
    }
}
