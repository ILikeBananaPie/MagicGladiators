using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Transform : Component
    {
        public static Vector2 playerPosition;

        public static Vector2 EnemyPosition;

        public Vector2 position { get; set; }


        public Transform(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            this.position = position;
        }

        public void Translate(Vector2 translation)
        {
            position += translation;

        }
    }
}
