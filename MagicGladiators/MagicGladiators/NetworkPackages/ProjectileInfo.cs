using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class ProjectileInfo
    {
        public string ProjectileName { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public ProjectileInfo(string name, Vector2 position, Vector2 velocity)
        {
            ProjectileName = name;
            Position = position;
            Velocity = velocity;
        }
    }
}
