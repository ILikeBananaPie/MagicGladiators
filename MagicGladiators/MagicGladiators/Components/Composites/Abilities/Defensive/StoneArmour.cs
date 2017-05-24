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

       
        private float activated;
        private float activatedTimer = 4f;
        private float slowSpeed = 0.5f;
        private float resist = 0.5f;
        private bool activatedAbility;
        private bool cooldownbool = false;
        private float cooldownTimer;


        public StoneArmour(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 5;
            
        }

        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
           

            if (keyState.IsKeyDown(Keys.B) && canShoot)
            {
                activatedAbility = true;
                canShoot = false;
                gameObject.Speed -= slowSpeed;
                gameObject.KnockBackResistance -= resist;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkSlateGray;
               
            }
            if(activatedAbility)
            {
                activated += GameWorld.Instance.deltaTime;
            }

            if(activated >= activatedTimer)
            {
                gameObject.Speed +=  slowSpeed;
                gameObject.KnockBackResistance += resist;
                cooldownbool = true;
                activatedAbility = false;
                activated = 0;
                (gameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
            }
            if (cooldownbool)
            {
                cooldownTimer += GameWorld.Instance.deltaTime;
            }
            if(cooldownTimer >= cooldown)
            {
                canShoot = true;
                cooldownTimer = 0;
            }
        }
    }
}
