using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagicGladiators
{
    //public enum PacketType { Position, Velocity, PlayerPos, EnemyPos, CreatePlayer }


    class TestServer
    {
        private NetServer server;
        private List<NetConnection> connections = new List<NetConnection>();
        private static readonly object locker = new object();
        private static readonly object locker2 = new object();

        public TestServer()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            config.Port = 24049;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            //config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);
            server.Start();
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write(text);

            server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void SendPosition(NetConnection con, float x, float y)
        {
            lock (locker)
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
        }

        public void SendConnection()
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

        public void UpdateConnectionList(NetConnection con)
        {

        }

        public void Update()
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
                            int x = msgIn.ReadInt32();
                            int y = msgIn.ReadInt32();

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
            //Thread.CurrentThread.Abort();
        }
    }
}
