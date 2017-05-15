using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    abstract class MobilityAbility:Component, ILoadable, IUpdateable
    {
        //Manipulating character movement, speed, velocity and/or position
        public MobilityAbility(GameObject go) : base(go)
        {
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void Update();
    }
}
