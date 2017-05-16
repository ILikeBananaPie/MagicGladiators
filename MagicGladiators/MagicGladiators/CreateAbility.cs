using Microsoft.Xna.Framework;
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

        public Component GetComponent(GameObject gameObject, Vector2 position)
        {
            if (name == "HomingMissile")
            {
                component = new HomingMissile(gameObject, position, Vector2.Zero);
            }
            if (name == "Blink")
            {

            }
            if (name == "Charge")
            {
                component = new Charge(gameObject, gameObject.transform, (gameObject.GetComponent("Animator") as Animator));
            }
            if (name == "Ricochet")
            {

            }
            return component;
        }




    }
}
