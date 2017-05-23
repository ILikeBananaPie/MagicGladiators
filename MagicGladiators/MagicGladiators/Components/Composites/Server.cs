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
using System.Threading;
using NetworkCommsDotNet.Connections.TCP;

namespace MagicGladiators.Components.Composites
{
    class Server:Component, IDrawable, ILoadable, IUpdateable
    {
        private static readonly Object thislock = new Object();
        private static Dictionary<Connection, GameObject> _players;
        private static Dictionary<Connection, GameObject> players
        {
            get { lock (thislock) { return _players; } }
            set { lock (thislock) { _players = value; } }
        }
        private static Dictionary
        private Player playerPos;

        TCPConnection oneself;
        public Server(GameObject gameObject) : base(gameObject)
        {
            players = new Dictionary<Connection, GameObject>();

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("JoinServer", JoinServer);

            NetworkComms.AppendGlobalIncomingPacketHandler<UpdatePackage>("UpdatePosition", UpdatePosition);

            //Start listening for incoming connections

            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 0));

            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            {
                if (localEndPoint.Address.ToString().Contains("127."))
                {
                    oneself = TCPConnection.GetConnection(new ConnectionInfo(localEndPoint));
                }
            }
            Addself();
        }

        private void Addself()
        {
            Debug.Write("Addself started");
            Director dir = new Director(new PlayerBuilder());
            GameObject go = dir.Construct(new Vector2(50), 2);
            players.Add(oneself, go);
            GameWorld.newObjects.Add(go);
            go.LoadContent(GameWorld.Instance.Content);
            playerPos = (GameWorld.gameObjects.Find(x => x.Tag == "Player").GetComponent("Player") as Player);

        }

        //private void TestFunction(Connection conn)
        //{
        //    ConnectionInfo connInfo  = conn.ConnectionInfo;
        //    connInfo.
        //}

        SpriteFont spriteFont;
        public void Draw(SpriteBatch spriteBatch)
        {
            int incriment = 0;
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            {
                spriteBatch.DrawString(spriteFont, localEndPoint.Address.ToString() + ":" + localEndPoint.Port.ToString(), new Vector2(150, 10 + incriment), Color.Black);
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
                    go.AddComponent(new Enemy(go));
                    go.AddComponent(new SpriteRenderer(go, "Player", 1));
                    go.LoadContent(GameWorld.Instance.Content);
                    players.Add(connection, go);
                    GameWorld.newObjects.Add(go);
                    connection.SendObject<TryConnectPackage>("TryConnect", new TryConnectPackage(true, connection.ConnectionInfo));
                } else
                {
                    connection.SendObject<TryConnectPackage>("TryConnect", new TryConnectPackage(false, connection.ConnectionInfo));
                }
            } else
            {
                connection.SendObject<TryConnectPackage>("TryConnect", new TryConnectPackage(false, connection.ConnectionInfo));
            }
        }

        private void UpdatePosition(PacketHeader header, Connection connection, UpdatePackage position)
        {
            ((players[connection] as GameObject).GetComponent("Enemy") as Enemy).UpdateEnemyInfo(position);

        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("fontText");
        }

        public void Update()
        {
            if (threadUpdate == null)
            {
                threadUpdate = new Thread(ThreadUpdate);
                threadUpdate.Start();
            } else if (!threadUpdate.IsAlive)
            {
                threadUpdate.Start();
            }
        }

        private Thread threadUpdate;
        public void ThreadUpdate()
        {
            Dictionary<Connection, GameObject> dicofplayers;
            while (true)
            {
                dicofplayers = new Dictionary<Connection, GameObject>(players);
                ServerPackage up = new ServerPackage(dicofplayers);
                foreach (Connection key in dicofplayers.Keys)
                {
                    if (key != oneself)
                    {
                        try
                        {
                            key.SendObject<ServerPackage>("ServerPackage", up);
                        } catch (CommunicationException ce)
                        {
                            GameWorld.objectsToRemove.Add(dicofplayers[key]);
                            players.Remove(key);
                        }
                    }
                }
            }
        }
    }
}