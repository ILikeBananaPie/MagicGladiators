using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class CreateAbility
    {
        private string name;
        private Component component;

        public CreateAbility(string name)
        {
            this.name = name;
        }

        public Component GetComponent()
        {
            if (name == "HomingMissile")
            {
                component = new HomingMissile(new GameObject());
            }
            if (name == "Blink")
            {

            }
            if (name == "Charge")
            {

            }
            if (name == "Ricochet")
            {

            }
            return component;
        }




    }
}
