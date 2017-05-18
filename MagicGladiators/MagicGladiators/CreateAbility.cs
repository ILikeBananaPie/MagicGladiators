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
                component = new Blink(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator);
            }
            if (name == "Charge")
            {
                component = new Charge(gameObject, gameObject.transform, (gameObject.GetComponent("Animator") as Animator));
            }
            if (name == "Drain")
            {
                component = new Drain(gameObject);
            }
            if (name == "Deflect")
            {
                component = new Deflect(gameObject);
            }
            if (name == "Mine")
            {
                component = new Mine(gameObject, gameObject.transform.position);
            }
            if (name == "SpeedBoost")
            {
                component = new SpeedBoost(gameObject);
            }
            if (name == "Chain")
            {
                component = new Chain(gameObject);
            }
            if(name.Contains("Nova"))
            {
                component = new Nova(gameObject, gameObject.transform.position, Vector2.Zero);
            }
            return component;
        }




    }
}
