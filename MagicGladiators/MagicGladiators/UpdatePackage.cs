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
        public Vector2 velocity { get; set; }

        [ProtoMember(1)]
        private float[] xypos;

        [ProtoMember(2)]
        private float[] xyvel;

        private UpdatePackage() { }

        /// <summary>
        /// Update package to communicate with server
        /// </summary>
        /// <param name="position">Your characters current position</param>
        public UpdatePackage(Vector2 position)
        {
            this.position = position;
            this.velocity = Vector2.Zero;
        }
        /// <summary>
        /// Update package to communicate with server
        /// </summary>
        /// <param name="position">Your characters current position</param>
        /// <param name="velocity">Your characters current direction. Should be normalized.</param>
        public UpdatePackage(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
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
                xyvel = new float[2] { velocity.X, velocity.Y };
            }
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            position = new Vector2(xypos[0], xypos[1]);
            velocity = new Vector2(xyvel[0], xyvel[1]);
        }

        public void InfoUpdate(Vector2 position)
        {
            this.position = position;
            this.velocity = Vector2.Zero;
        }
        public void InfoUpdate(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }
    }
}
