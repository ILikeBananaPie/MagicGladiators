using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public struct Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(int x, int y, float radius)
        {
            Center = new Vector2(x, y);
            Radius = radius;
        }

        public bool Intersects(Circle other)
        {
            float lengthTest = (other.Center - Center).Length();
            float radiusTest = other.Radius + Radius;

            return ((other.Center - Center).Length() < (other.Radius + Radius));
        }
    }
}
