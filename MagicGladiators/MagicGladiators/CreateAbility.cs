using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        private static int abilityIndex = 0;
        public static Keys[] keys = new Keys[7] { Keys.Q, Keys.E, Keys.R, Keys.F, Keys.Space, Keys.C, Keys.X };

        public CreateAbility(string name)
        {
            this.name = name;
        }

        public static T ConvertToType<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public Component GetComponent(GameObject gameObject, Vector2 position)
        {
            if (name == "HomingMissile")
            {
                component = new HomingMissile(gameObject, position, Vector2.Zero);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Blink")
            {
                component = new Blink(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Charge")
            {
                component = new Charge(gameObject, gameObject.transform, (gameObject.GetComponent("Animator") as Animator));
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Drain")
            {
                component = new Drain(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Deflect")
            {
                component = new Deflect(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Mine")
            {
                component = new Mine(gameObject, gameObject.transform.position);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "SpeedBoost")
            {
                component = new SpeedBoost(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Chain")
            {
                component = new Chain(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if(name.Contains("Nova"))
            {
                component = new Nova(gameObject, gameObject.transform.position, Vector2.Zero);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name.Contains("Spellshield"))
            {
                component = new Spellshield(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if(name == "StoneArmour")
            {
                component = new StoneArmour(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Boomerang")
            {
                component = new Boomerang(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if (name == "Recall")
            {
                component = new Recall(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            if ( name == "GravityWell")
            {
                component = new GravityWell(gameObject);
                component.abilityIndex = abilityIndex;
                component.key = keys[abilityIndex];
                component.Name = name;
                abilityIndex++;
            }
            return component;
        }




    }
}
