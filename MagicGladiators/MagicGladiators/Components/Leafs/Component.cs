using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators
{
    public abstract class Component : IDrawable
    {
        public Keys key { get; set; }
        public int abilityIndex { get; set; }
        public string Name { get; set; }

        public GameObject gameObject { get; private set; }


        public Component(GameObject gameObject)
        {

            this.gameObject = gameObject;
        }

        public Component()
        { }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
