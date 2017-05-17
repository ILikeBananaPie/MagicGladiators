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
        public Dictionary<ConnectionInfo, Vector2[]> players { get; set; }

        [ProtoMember(1)]
        private float[,] xypos;

        [ProtoMember(2)]
        private float[,] xyvel;

        [ProtoMember(3)]
        private MemoryStream[] ms;

        private ServerPackage() { }

        public ServerPackage(Dictionary<Connection, GameObject> so)
        {
            players = new Dictionary<ConnectionInfo, Vector2[]>();
            foreach (Connection key in so.Keys)
            {
                Vector2[] input = new Vector2[2] {so[key].transform.position, (so[key].GetComponent("Physics") as Physics).Velocity};
                players.Add(key.ConnectionInfo, input);
            }
        }

        [ProtoBeforeSerialization]
        private void Serialize()
        {
            int length = players.Count;
            xypos = new float[length, 2];
            xyvel = new float[length, 2];
            ms = new MemoryStream[length];
            int current = 0;
            foreach (ConnectionInfo key in players.Keys)
            {
                xypos[current, 0] = players[key][0].X;
                xypos[current, 1] = players[key][0].Y;
                xyvel[current, 0] = players[key][1].X;
                xyvel[current, 1] = players[key][1].Y;
                key.Serialize(ms[current]);
                current++;
            }
        }

        [ProtoAfterDeserialization]
        private void Deserialize()
        {
            players = new Dictionary<ConnectionInfo, Vector2[]>();
            int length = ms.Length;
            for (int i = 0; i < length; i++)
            {
                ConnectionInfo connInfo;
                ConnectionInfo.Deserialize(ms[i], out connInfo);
                players.Add
                    (
                    connInfo,
                    new Vector2[2]
                    {
                        new Vector2(xypos[i,0], xypos[i,1]),
                        new Vector2(xyvel[i,0], xyvel[i,1])
                    }
                    );
            }
        }
    }
}
