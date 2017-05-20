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

        void BuildGameObject(Vector2 position);
        void BuildItem(Vector2 position, string[] stats);
        void BuildIcon(Vector2 position, string name, int value);
        void FireProjectile(Vector2 position, Vector2 targetVector, string ability);
        void BuildMapPart(Vector2 position, string name);
    }
}
