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

        public Dictionary<string, ProjectileInfo[]> projectiles { get; set; }

        [ProtoMember(1)]
        private float[] xypos;

        [ProtoMember(2)]
        private float[] xyvel;

        [ProtoMember(3)]
        private string[] ci;

        [ProtoMember(4)]
        private string[] pname;

        [ProtoMember(5)]
        private float[] pxypos;

        [ProtoMember(6)]
        private float[] pxyvel;

        [ProtoMember(7)]
        private int[] pskip;

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

            if (projectiles != null)
            {
                pskip = new int[projectiles.Count];
                length = 0;
                foreach (string key in projectiles.Keys)
                {
                    pskip[current] = length;
                    length += projectiles[key].Length;
                    current++;
                }
                pname = new string[length];
                pxypos = new float[length * 2];
                pxyvel = new float[length * 2];

                current = 0;
                foreach (string key in projectiles.Keys)
                {
                    foreach (ProjectileInfo pi in projectiles[key])
                    {
                        pname[current / 2] = pi.ProjectileName;
                        pxypos[current] = pi.Position.X;
                        pxyvel[current] = pi.Velocity.X;
                        current++;
                        pxypos[current] = pi.Position.X;
                        pxyvel[current] = pi.Velocity.X;
                        current++;
                    }
                }
            }


            current = 0;
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
                pos.Y = xypos[current];
                vel.Y = xyvel[current];
                players.Add(t, new Vector2[2] { pos, vel });
                current++;
            }

            if (pname != null && pxypos != null && pxyvel != null && pskip != null)
            {
                current = 0;
                length = pxypos.Length;
                int posa = 0;
                while (current < length)
                {
                    if (current < pskip[posa])
                    {

                    } else
                    {
                        if (posa + 1 < pskip.Length)
                        {
                            posa++;
                        }
                    }
                }
            }
        }
    }
}