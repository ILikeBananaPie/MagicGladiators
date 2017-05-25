using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicGladiators;
using Microsoft.Xna.Framework;

namespace TestServer
{

    class Program
    {
        private static NetServer server;
        private static List<NetConnection> connectionList = new List<NetConnection>();

        public static void SendConnection()
        {
            if (server.Connections.Count > 1)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.CreatePlayer);
                msgOut.Write("test");
                server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

        }

        public static void UpdateConnectionList(NetConnection con)
        {
            connectionList.Clear();
            foreach (NetConnection con2 in server.Connections)
            {
                connectionList.Add(con2);
            }
            connectionList.Remove(con);
        }

        public static void SendPosition(NetConnection con, int x, int y)
        {

            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.EnemyPos);
                //int[] test = new int[2] { 0, 1 };
                msgOut.Write(x);
                msgOut.Write(y);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                //Console.WriteLine("Sending (" + x + ", " + y + ") to players: ");
                for (int i = 0; i < connectionList.Count; i++)
                {
                    //Console.Write(connectionList[i].ToString() + ", ");
                }
            }
        }
        public static void SendVelocity(float x, float y)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write(x);
            msgOut.Write(y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void SendProjectile(string name, Vector2 position, Vector2 velVector)
        {
            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                if (name == "FireballCreate")
                {
                    float posX = position.X;
                    float posY = position.Y;
                    float velX = velVector.X;
                    float velY = velVector.Y;
                    msgOut.Write((byte)PacketType.FireballCreate);
                    msgOut.Write(name);
                    msgOut.Write(posX);
                    msgOut.Write(posY);
                    msgOut.Write(velX);
                    msgOut.Write(velY);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
                if (name.Contains("Update"))
                {
                    name = name.Split(',').First();
                    msgOut.Write((byte)PacketType.FireballUpdate);
                    msgOut.Write(name);
                    msgOut.Write(position.X);
                    msgOut.Write(position.Y);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
                if (name == "HomingCreate")
                {
                    float posX = position.X;
                    float posY = position.Y;
                    float velX = velVector.X;
                    float velY = velVector.Y;
                    msgOut.Write((byte)PacketType.HomingCreate);
                    msgOut.Write(name);
                    msgOut.Write(posX);
                    msgOut.Write(posY);
                    msgOut.Write(velX);
                    msgOut.Write(velY);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }
        }


        static void Main(string[] args)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            config.Port = 51234;
            config.MaximumConnections = 8;
            //config.EnableMessageType(NetIncomingMessageType.)
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            //config.EnableMessageType(NetIncomingMessageType.)
            //config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);
            server.Start();

            while (true)
            {
                NetIncomingMessage msgIn;
                while ((msgIn = server.ReadMessage()) != null)
                {
                    switch (msgIn.MessageType)
                    {
                        case NetIncomingMessageType.Error:
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            if (msgIn.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                Console.WriteLine("Player Connected!");
                                SendConnection();
                            }
                            if (msgIn.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                            {
                                Console.WriteLine("Player Disconnecting!");
                            }
                            if (msgIn.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                Console.WriteLine("Player Disconnected!");
                            }
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            break;
                        case NetIncomingMessageType.ConnectionApproval:

                            //Someone is trying to connect to server. Check for password
                            msgIn.SenderConnection.Approve();
                            //Console.WriteLine("Player Connected!");

                            //server response to the connecting client (doesn't work, the connection hasn't been established yet)
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
                                //Console.WriteLine("Receiving (" + x + ", " + y + ") from player: " + msgIn.SenderConnection.ToString());
                                UpdateConnectionList(msgIn.SenderConnection);

                                SendPosition(msgIn.SenderConnection, x, y);

                            }
                            if (type == (byte)PacketType.PlayerVel)
                            {
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();

                            }
                            if (type == (byte)PacketType.CreatePlayer)
                            {
                                SendConnection();
                            }
                            if (type == (byte)PacketType.FireballCreate)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                Console.WriteLine("Fireball Created!");
                                SendProjectile(name, new Vector2(posX, posY), new Vector2(velX, velY));
                            }
                            if (type == (byte)PacketType.FireballUpdate)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                SendProjectile(name, new Vector2(posX, posY), Vector2.Zero);
                            }
                            if (type == (byte)PacketType.HomingCreate)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                Console.WriteLine("Homing Created!");
                                SendProjectile(name, new Vector2(posX, posY), new Vector2(velX, velY));
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
