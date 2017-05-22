using Microsoft.Xna.Framework;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    [ProtoContract]
    class ServerPackage
    {
        public Dictionary<string, Vector2[]> players { get; set; }

        [ProtoMember(1)]
        private float[] xypos;

        [ProtoMember(2)]
        private float[] xyvel;

        [ProtoMember(3)]
        private string[] ci;

        private ServerPackage() { }

        public ServerPackage(Dictionary<Connection, GameObject> so)
        {
            players = new Dictionary<string, Vector2[]>();
            foreach (Connection key in so.Keys)
            {
                Vector2[] input;
                if (so[key].GetComponent("Enemy") is Enemy)
                {
                    input = new Vector2[2] { so[key].transform.position, (so[key].GetComponent("Enemy") as Enemy).velocity };
                } else
                {
                    input = new Vector2[2] { so[key].transform.position, (so[key].GetComponent("Physics") as Physics).Velocity };
                }
                players.Add(key.ConnectionInfo.ToString(), input);
            }
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            int length = players.Count;
            xypos = new float[length * 2];
            xyvel = new float[length * 2];
            ci = new string[length];

            int current = 0;
            foreach (string key in players.Keys)
            {
                ci[current / 2] = key;
                xypos[current] = players[key][0].X;
                xyvel[current] = players[key][1].X;
                current++;
                xypos[current] = players[key][0].Y;
                xyvel[current] = players[key][1].Y;
                current++;
            }
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            players = new Dictionary<string, Vector2[]>();
            int length = ci.Length;
            int current = 0;
            while (current < length * 2)
            {
                string t = ci[current / 2];
                Vector2 pos = new Vector2();
                Vector2 vel = new Vector2();
                pos.X = xypos[current];
                vel.X = xyvel[current];
                current++;
                pos.X = xypos[current];
                vel.X = xyvel[current];
                players.Add(t, new Vector2[2] {pos, vel });
                current++;
            }
        }
    }
}
