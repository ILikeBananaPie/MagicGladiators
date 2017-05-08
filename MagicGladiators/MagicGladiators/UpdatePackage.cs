using Microsoft.Xna.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    [ProtoContract]
    public class UpdatePackage
    {
        public Vector2 position { get; set; }

        [ProtoMember(1)]
        private float[] xy;

        private UpdatePackage() { }

        public UpdatePackage(Vector2 position)
        {
            this.position = position;
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            if (position != null)
            {
                xy = new float[2] { position.X, position.Y };
            }
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            position = new Vector2(xy[0], xy[1]);
        }
    }
}
