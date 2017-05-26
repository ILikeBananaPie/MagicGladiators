using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MagicGladiators;
using System.Net;

namespace MagicGladiators
{
    public enum PacketType { Position, Velocity, PlayerPos, EnemyPos, CreatePlayer, PlayerVel, EnemyVel, RemoveProjectile, CreateProjectile, UpdateProjectile, Push, Deflect, ProjectileVel }


    public class TestClient
    {
        private NetClient client;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        public static string text = "";
        private static readonly object locker = new object();
        private int test = 0;
        private string playerIDTest;
        private string EnemyIDTest;

        public TestClient()
        {
            string hostip = "25.28.211.248";
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
            font = GameWorld.Instance.Content.Load<SpriteFont>("fontText");
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            //config.Port = 24049;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(config);
            client.Start();
            client.Connect(hostip, 51234);
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

            int x = (int)vector.X;
            int y = (int)vector.Y;
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.PlayerPos);
            msgOut.Write(x);
            msgOut.Write(y);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendProjectile(string name, Vector2 position, Vector2 target)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            if (name.Contains("Update"))
            {
                //name = name.Split(',').First();
                float posX = position.X;
                float posY = position.Y;
                msgOut.Write((byte)PacketType.UpdateProjectile);
                msgOut.Write(name);
                msgOut.Write(posX);
                msgOut.Write(posY);
                msgOut.Write(target.X);
                msgOut.Write(target.Y);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            }
            if (name.Contains("Create"))
            {
                msgOut.Write((byte)PacketType.CreateProjectile);
                msgOut.Write(name);
                msgOut.Write(position.X);
                msgOut.Write(position.Y);
                msgOut.Write(target.X);
                msgOut.Write(target.Y);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void SendRemoval(string name, string id)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.RemoveProjectile);
            msgOut.Write(id);
            msgOut.Write(name);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);

        }

        public void SendVelocity(Vector2 vector)
        {
            float x = vector.X;
            float y = vector.Y;
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.PlayerVel);
            msgOut.Write(x);
            msgOut.Write(y);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPush(string id, Vector2 vector)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.Push);
            msgOut.Write(id);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
        }

        public void Deflect(string id, string name, Vector2 position, Vector2 newVel)
        {
            NetOutgoingMessage msgout;
            msgout = client.CreateMessage();
            msgout.Write((byte)PacketType.Deflect);
            msgout.Write(id);
            msgout.Write(name);
            msgout.Write(position.X);
            msgout.Write(position.Y);
            msgout.Write(newVel.X);
            msgout.Write(newVel.Y);
            client.SendMessage(msgout, NetDeliveryMethod.ReliableOrdered);
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
                        if (msgIn.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            foreach (GameObject player in GameWorld.gameObjects)
                            {
                                if (player.Tag == "Player")
                                {
                                    player.Id = client.ServerConnection.ToString();
                                    playerIDTest = client.ServerConnection.ToString();
                                    //player.Id = 
                                }
                            }
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
                        /*
                        text = msgIn.ReadByte().ToString();
                        GameObject go2 = new GameObject();
                        go2.AddComponent(new Enemy(go2));
                        go2.AddComponent(new SpriteRenderer(go2, "Player", 1));
                        //go2.AddComponent(new Physics(gameob)
                        go2.Tag = "Enemy";
                        GameWorld.newObjects.Add(go2);
                        foreach (GameObject dummy in GameWorld.gameObjects)
                        {
                            if (dummy.Tag == "Dummy")
                            {
                                go2.transform.position = dummy.transform.position;
                                GameWorld.objectsToRemove.Add(dummy);
                                break;
                            }
                        }
                        */
                        break;
                    case NetIncomingMessageType.Data:
                        //text = msgIn.ReadString();
                        byte type = msgIn.ReadByte();
                        if (type == (byte)PacketType.EnemyPos)
                        {
                            //string test = msgIn.ReadFloat().ToString();
                            //string[] arr = test.Split(',');
                            string id = msgIn.ReadString();
                            EnemyIDTest = id;
                            int x = msgIn.ReadInt32();
                            int y = msgIn.ReadInt32();

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy" && go.Id == id)
                                {
                                    go.transform.position = new Vector2(x, y);
                                }
                            }
                        }
                        if (type == (byte)PacketType.EnemyVel)
                        {
                            string id = msgIn.ReadString();
                            float x = msgIn.ReadFloat();
                            float y = msgIn.ReadFloat();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy" && go.Id == id)
                                {
                                    //(go.GetComponent("Physics") as Physics).Velocity = new Vector2(x, y);
                                    (go.GetComponent("Enemy") as Enemy).velocity = new Vector2(x, y);
                                }
                            }
                        }
                        if (type == (byte)PacketType.CreatePlayer)
                        {
                            string id = msgIn.ReadString();
                            GameObject go = new GameObject();
                            go.AddComponent(new Enemy(go));
                            go.AddComponent(new SpriteRenderer(go, "Player", 1));
                            go.AddComponent(new Collider(go, true));
                            go.AddComponent(new Physics(go));
                            go.Tag = "Enemy";
                            go.Id = id;
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
                        if (type == (byte)PacketType.UpdateProjectile)
                        {
                            string id = msgIn.ReadString();
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == name && go.Id == id)
                                {
                                    go.transform.position = new Vector2(posX, posY);
                                    /*
                                    if (name != "Deflect")
                                    {
                                        (go.GetComponent("Physics") as Physics).Velocity = new Vector2(velX, velY);
                                        (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2(velX, velY);
                                    }
                                    */
                                }
                            }
                        }
                        if (type == (byte)PacketType.CreateProjectile)
                        {
                            string id = msgIn.ReadString();
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float targetX = msgIn.ReadFloat();
                            float targetY = msgIn.ReadFloat();

                            if (name.Contains("Drain"))
                            {

                            }
                            else if (name.Contains("Deflect"))
                            {
                                GameObject effect = new GameObject();
                                effect.AddComponent(new SpriteRenderer(effect, "Deflect", 1));
                                //effect.AddComponent(new Deflect(effect));
                                //effect.AddComponent(new Collider(effect, true));
                                effect.Tag = "Deflect";
                                effect.Id = id;
                                //(effect.GetComponent("Deflect") as Deflect).activated = true;
                                GameWorld.newObjects.Add(effect);
                            }
                            else
                            {
                                Director director = new Director(new ProjectileBuilder());
                                director.ConstructProjectile(new Vector2(posX, posY), new Vector2(targetX, targetY), name, new GameObject(), id);
                            }

                        }
                        if (type == (byte)PacketType.RemoveProjectile)
                        {
                            string id = msgIn.ReadString();
                            string sender = msgIn.ReadString();
                            string name = msgIn.ReadString();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == name && go.Id == sender)
                                {
                                    GameWorld.objectsToRemove.Add(go);
                                }
                            }
                        }

                        if (type == (byte)PacketType.Push)
                        {
                            float x = msgIn.ReadFloat();
                            float y = msgIn.ReadFloat();
                            (GameWorld.Instance.player.GetComponent("Player") as Player).isPushed(new Vector2(x, y));
                        }
                        if (type == (byte)PacketType.Deflect)
                        {
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (name == go.Tag && go.Id == GameWorld.Instance.player.Id)
                                {
                                    go.transform.position = new Vector2(posX, posY);
                                    (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2(velX, velY);
                                    (go.GetComponent("Physics") as Physics).Velocity = new Vector2(velX, velY);
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
