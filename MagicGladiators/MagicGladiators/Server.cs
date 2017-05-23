using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    enum PacketTypes { LOGIN, MOVE, WORLDSTATE }
    class Server
    {
        static NetServer server;
        static NetPeerConfiguration config;

        public Server()
        {
            config = new NetPeerConfiguration("game");
            config.Port = 14242;
            config.MaximumConnections = 8;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);
            server.Start();

            NetIncomingMessage inc;
            DateTime time = DateTime.Now;
            TimeSpan timetopass = new TimeSpan(0, 0, 0, 0, 30);

            while ((inc = server.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Connection == inc.SenderConnection)
                                {
                                    GameWorld.objectsToRemove.Add(go);
                                }
                            }
                        }
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
                        {
                            inc.SenderConnection.Approve();
                            Random r = new Random();

                            //create new gameobject (player)

                            NetOutgoingMessage outmsg = server.CreateMessage();
                            outmsg.Write((byte)PacketTypes.WORLDSTATE);

                            outmsg.Write(GameWorld.gameObjects.Count);

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                outmsg.WriteAllProperties(go);
                            }

                            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                        }
                        break;
                    case NetIncomingMessageType.Data:
                        if (inc.ReadByte() == (byte)PacketTypes.MOVE)
                        {
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Connection != inc.SenderConnection) continue;

                                byte b = inc.ReadByte();


                                NetOutgoingMessage outmsg = server.CreateMessage();
                                outmsg.Write((byte)PacketTypes.WORLDSTATE);
                                outmsg.Write(GameWorld.gameObjects.Count);
                                foreach (GameObject go2 in GameWorld.gameObjects)
                                {
                                    outmsg.WriteAllProperties(go2);
                                }
                                server.SendMessage(outmsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                            }
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
                if ((time + timetopass) < DateTime.Now)
                {
                    if (server.ConnectionsCount != 0)
                    {
                        NetOutgoingMessage outmsg = server.CreateMessage();
                        outmsg.Write((byte)PacketTypes.WORLDSTATE);
                        outmsg.Write(GameWorld.gameObjects.Count);
                        foreach (GameObject go in GameWorld.gameObjects)
                        {
                            outmsg.WriteAllProperties(go);
                        }
                        server.SendMessage(outmsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                    }
                    time = DateTime.Now;
                }
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
