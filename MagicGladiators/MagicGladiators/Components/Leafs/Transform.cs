using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Transform:Component
    {
        public static Vector2 playerPosition;

        public static Vector2 EnemyPosition;

        private readonly Object thisLock = new Object();

        private Vector2 _position;
        public Vector2 position
        {
            get
            {
                lock (thisLock) { return _position; }
            }
            set
            {
                lock (thisLock) { _position = value; }
            }
        }


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
