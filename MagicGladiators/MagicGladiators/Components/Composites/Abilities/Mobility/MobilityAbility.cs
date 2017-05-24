using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    public abstract class MobilityAbility: Ability
    {
        //Manipulating character movement, speed, velocity and/or position
        public MobilityAbility(GameObject go) : base(go)
        {
        }

        
    }
}
