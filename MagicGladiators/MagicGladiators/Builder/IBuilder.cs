using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    interface IBuilder
    {
        GameObject GetResult();

        void BuildGameObject(Vector2 position, object id);
        void FireProjectile(Vector2 position, Vector2 targetVector);
    }
}
