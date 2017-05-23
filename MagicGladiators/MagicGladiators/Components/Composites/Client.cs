using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MagicGladiators;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using System.Threading;
using System.Diagnostics;

namespace MagicGladiators.Components.Composites
{
    class Client : Component, IUpdateable, IDrawable, ILoadable
    {
        private bool connected;
        private string ip;
        private Texture2D rect;

        private static readonly Object thislock = new Object();
        private static Dictionary<string, GameObject> _players;
        private static Dictionary<string, GameObject> players
        {
            get { lock (thislock) { return _players; } }
            set { lock (thislock) { _players = value; } }
        }
        private string thisconninfo;
        private Player playerPos;

        private Keys[] lastPressedKeys;

        Connection tCPConn;
        //Es is client!
        public Client(GameObject gameObject) : base(gameObject)
        {
            connected = false;
            updatingHostInfo = false;
            tCPConn = null;
            ip = string.Empty;
            lastPressedKeys = Keyboard.GetState().GetPressedKeys();
            players = new Dictionary<string, GameObject>();
            idle = true;
        }

        SpriteFont spriteFont;
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!connected)
            {
                spriteBatch.Draw(rect, new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, ip, new Vector2(20, 20), Color.Black);
            }
        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("fontText");
            rect = new Texture2D(GameWorld.Instance.GraphicsDevice, 15 * 21, 14);
            Color[] data = new Color[15 * 21 * 14];
            for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            rect.SetData(data);
        }

        public void Update()
        {
            if (!connected)
            {
                if (threadUpdate != null)
                {
                    threadUpdate.Abort();
                }
                KeyboardState kbState = Keyboard.GetState();
                Keys[] pressedKeys = kbState.GetPressedKeys();

                //check if any of the previous update's keys are no longer pressed
                //foreach (Keys key in lastPressedKeys)
                //{
                //    if (!pressedKeys.Contains(key))
                //        OnKeyUp(key);
                //}

                //check if the currently pressed keys were already pressed
                foreach (Keys key in pressedKeys)
                {
                    if (!lastPressedKeys.Contains(key))
                    {
                        if (key.ToString().Contains("D") && key.ToString().Length > 1)
                        {
                            ip += key.ToString()[1];
                        }
                        else if (key == Keys.OemPeriod)
                        {
                            if (pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift))
                            {
                                ip += ":";
                            }
                            else
                            {
                                ip += ".";
                            }
                        }
                        else if (key == Keys.Back)
                        {
                            ip = ip.Truncate(ip.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            string serverIP = ip.Split(':').First();
                            int serverPort = int.Parse(ip.Split(':').Last());
                            ConnectionInfo connInfo = new ConnectionInfo(serverIP, serverPort);
                            try
                            {
                                tCPConn = TCPConnection.GetConnection(connInfo);
                                tCPConn.AppendIncomingPacketHandler<bool>("JoinedServerRespond", JoinedServerRespond, NetworkComms.DefaultSendReceiveOptions);
                                tCPConn.AppendIncomingPacketHandler<UpdatePackage>("HostPos", HostPos, NetworkComms.DefaultSendReceiveOptions);
                                tCPConn.AppendIncomingPacketHandler<ServerPackage>("ServerPackage", IncommingServerPackage, NetworkComms.DefaultSendReceiveOptions);
                                tCPConn.AppendIncomingPacketHandler<TryConnectPackage>("TryConnect", TryConnect, NetworkComms.DefaultSendReceiveOptions);
                                tCPConn.SendObject<string>("JoinServer", "Player2");
                            }
                            catch (Exception e)
                            {
                                Debug.Write(e);
                            }
                        }
                        //else
                        //{
                        //    ip += key;
                        //}
                    }
                }
                //save the currently pressed keys so we can compare on the next update
                lastPressedKeys = pressedKeys;
            }
            else
            {
                if (threadUpdate == null)
                {
                    threadUpdate = new Thread(ThreadUpdate);
                    threadUpdate.Start();
                }
                else if (!threadUpdate.IsAlive)
                {
                    threadUpdate.Start();
                }
            }
        }

        private void TryConnect(PacketHeader packetHeader, Connection connection, TryConnectPackage incomingObject)
        {
            idle = false;
            if (incomingObject.connected && !players.ContainsKey(incomingObject.connectionInfo))
            {
                Director dir = new Director(new PlayerBuilder());
                GameObject go = dir.Construct(new Vector2(20), 2);
                playerPos = go.GetComponent("Player") as Player;
                go.LoadContent(GameWorld.Instance.Content);
                GameWorld.newObjects.Add(go);
                thisconninfo = incomingObject.connectionInfo;
                players.Add(incomingObject.connectionInfo, go);
                this.connected = incomingObject.connected;
            }
            idle = true;
        }

        private bool idle;
        private void IncommingServerPackage(PacketHeader packetHeader, Connection connection, ServerPackage incomingObject)
        {
            if (players.Count > 0)
            {
                foreach (string key in incomingObject.players.Keys)
                {
                    if (key != thisconninfo)
                    {
                        if (idle)
                        {
                            idle = false;
                            if (players.Keys.Contains(key))
                            {
                                (players[key].GetComponent("Enemy") as Enemy).UpdateEnemyInfo(incomingObject.players[key][0], incomingObject.players[key][1]);
                            } else
                            {
                                try
                                {
                                    GameObject go = new GameObject(0);
                                    go.Tag = connection.ToString();
                                    go.AddComponent(new Enemy(go));
                                    go.AddComponent(new SpriteRenderer(go, "Player", 1));
                                    go.LoadContent(GameWorld.Instance.Content);
                                    players.Add(key, go);
                                    GameWorld.newObjects.Add(go);
                                } catch (Exception e)
                                {
                                    //HAHAAHAHAHAHAHA nope
                                }
                            }
                            idle = true;
                        }
                    }
                }
            }
        }

        private Thread threadUpdate;
        public void ThreadUpdate()
        {
            while (true)
            {
                try
                {
                    tCPConn.SendObject<UpdatePackage>("UpdatePosition", playerPos.updatePackage);
                }
                catch (ConnectionSendTimeoutException cste)
                {
                    connected = false;
                    GameWorld.objectsToRemove.Add(GameWorld.gameObjects.Find(x => x.GetComponent("Enemy") is Enemy));
                    Debug.Write(cste.Message);
                }
            }
        }

        private bool updatingHostInfo;
        private void HostPos(PacketHeader packetHeader, Connection connection, UpdatePackage incomingObject)
        {
            //if (connected)
            //{
            //    if (!updatingHostInfo)
            //    {
            //        updatingHostInfo = true;
            //        hostPos.UpdateEnemyInfo(incomingObject);
            //        updatingHostInfo = false;
            //    }
            //}
        }

        private void JoinedServerRespond(PacketHeader packetHeader, Connection connection, bool incomingObject)
        {
            //if (incomingObject)
            //{
            //    connected = true;
            //    GameObject host = new GameObject(0);
            //    host.AddComponent(new Enemy(host));
            //    host.AddComponent(new SpriteRenderer(host, "Player", 1));
            //    host.AddComponent(new Collider(host, false));
            //    host.LoadContent(GameWorld.Instance.Content);
            //    GameWorld.newObjects.Add(host);
            //    hostPos = host.GetComponent("Enemy") as Enemy;
            //}
        }
    }
}
