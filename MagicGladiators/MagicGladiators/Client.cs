using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Client
    {
        static NetClient client;
        static List<GameObject> gameObjects;
        static System.Timers.Timer update;

        static string hostip = "localhost";

        public Client()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            client = new NetClient(config);
            NetOutgoingMessage outmsg = client.CreateMessage();
            client.Start();
            outmsg.Write((byte)PacketTypes.LOGIN);
            outmsg.Write("Myname");
            client.Connect(hostip, 14242, outmsg);
            gameObjects = new List<GameObject>();
            update = new System.Timers.Timer(50);
            update.Elapsed += new System.Timers.ElapsedEventHandler(update_Elapsed);
            WaitForStartingInfo();
            update.Start();


            /* send data
            while (true)
            {
                GetInputAndSendItToServer();
            }
            */
        }

        static void update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckServerMessages();

            //DrawGameState();
        }

        private static void WaitForStartingInfo()
        {
            bool canStart = false;
            NetIncomingMessage inc;

            while (!canStart)
            {
                if ((inc = client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Error:
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            break;
                        case NetIncomingMessageType.Data:
                            if (inc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                            {
                                int count = 0;
                                count = inc.ReadInt32();
                                gameObjects.Clear();
                                for (int i = 0; i < count; i++)
                                {
                                    GameObject go = new GameObject();
                                    inc.ReadAllProperties(go);
                                    gameObjects.Add(go);
                                }
                                canStart = true;
                            }
                            break;
                        case NetIncomingMessageType.Receipt:
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            break;
                        case NetIncomingMessageType.DiscoveryResponse:
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            break;
                        case NetIncomingMessageType.NatIntroductionSuccess:
                            break;
                        case NetIncomingMessageType.ConnectionLatencyUpdated:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private static void CheckServerMessages()
        {
            NetIncomingMessage inc;
            while ((inc = client.ReadMessage()) != null)
            {
                if (inc.MessageType == NetIncomingMessageType.Data)
                {
                    if (inc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                    {
                        gameObjects.Clear();
                        int jii = 0;
                        for (int i = 0; i < jii; i++)
                        {
                            GameObject go = new GameObject();
                            inc.ReadAllProperties(go);
                            gameObjects.Add(go);
                        }
                    }
                }
            }
        }
        private static void GetInputAndSendItToServer()
        {
            //velocity
            foreach (GameObject go in gameObjects)
            {
                Vector2 test = (go.GetComponent("Physics") as Physics).Velocity;
                float x = test.X;
                float y = test.Y;
                NetOutgoingMessage outmsg = client.CreateMessage();
                outmsg.Write((byte)x);
                outmsg.Write((byte)y);

            }
        }
    }
}
