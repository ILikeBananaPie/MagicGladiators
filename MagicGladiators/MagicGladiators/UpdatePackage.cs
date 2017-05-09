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
        public Vector2 direction { get; set; }

        [ProtoMember(1)]
        private float[] xypos;

        [ProtoMember(2)]
        private float[] xydir;

        private UpdatePackage() { }

        /// <summary>
        /// Update package to communicate with server
        /// </summary>
        /// <param name="position">Your characters current position</param>
        public UpdatePackage(Vector2 position)
        {
            this.position = position;
            this.direction = Vector2.Zero;
        }
        /// <summary>
        /// Update package to communicate with server
        /// </summary>
        /// <param name="position">Your characters current position</param>
        /// <param name="direction">Your characters current direction. Should be normalized.</param>
        public UpdatePackage(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            if (position != null)
            {
                xypos = new float[2] { position.X, position.Y };
            }
            if (position != null)
            {
                xydir = new float[2] { direction.X, direction.Y };
            }
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            position = new Vector2(xypos[0], xypos[1]);
            direction = new Vector2(xydir[0], xydir[1]);
        }
    }
}
