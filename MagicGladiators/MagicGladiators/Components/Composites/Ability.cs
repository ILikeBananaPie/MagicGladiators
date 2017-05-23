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
        private int cooldown;
        private bool canShoot;
        private float cooldownTimer;


        public Ability(GameObject gameObject, float cooldownTimer, bool canShoot) : base(gameObject)
        {

        }

        public void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

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
