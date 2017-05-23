using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    public abstract class Ability : Component, ILoadable, IUpdateable
    {
        protected int cooldown;
        protected bool canShoot;
        protected float cooldownTimer;


        public Ability(GameObject gameObject, float cooldown, bool canShoot) : base(gameObject)
        {
            
        }

        public abstract void LoadContent(ContentManager content);
        
           
        

        public abstract void Update();
       
            
                      
        
        public void Cooldown()
        {
            if (!canShoot)
            {
                cooldownTimer += GameWorld.Instance.deltaTime;
            }

            if (cooldownTimer > cooldown)
            {
                cooldownTimer = 0;
                canShoot = true;
            }
        }
        
           


    }
}
