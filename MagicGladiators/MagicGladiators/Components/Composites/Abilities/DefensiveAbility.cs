using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators.Components.Composites.Abilities
{
    abstract class DefensiveAbility:Component, ILoadable, IUpdateable
    {
        //Something stationary or character based
        public DefensiveAbility(GameObject go) : base(go)
        {
        }

        public void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
