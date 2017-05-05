using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public abstract class Component
    {

        public GameObject gameObject { get; private set; }


        public Component(GameObject gameObject)
        {

            this.gameObject = gameObject;
        }

        public Component()
        { }
    }
}
