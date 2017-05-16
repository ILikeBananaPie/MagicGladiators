using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{

    class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject Construct(Vector2 position)
        {
            builder.BuildGameObject(position);

            return builder.GetResult();
        }

        public GameObject ConstructProjectile(Vector2 position, Vector2 target, string ability)
        {
            builder.FireProjectile(position, target, ability);
            return builder.GetResult();
        }

      
    }
}
