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
        /// <summary>
        /// ldsjflksjd
        /// </summary>
        /// <param name="go"></param>
        //Something stationary or character based
        public DefensiveAbility(GameObject go) : base(go)
        {
        }
        //IDK im tired
        public abstract void LoadContent(ContentManager content);

        public abstract void Update();
    }
}
