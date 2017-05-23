using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    public abstract class OffensiveAbility: Ability
    {
        //Something akin to missiles mainly
        public OffensiveAbility(GameObject go, float cooldown, bool canShoot) : base(go, cooldown, canShoot)
        {

        }

        

        
    }
}
