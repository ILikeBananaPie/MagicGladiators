using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{
    public enum PacketType { PlayerPos, EnemyPos, CreatePlayer, PlayerVel, EnemyVel, RemoveProjectile, CreateProjectile, UpdateProjectile, Push, Deflect, ProjectileVel, ColorChange, AssignID, UpdateStats, ShrinkMap, Chain, Invisibility, Clone, RemovePlayer, UpdatePlayerIndex, Critter, EnemyAcceleration, MapSettings, StartGame, Ready, SwitchPhase, SpeedUp, SpeedDown, ChainRemove }

    public class Program
    {
        private static NetServer server;
        private static List<NetConnection> connectionList = new List<NetConnection>();


        private static List<Player> players = new List<Player>();
        private static int playerIndex = 0;

        private static List<string> TestID = new List<string>();
        private static List<string> TestName = new List<string>();
        private static List<string> colors = new List<string>() { "Blue", "Red", "Orange", "Purple", "Brown", "Green", "LightGreen", "Yellow" };
        private static int colorIndex = 0;
        private static int test = 0;
        //private static int spellId = 0;

        private static void CorrectPlayerIndex(NetConnection con, string command, int index)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            if (command == "Remove")
            {
                foreach (Player player in players)
                {
                    if (player.connectionID == con)
                    {
                        players.Remove(player);
                        break;
                    }
                }
                while (server.Connections.Count != players.Count)
                {
                    Thread.Sleep(50);
                }

                for (int i = 0; i < server.Connections.Count; i++)
                {
                    //players[i].playerIndex = i;
                    msgOut = server.CreateMessage();
                    msgOut.Write((byte)PacketType.RemovePlayer);
                    msgOut.Write(con.ToString());
                    msgOut.Write(i);
                    if (server.Connections[i] != null)
                    {
                    }
                    server.SendMessage(msgOut, server.Connections[i], NetDeliveryMethod.ReliableOrdered, 0);

                    test++;
                }
                playerIndex--;
                colorIndex--;
                test = 0;
            }

            else
            {
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.UpdatePlayerIndex);
                msgOut.Write(con.ToString());
                msgOut.Write(index);
                if (connectionList.Count > 0)
                {
                }
                server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.Unreliable, 0);
            }
        }

        private static void Critter(string id, string tag, float posX, float posY, string command)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Critter);
            msgOut.Write(id);
            msgOut.Write(tag);
            msgOut.Write(posX);
            msgOut.Write(posY);
            msgOut.Write(command);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        private static void SendEnemyAcceleration(string id, float x, float y)
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
            msgOut.Write((byte)PacketType.EnemyAcceleration);
            msgOut.Write(x);
            msgOut.Write(y);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        private static void SendMapSettings(string map, int rounds)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.MapSettings);
            msgOut.Write(map);
            msgOut.Write(rounds);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void SendSwitchPhase()
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.SwitchPhase);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void SendStartgame()
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.StartGame);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void SendReady(string id, bool isReady)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Ready);
            msgOut.Write(id);
            msgOut.Write(isReady);
            server.SendMessage(msgOut, server.Connections, NetDeliveryMethod.Unreliable, 0);
        }

        public static void ChainRemove(string id)
        {
            connectionList.Clear();
            //string text = id;
            //text = text.Split(' ').Last();
            //text = text.Remove(text.Length - 1);
            foreach (NetConnection con in server.Connections)
            {
                if (con.ToString() == id)
                {
                    connectionList.Add(con);
                }
            }
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.ChainRemove);
            //msgOut.Write(id);
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
                                UpdateConnectionList(msgIn.SenderConnection);
                                CorrectPlayerIndex(msgIn.SenderConnection, "Remove", 0);
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
                            #region ChainRemove
                            if (type == (byte)PacketType.ChainRemove)
                            {
                                ChainRemove(msgIn.ReadString());
                            }
                            #endregion
                            #region SpeedUp
                            if (type == (byte)PacketType.SpeedUp)
                            {
                                SendSpeedUp(msgIn.ReadString(), msgIn.ReadFloat());
                            }
                            #endregion
                            #region SpeedDown
                            if (type == (byte)PacketType.SpeedDown)
                            {
                                SendSpeedDown(msgIn.ReadString(), msgIn.ReadFloat());
                            }
                            #endregion
                            #region SwitchPhase
                            if (type == (byte)PacketType.SwitchPhase)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendSwitchPhase();
                            }
                            #endregion
                            #region StartGame
                            if (type == (byte)PacketType.StartGame)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendStartgame();
                            }
                            #endregion
                            #region Ready
                            if (type == (byte)PacketType.Ready)
                            {
                                SendReady(msgIn.ReadString(), msgIn.ReadBoolean());
                            }
                            #endregion
                            #region MapSettings
                            if (type == (byte)PacketType.MapSettings)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendMapSettings(msgIn.ReadString(), msgIn.ReadInt32());
                            }
                            #endregion
                            #region EnemyAcceleration
                            if (type == (byte)PacketType.EnemyAcceleration)
                            {
                                string id = msgIn.ReadString();
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();
                                SendEnemyAcceleration(id, x, y);
                            }
                            #endregion
                            #region PlayerPos
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
                            #endregion
                            #region PlayerVel
                            if (type == (byte)PacketType.PlayerVel)
                            {
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();

                            }
                            #endregion
                            #region Critter
                            if (type == (byte)PacketType.Critter)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                Critter(msgIn.ReadString(), msgIn.ReadString(), msgIn.ReadFloat(), msgIn.ReadFloat(), msgIn.ReadString());
                            }
                            #endregion
                            #region UpdatePlayerIndex
                            if (type == (byte)PacketType.UpdatePlayerIndex)
                            {
                                int index = msgIn.ReadInt32();
                                UpdateConnectionList(msgIn.SenderConnection);
                                CorrectPlayerIndex(msgIn.SenderConnection, "Update", index);
                            }
                            #endregion
                            #region CreatePlayer
                            if (type == (byte)PacketType.CreatePlayer)
                            {
                                SendConnection(msgIn.SenderConnection);
                            }
                            #endregion
                            #region UpdateProjectile
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
                                    SendProjectile(name, posX, posY, velX, velY, msgIn.SenderConnection);
                                }
                            }
                            #endregion
                            #region CreateProjectile
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
                                SendProjectile(name, posX, posY, velX, velY, msgIn.SenderConnection);
                            }
                            #endregion
                            #region RemoveProjectile
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
                            #endregion
                            #region Push
                            if (type == (byte)PacketType.Push)
                            {
                                string id = msgIn.ReadString();
                                float damage = msgIn.ReadFloat();
                                float x = msgIn.ReadFloat();
                                float y = msgIn.ReadFloat();
                                Push(id, x, y, damage);
                            }
                            #endregion
                            #region Deflect
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
                                Deflect(id, name, posX, posY, velX, velY);
                            }
                            #endregion
                            #region ColorChange
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
                            #endregion
                            #region UpdateStats
                            if (type == (byte)PacketType.UpdateStats)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                UpdateStats(msgIn.ReadString(), msgIn.ReadFloat());
                            }
                            #endregion
                            #region ShrinkMap
                            if (type == (byte)PacketType.ShrinkMap)
                            {
                                UpdateConnectionList(msgIn.SenderConnection);
                                ShrinkMap();
                            }
                            #endregion
                            #region Chain
                            if (type == (byte)PacketType.Chain)
                            {
                                Chain(msgIn.ReadString(), msgIn.ReadFloat(), msgIn.ReadFloat());
                            }
                            #endregion
                            #region Invisibility
                            if (type == (byte)PacketType.Invisibility)
                            {
                                string id = msgIn.ReadString();
                                bool isInvis = msgIn.ReadBoolean();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendInvisibility(id, isInvis);
                            }
                            #endregion
                            #region Clone
                            if (type == (byte)PacketType.Clone)
                            {
                                string id = msgIn.ReadString();
                                float posX = msgIn.ReadFloat();
                                float posY = msgIn.ReadFloat();
                                UpdateConnectionList(msgIn.SenderConnection);
                                SendClone(id, posX, posY);
                            }
                            #endregion
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

                //to all but the sender(connector). Create an enemy
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketType.CreatePlayer);
                msgOut.Write(sender.ToString());
                msgOut.Write(colors[colorIndex]);
                msgOut.Write(playerIndex);
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);

                //to the sender(connector). Create enemies
                for (int i = 0; i < server.Connections.Count - 1; i++)
                {
                    msgOut = server.CreateMessage();
                    msgOut.Write((byte)PacketType.CreatePlayer);
                    msgOut.Write(server.Connections[i].ToString());
                    msgOut.Write(colors[i]);
                    msgOut.Write(i);
                    server.SendMessage(msgOut, sender, NetDeliveryMethod.Unreliable, 0);
                }
            }
        }

        public static void SendClone(string id, float posX, float posY)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Clone);
            msgOut.Write(id);
            msgOut.Write(posX);
            msgOut.Write(posY);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
        }

        public static void AssignID(NetConnection con)
        {
            //connectionList.Clear();
            //connectionList.Add(con);
            players.Add(new Player(playerIndex, con));

            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.AssignID);
            msgOut.Write(con.ToString());
            msgOut.Write(colors[colorIndex]);
            msgOut.Write(playerIndex);
            server.SendMessage(msgOut, con, NetDeliveryMethod.Unreliable, 0);
            HostInfo();
            colorIndex++;
            playerIndex++;
        }

        public static void HostInfo()
        {

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

        public static void SendProjectileVel(string name, float velX, float velY, NetConnection sender)
        {
            if (connectionList.Count > 0)
            {
                NetOutgoingMessage msgOut;
                msgOut = server.CreateMessage();

                name = name.Split(',').First();
                msgOut.Write((byte)PacketType.ProjectileVel);
                msgOut.Write(sender.ToString());
                msgOut.Write(name);
                msgOut.Write(velX);
                msgOut.Write(velY);
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

        public static void SendProjectile(string name, float posX, float posY, float targetX, float targetY, NetConnection sender)
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
                    msgOut.Write(posX);
                    msgOut.Write(posY);
                    msgOut.Write(targetX);
                    msgOut.Write(targetY);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
                }
                if (name.Contains("Create"))
                {
                    string name2 = name.Split(',').First();
                    msgOut.Write((byte)PacketType.CreateProjectile);
                    msgOut.Write(sender.ToString());
                    msgOut.Write(name2);
                    msgOut.Write(posX);
                    msgOut.Write(posY);
                    msgOut.Write(targetX);
                    msgOut.Write(targetY);
                    server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);
                }
            }
        }

        public static void Push(string id, float vectorX, float vectorY, float damage)
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
            msgOut.Write(damage);
            msgOut.Write(vectorX);
            msgOut.Write(vectorY);
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
                server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public static void Deflect(string id, string name, float posX, float posY, float newVelX, float newVelY)
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
            msgOut.Write(posX);
            msgOut.Write(posY);
            msgOut.Write(newVelX);
            msgOut.Write(newVelY);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.Unreliable, 0);

        }

        public static void Chain(string id, float vectorX, float vectorY)
        {
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.Chain);
            msgOut.Write(id);
            msgOut.Write(vectorX);
            msgOut.Write(vectorY);

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

        public static void SendSpeedUp(string id, float factor)
        {
            connectionList.Clear();
            foreach (NetConnection con in server.Connections)
            {
                if (con.ToString() == id)
                {
                    connectionList.Add(con);
                    break;
                }
            }
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.SpeedUp);
            msgOut.Write(id);
            msgOut.Write(factor);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public static void SendSpeedDown(string id, float factor)
        {
            connectionList.Clear();
            foreach (NetConnection con in server.Connections)
            {
                if (con.ToString() == id)
                {
                    connectionList.Add(con);
                    break;
                }
            }
            NetOutgoingMessage msgOut;
            msgOut = server.CreateMessage();
            msgOut.Write((byte)PacketType.SpeedDown);
            msgOut.Write(id);
            msgOut.Write(factor);
            server.SendMessage(msgOut, connectionList, NetDeliveryMethod.ReliableOrdered, 0);
        }

    }
}
