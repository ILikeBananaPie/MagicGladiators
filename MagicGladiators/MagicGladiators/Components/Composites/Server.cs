using Microsoft.Xna.Framework;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace MagicGladiators.Components.Composites
{
    class Server : Component, IDrawable, ILoadable
    {
        private static Dictionary<Connection, GameObject> players;


        public Server(GameObject gameObject) : base(gameObject)
        {
            players = new Dictionary<Connection, GameObject>();

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("JoinServer", Test);

            NetworkComms.AppendGlobalIncomingPacketHandler<Vector2>("UpdatePosition", UpdatePosition);
            //Start listening for incoming connections
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

        }

        SpriteFont spriteFont;
        public void Draw(SpriteBatch spriteBatch)
        {
            int incriment = 0;
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            {
                spriteBatch.DrawString(spriteFont, localEndPoint.Address.ToString() + ":" + localEndPoint.Port.ToString(), new Vector2(20, 20 + incriment), Color.Black);
                incriment += 15;
            }
        }

        private static void JoinServer(PacketHeader header, Connection connection, string message)
        {
            if (players.Count <= 8)
            {
                if (!players.ContainsKey(connection))
                {
                    GameObject go = new GameObject(0);
                    connection.SendObject<bool>("JoinedServerRespond", true);
                }
                else
                {
                    connection.SendObject<bool>("JoinedServerRespond", false);
                }
            }
            else
            {
                connection.SendObject<bool>("JoinedServerRespond", false);
            }
        }

        private static void Test(PacketHeader header, Connection connection, string message)
        {
            Debug.Write("Eh");
        }

        private void UpdatePosition(PacketHeader header, Connection connection, Vector2 position)
        {

        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("Font");
        }
    }
}