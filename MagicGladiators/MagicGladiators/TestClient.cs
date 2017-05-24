using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class TestClient
    {
        private NetClient client;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        public static string text = "";
        private static readonly object locker = new object();

        public TestClient()
        {
            string hostip = "192.168.1.206";
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
            font = GameWorld.Instance.Content.Load<SpriteFont>("fontText");
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            //config.Port = 24049;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(config);
            client.Start();
            client.Connect(hostip, 24049);
            //client.DiscoverLocalPeers(24049);
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write(text);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
        }
        public void SendPositions(Vector2 vector)
        {
            lock (locker)
            {
                float x = vector.X;
                float y = vector.Y;
                NetOutgoingMessage msgOut;
                msgOut = client.CreateMessage();
                msgOut.Write((byte)PacketType.PlayerPos);
                msgOut.Write(x);
                msgOut.Write(y);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            }

        }

        public void Draw()
        {
            lock (locker)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "You Dick!", new Vector2(0, GameWorld.Instance.Window.ClientBounds.Height / 2), Color.Black);
                spriteBatch.End();
            }
            Thread.CurrentThread.Abort();
        }

        public void Update()
        {
            NetIncomingMessage msgIn;
            while ((msgIn = client.ReadMessage()) != null)
            {
                //text = msgIn.ReadString();
                if (text.Contains("Connect"))
                {

                }
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        //text = msgIn.ReadString();
                        text = "Connected!";
                        NetOutgoingMessage msgOut;
                        msgOut = client.CreateMessage();
                        msgOut.Write((byte)PacketType.CreatePlayer);
                        client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);

                        break;
                    case NetIncomingMessageType.UnconnectedData:

                        break;
                    case NetIncomingMessageType.ConnectionApproval:

                        break;
                    case NetIncomingMessageType.Data:
                        //text = msgIn.ReadString();
                        byte type = msgIn.ReadByte();
                        if (type == (byte)PacketType.EnemyPos)
                        {
                            //string test = msgIn.ReadFloat().ToString();
                            //string[] arr = test.Split(',');
                            float x = msgIn.ReadFloat();
                            float y = msgIn.ReadFloat();

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy")
                                {
                                    go.transform.position = new Vector2(x, y);
                                }
                            }
                        }
                        if (type == (byte)PacketType.CreatePlayer)
                        {
                            GameObject go = new GameObject();
                            go.AddComponent(new Enemy(go));
                            go.AddComponent(new SpriteRenderer(go, "Player", 1));
                            go.Tag = "Enemy";
                            GameWorld.newObjects.Add(go);
                            foreach (GameObject dummy in GameWorld.gameObjects)
                            {
                                if (dummy.Tag == "Dummy")
                                {
                                    go.transform.position = dummy.transform.position;
                                    GameWorld.objectsToRemove.Add(dummy);
                                    break;
                                }
                            }
                        }

                        break;
                    case NetIncomingMessageType.Receipt:

                        break;
                    case NetIncomingMessageType.DiscoveryRequest:

                        break;
                    case NetIncomingMessageType.DiscoveryResponse:

                        //writeline server said: " ";
                        /*
                        text = msgIn.ToString();
                        client.Connect(msgIn.SenderEndPoint);
                        */
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        //text = msgIn.ReadString();
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        //text = msgIn.ReadString();
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        //text = msgIn.ReadString();
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        break;
                }
            }
            //Thread.CurrentThread.Abort();

        }
    }
}
