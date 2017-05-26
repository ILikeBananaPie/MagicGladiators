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

        public static void SendConnection(NetConnection sender)
        {
            if (server.Connections.Count > 1)
            {
                for (int i = 0; i < server.Connections.Count; i++)
                {
                    NetOutgoingMessage msgOut;
                    msgOut = server.CreateMessage();
                    msgOut.Write((byte)PacketType.CreatePlayer);
                    msgOut.Write(server.Connections[i].ToString());
                    connectionList.Clear();
                    foreach (NetConnection con in server.Connections)
                    {
                        connectionList.Add(con);
                    }
                    connectionList.Remove(server.Connections[i]);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
                //msgOut.Write(sender.ToString());
                //server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

        }

        public static void AssignID(NetConnection con)
        {

            for (int i = 0; i < server.Connections.Count; i++)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.AssignID);
                msgOut.Write(con.RemoteEndPoint.ToString());
                connectionList.Clear();
                connectionList.Add(con);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
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

        public static void SendPosition(int x, int y, NetConnection sender)
        {

            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.EnemyPos);
                msgOut.Write(sender.ToString());
                msgOut.Write(x);
                msgOut.Write(y);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public static void SendProjectileVel(string name, Vector2 velocity, NetConnection sender)
        {
            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();

                name = name.Split(',').First();
                msgOut.Write((byte)PacketType.ProjectileVel);
                msgOut.Write(sender.ToString());
                msgOut.Write(name);
                msgOut.Write(velocity.X);
                msgOut.Write(velocity.Y);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public static void SendProjectile(string name, Vector2 position, Vector2 target, NetConnection sender)
        {
            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                if (name.Contains("Update"))
                {
                    name = name.Split(',').First();
                    msgOut.Write((byte)PacketType.UpdateProjectile);
                    msgOut.Write(sender.ToString());
                    msgOut.Write(name);
                    msgOut.Write(position.X);
                    msgOut.Write(position.Y);
                    msgOut.Write(target.X);
                    msgOut.Write(target.Y);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
                if (name.Contains("Create"))
                {
                    string name2 = name.Split(',').First();
                    msgOut.Write((byte)PacketType.CreateProjectile);
                    msgOut.Write(sender.ToString());
                    msgOut.Write(name2);
                    msgOut.Write(position.X);
                    msgOut.Write(position.Y);
                    msgOut.Write(target.X);
                    msgOut.Write(target.Y);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }
        }

        public static void Push(string id, Vector2 vector)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            connectionList.Clear();
            string test = id.Split(' ').Last();
            test = test.Remove(test.Length - 1);
            foreach (NetConnection con in server.Connections)
            {
                string test2 = con.RemoteEndPoint.ToString();
                if (test2 == test)
                {
                    connectionList.Add(con);
                }
            }
            msgOut.Write((byte)PacketType.Push);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void SendColor(string id, byte R, byte G, byte B, byte A)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.ColorChange);
            msgOut.Write(id);
            msgOut.Write(R);
            msgOut.Write(G);
            msgOut.Write(B);
            msgOut.Write(A);
            server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void RemoveProjectile(string name, NetConnection sender, string id)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            //string name2 = name + "Enemy";
            msgOut.Write((byte)PacketType.RemoveProjectile);
            msgOut.Write(id);
            msgOut.Write(sender.ToString());
            msgOut.Write(name);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void Deflect(string id, string name, Vector2 position, Vector2 newVel)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            connectionList.Clear();
            string test = id.Split(' ').Last();
            test = test.Remove(test.Length - 1);
            foreach (NetConnection con in server.Connections)
            {
                string test2 = con.RemoteEndPoint.ToString();
                if (test2 == test)
                {
                    connectionList.Add(con);
                }
            }
            msgOut.Write((byte)PacketType.Deflect);
            msgOut.Write(name);
            msgOut.Write(position.X);
            msgOut.Write(position.Y);
            msgOut.Write(newVel.X);
            msgOut.Write(newVel.Y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);

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
                                SendConnection(msgIn.SenderConnection);
                                AssignID(msgIn.SenderConnection);
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

                                SendPosition(x, y, msgIn.SenderConnection);

                            }
                            if (type == (byte)PacketType.PlayerVel)
                            {
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();

                            }
                            if (type == (byte)PacketType.CreatePlayer)
                            {
                                SendConnection(msgIn.SenderConnection);
                            }
                            if (type == (byte)PacketType.UpdateProjectile)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                SendProjectile(name, new Vector2(posX, posY), new Vector2(velX, velY), msgIn.SenderConnection);
                            }
                            if (type == (byte)PacketType.CreateProjectile)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                string writeline = name.Split(',').First();
                                Console.WriteLine(writeline + " Created!");
                                SendProjectile(name, new Vector2(posX, posY), new Vector2(velX, velY), msgIn.SenderConnection);
                            }
                            if (type == (byte)PacketType.RemoveProjectile)
                            {
                                string id = msgIn.ReadString();
                                string name = msgIn.ReadString();
                                //string name2 = name.Split(',').First();
                                Console.WriteLine("Removing " + name);
                                UpdateConnectionList(msgIn.SenderConnection);
                                RemoveProjectile(name, msgIn.SenderConnection, id);
                            }
                            if (type == (byte)PacketType.Push)
                            {
                                string id = msgIn.ReadString();
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();
                                Vector2 vector = new Vector2(x, y);
                                Push(id, vector);
                            }
                            if (type == (byte)PacketType.Deflect)
                            {
                                string id = msgIn.ReadString();
                                string name = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                Deflect(id, name, new Vector2(posX, posY), new Vector2(velX, velY));
                            }
                            if (type == (byte)PacketType.ColorChange)
                            {
                                string id = msgIn.ReadString();
                                byte R = msgIn.ReadByte();
                                byte G = msgIn.ReadByte();
                                byte B = msgIn.ReadByte();
                                byte A = msgIn.ReadByte();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendColor(id, R, G, B, A);
                            }
                            if (type == (byte)PacketType.UpdateStats)
                            {

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
