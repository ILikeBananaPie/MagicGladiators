using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicGladiators;

namespace TestServer
{
    class Program
    {
        private static NetServer server;
        private static List<NetConnection> connections = new List<NetConnection>();

        public static void SendConnection()
        {
            if (connections.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.CreatePlayer);
                //msgOut.Write("test");
                server.SendMessage(msgOut, connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

        }

        public static void SendPosition(NetConnection con, float x, float y)
        {

            foreach (NetConnection con2 in server.Connections)
            {
                connections.Add(con2);
            }
            connections.Remove(con);

            if (connections.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.EnemyPos);
                //int[] test = new int[2] { 0, 1 };
                msgOut.Write(x);
                msgOut.Write(y);
                server.SendMessage(msgOut, connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

        }

        static void Main(string[] args)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            config.Port = 24049;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            //config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);
            server.Start();

            while (true)
            {
                NetIncomingMessage msgIn;
                NetOutgoingMessage msgOut;
                while ((msgIn = server.ReadMessage()) != null)
                {
                    switch (msgIn.MessageType)
                    {
                        case NetIncomingMessageType.Error:
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            break;
                        case NetIncomingMessageType.ConnectionApproval:

                            //Someone is trying to connect to server. Check for password
                            msgIn.SenderConnection.Approve();
                            Console.WriteLine("Player Connected!");

                            //server response to the connecting client (doesn't work, the connect hasn't been established yet)
                            /*
                            SendConnection();
                            msgOut = server.CreateMessage();
                            msgOut.Write("Connection Approved");
                            server.SendMessage(msgOut, msgIn.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                            */
                            break;
                        case NetIncomingMessageType.Data:
                            //TestClient.text = msgIn.ReadString();
                            byte type = msgIn.ReadByte();
                            if (type == (byte)PacketType.PlayerPos)
                            {
                                //TestClient.text = "(" + msgIn.ReadFloat().ToString(".");
                                //TestClient.text += ", " + msgIn.ReadFloat().ToString(".") + ")";
                                //string test = msgIn.ReadFloat().ToString(".");
                                //test += "," + msgIn.ReadFloat().ToString(".");
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();

                                //UpdateConnectionList(msgIn.SenderConnection);
                                SendPosition(msgIn.SenderConnection, x, y);
                            }
                            if (type == (byte)PacketType.CreatePlayer)
                            {
                                SendConnection();
                            }

                            break;
                        case NetIncomingMessageType.DebugMessage:
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            break;
                        case NetIncomingMessageType.ConnectionLatencyUpdated:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
