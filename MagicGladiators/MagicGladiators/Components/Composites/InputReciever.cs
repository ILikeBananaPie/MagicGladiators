using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators.Components.Composites
{
    class InputReciever:Component
    {


        public InputReciever(GameObject gameObject) : base(gameObject)
        {

        }

        public void UpdatePosition(Vector2 pos)
        {
            gameObject.transform.position = pos;
        }
    }
}
