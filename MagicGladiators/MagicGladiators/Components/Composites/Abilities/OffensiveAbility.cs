using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators.Components.Composites.Abilities
{
    abstract class OffensiveAbility:Component, ILoadable, IUpdateable
    {
        //Something akin to missiles mainly
        public OffensiveAbility(GameObject go) : base(go)
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
