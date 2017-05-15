using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Item
    {
        private string name;
        private int health;
        private int speed;
        private int damageResistance;
        private int lavaResistance;


        public Item(string name, int health, int speed, int damageResistance, int lavaResistance)
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.damageResistance = damageResistance;
            this.lavaResistance = lavaResistance;
        }


    }
}
