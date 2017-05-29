using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TestServer
{
    public enum PacketType { PlayerPos, EnemyPos, CreatePlayer, PlayerVel, EnemyVel, RemoveProjectile, CreateProjectile, UpdateProjectile, Push, Deflect, ProjectileVel, ColorChange, AssignID, UpdateStats, ShrinkMap, Chain, Invisibility, Clone }

    class Program
    {
        private static NetServer server;
        private static List<NetConnection> connectionList = new List<NetConnection>();
        private static List<string> TestID = new List<string>();
        private static List<string> TestName = new List<string>();
        private static List<string> colors = new List<string>() { "Blue", "Red", "Orange", "Purple", "Brown", "Green", "LightGreen", "Yellow" };
        private static int colorIndex = 0;
        private static int spellId = 0;

        public static void SendConnection(NetConnection sender)
        {
            if (server.Connections.Count > 1)
            {
                connectionList.Clear();
                foreach (NetConnection con in server.Connections)
                {
                    connectionList.Add(con);
                }
                connectionList.Remove(sender);

                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.CreatePlayer);
                msgOut.Write(sender.ToString());
                msgOut.Write(colors[colorIndex]);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);

                for (int i = 0; i < server.Connections.Count - 1; i++)
                {
                    msgOut = server.CreateMessage();
                    msgOut.Write((byte)PacketType.CreatePlayer);
                    msgOut.Write(server.Connections[i].ToString());
                    msgOut.Write(colors[i]);
                    server.SendMessage(msgOut, sender, NetDeliveryMethod.Unreliable, 0);
                }
            }
        }

        public static void SendClone(string id, Vector2 position)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Clone);
            msgOut.Write(id);
            msgOut.Write(position.X);
            msgOut.Write(position.Y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void AssignID(NetConnection con)
        {
            //connectionList.Clear();
            //connectionList.Add(con);

            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.AssignID);
            msgOut.Write(con.ToString());
            msgOut.Write(colors[colorIndex]);
            server.SendMessage(msgOut, con, NetDeliveryMethod.Unreliable, 0);

            colorIndex++;
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
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
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
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
            }
        }

        public static void ShrinkMap()
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.ShrinkMap);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
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
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
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
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
                }
            }
        }

        public static void Push(string id, Vector2 vector)
        {
            connectionList.Clear();
            foreach (NetConnection con in server.Connections)
            {
                if (con.ToString() == id)
                {
                    connectionList.Add(con);
                }
            }

            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Push);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);
            if (connectionList.Count > 0)
            {
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
            }
        }

        public static void SendColor(string id, string name, byte R, byte G, byte B, byte A)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.ColorChange);
            msgOut.Write(id);
            msgOut.Write(name);
            msgOut.Write(R);
            msgOut.Write(G);
            msgOut.Write(B);
            msgOut.Write(A);
            server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.Unreliable, 0);
        }

        public static void UpdateStats(string id, float DamageResistance)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.UpdateStats);
            msgOut.Write(id);
            msgOut.Write(DamageResistance);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void RemoveProjectile(string name, NetConnection sender, string id)
        {
            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                //string name2 = name + "Enemy";
                msgOut.Write((byte)PacketType.RemoveProjectile);
                msgOut.Write(id);
                msgOut.Write(sender.ToString());
                msgOut.Write(name);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
            }
        }

        public static void Deflect(string id, string name, Vector2 position, Vector2 newVel)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            connectionList.Clear();
            //string test = id.Split(' ').Last();
            //test = test.Remove(test.Length - 1);
            foreach (NetConnection con in server.Connections)
            {
                string test2 = con.ToString();
                if (test2 == id)
                {
                    connectionList.Add(con);
                }
            }
            msgOut.Write((byte)PacketType.Deflect);
            msgOut.Write(id);
            msgOut.Write(name);
            msgOut.Write(position.X);
            msgOut.Write(position.Y);
            msgOut.Write(newVel.X);
            msgOut.Write(newVel.Y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);

        }

        public static void Chain(string id, Vector2 vector)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Chain);
            msgOut.Write(id);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);

            //string test = id.Split(' ').Last();
            //test = test.Remove(test.Length - 1);
            connectionList.Clear();
            foreach (NetConnection con in server.Connections)
            {
                string test2 = con.ToString();
                if (test2 == id)
                {
                    connectionList.Add(con);
                }
            }

            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void SendInvisibility(string id, bool isInvis)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Invisibility);
            msgOut.Write(id);
            msgOut.Write(isInvis);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
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
                                colorIndex--;
                            }
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            break;
                        case NetIncomingMessageType.ConnectionApproval:

                            //Someone is trying to connect to server. Check for password
                            bool allow = true;
                            string temp = msgIn.SenderConnection.ToString();
                            temp = temp.Split(' ').Last();
                            temp = temp.Remove(temp.Length - 1);
                            foreach (NetConnection con in server.Connections)
                            {
                                string str = con.RemoteEndPoint.ToString();
                                str = str.Split(' ').Last();
                                str = str.Remove(str.Length - 1);
                                if (str == temp)
                                {
                                    allow = false;
                                    break;
                                }
                                else allow = true;
                            }
                            if (allow)
                            {
                                msgIn.SenderConnection.Approve();
                            }
                            //Console.WriteLine("Player Connected!");

                            //server response to the connecting client (doesn't work, the connection hasn't been established yet)
                            /*
                            SendConnection();
                            msgOut = server.CreateMessage();
                            msgOut.Write("Connection Approved");
                            server.SendMessage(msgOut, msgIn.SenderConnection, NetDeliveryMethod.Unreliable);
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
                                if (!TestName.Exists(x => x == name) && !TestID.Exists(x => x == msgIn.SenderConnection.ToString()))
                                {
                                    SendProjectile(name, new Vector2(posX, posY), new Vector2(velX, velY), msgIn.SenderConnection);
                                }
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
                                if (TestName.Exists(x => x == name) && TestID.Exists(x => x == id))
                                {
                                    TestName.Remove(name);
                                    TestID.Remove(id);
                                }
                                UpdateConnectionList(msgIn.SenderConnection);
                                RemoveProjectile(name, msgIn.SenderConnection, id);

                                if (!TestName.Exists(x => x == name) && !TestID.Exists(x => x == msgIn.SenderConnection.ToString()))
                                {
                                }
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

                                //string test;
                                //test = id;
                                //test = test.Split(' ').Last();
                                //test = test.Remove(test.Length - 1);
                                //TestID.Add(test);
                                //TestName.Add(name);

                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                float velX = msgIn.ReadFloat();
                                float velY = msgIn.ReadFloat();
                                Deflect(id, name, new Vector2(posX, posY), new Vector2(velX, velY));
                            }
                            if (type == (byte)PacketType.ColorChange)
                            {
                                string id = msgIn.ReadString();
                                string name = msgIn.ReadString();
                                byte R = msgIn.ReadByte();
                                byte G = msgIn.ReadByte();
                                byte B = msgIn.ReadByte();
                                byte A = msgIn.ReadByte();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendColor(id, name, R, G, B, A);
                            }
                            if (type == (byte)PacketType.UpdateStats)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                UpdateStats(msgIn.ReadString(), msgIn.ReadFloat());
                            }
                            if (type == (byte)PacketType.ShrinkMap)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                ShrinkMap();
                            }
                            if (type == (byte)PacketType.Chain)
                            {
                                Chain(msgIn.ReadString(), new Vector2(msgIn.ReadFloat(), msgIn.ReadFloat()));
                            }
                            if (type == (byte)PacketType.Invisibility)
                            {
                                string id = msgIn.ReadString();
                                bool isInvis = msgIn.ReadBoolean();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendInvisibility(id, isInvis);
                            }
                            if (type == (byte)PacketType.Clone)
                            {
                                string id = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendClone(id, new Vector2(posX, posY));
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
