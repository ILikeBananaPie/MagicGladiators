using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    public abstract class Ability : Component, ILoadable, IUpdateable, IAbility
    {
        protected int cooldown;
        protected bool canShoot = true;
        protected float cooldownTimer;


        public Ability(GameObject gameObject) : base(gameObject)
        {

        }

        public abstract void LoadContent(ContentManager content);




        public abstract void Update();

        public void Cooldown()
        {
            if (!canShoot)
            {
               // gameObject.CooldownReduction * cooldown 
            }

            if (cooldownTimer > cooldown)
            {
                cooldownTimer = 0;
                canShoot = true;
            }
        }




    }
}
