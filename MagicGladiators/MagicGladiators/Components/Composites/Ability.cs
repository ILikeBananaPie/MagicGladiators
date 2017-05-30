using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    public enum actions { Fireball, HomingMissile, Drain, Chain }

    public abstract class Ability : Component, ILoadable, IUpdateable, IAbility
    {
        protected float cooldown;
        protected bool canShoot = true;
        protected float cooldownTimer;
        private bool canUse;


        public Ability(GameObject gameObject) : base(gameObject)
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

            if (cooldownTimer > cooldown * gameObject.CooldownReduction)
            {
                cooldownTimer = 0;
                canShoot = true;
            }

        }
    }
}
