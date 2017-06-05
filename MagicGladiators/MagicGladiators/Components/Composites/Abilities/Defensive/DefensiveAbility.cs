using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    abstract class DefensiveAbility: Ability, IUpdateable
    {
       
        //Something stationary or character based
        public DefensiveAbility(GameObject go) : base(go)
        {
        }
        
       
    }
}
