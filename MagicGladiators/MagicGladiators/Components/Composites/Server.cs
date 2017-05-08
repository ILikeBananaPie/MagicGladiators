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
using System.Net;
using NetworkCommsDotNet.Connections.TCP;

namespace MagicGladiators.Components.Composites
{
    class Server : Component, IDrawable, ILoadable, IUpdateable
    {
        private static Dictionary<Connection, GameObject> players;
        private Transform playerPos;


        public Server(GameObject gameObject) : base(gameObject)
        {
            players = new Dictionary<Connection, GameObject>();

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("JoinServer", JoinServer);

            NetworkComms.AppendGlobalIncomingPacketHandler<UpdatePackage>("UpdatePosition", UpdatePosition);
            //Start listening for incoming connections
            
            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 0));

            playerPos = (GameWorld.gameObjects.Find(x => x.Tag == "Player").GetComponent("Transform") as Transform);

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
                    go.Tag = connection.ToString();
                    go.AddComponent(new InputReciever(go));
                    go.AddComponent(new SpriteRenderer(go, "Player", 1));
                    go.LoadContent(GameWorld.Instance.Content);
                    players.Add(connection, go);
                    GameWorld.gameObjects.Add(go);
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

        private void UpdatePosition(PacketHeader header, Connection connection, UpdatePackage position)
        {
            ((players[connection] as GameObject).GetComponent("InputReciever") as InputReciever).UpdatePosition(position.position);
            connection.SendObject<UpdatePackage>("HostPos", new UpdatePackage(playerPos.position));
        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("Font");
        }

        public void Update()
        {
            foreach (Connection key in players.Keys)
            {
                key.SendObject<UpdatePackage>("HostPos", new UpdatePackage(playerPos.position));
            }
        }
    }
}