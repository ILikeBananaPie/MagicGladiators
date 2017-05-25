﻿using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public enum PacketType { Position, Velocity, PlayerPos, EnemyPos, CreatePlayer, PlayerVel, EnemyVel, FireballCreate, FireballUpdate, HomingCreate, RemoveProjectile }


    public class TestClient
    {
        private NetClient client;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        public static string text = "";
        private static readonly object locker = new object();
        private int test = 0;

        public TestClient()
        {
            string hostip = "213.32.242.96";
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
            font = GameWorld.Instance.Content.Load<SpriteFont>("fontText");
            NetPeerConfiguration config = new NetPeerConfiguration("Server");
            config.Port = 24049;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(config);
            client.Start();
            client.Connect(hostip, 24049);
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

        public void SendProjectile(string name, Vector2 position, Vector2 velVector)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
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
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            }
            if (name.Contains("Update"))
            {
                //name = name.Split(',').First();
                float posX = position.X;
                float posY = position.Y;
                msgOut.Write((byte)PacketType.FireballUpdate);
                msgOut.Write(name);
                msgOut.Write(posX);
                msgOut.Write(posY);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
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
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            }

        }

        public void SendRemoval()
        {
            //send removing message
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
                        //text = msgIn.ReadString();
                        /*
                        if (msgIn.ToString() == "5")
                        {
                            text = "Connected!";
                            NetOutgoingMessage msgOut;
                            msgOut = client.CreateMessage();
                            msgOut.Write((byte)PacketType.CreatePlayer);
                            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
                        }
                        */
                        //text = msgIn.ReadByte().ToString();


                        break;
                    case NetIncomingMessageType.UnconnectedData:

                        break;
                    case NetIncomingMessageType.ConnectionApproval:
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
                        break;
                    case NetIncomingMessageType.Data:
                        //text = msgIn.ReadString();
                        byte type = msgIn.ReadByte();
                        if (type == (byte)PacketType.EnemyPos)
                        {
                            //string test = msgIn.ReadFloat().ToString();
                            //string[] arr = test.Split(',');
                            int x = msgIn.ReadInt32();
                            int y = msgIn.ReadInt32();

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy")
                                {
                                    go.transform.position = new Vector2(x, y);
                                }
                            }
                        }
                        if (type == (byte)PacketType.EnemyVel)
                        {
                            float x = msgIn.ReadFloat();
                            float y = msgIn.ReadFloat();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy")
                                {
                                    //(go.GetComponent("Physics") as Physics).Velocity = new Vector2(x, y);
                                    (go.GetComponent("Enemy") as Enemy).velocity = new Vector2(x, y);
                                }
                            }
                        }
                        if (type == (byte)PacketType.CreatePlayer)
                        {
                            GameObject go = new GameObject();
                            go.AddComponent(new Enemy(go));
                            go.AddComponent(new SpriteRenderer(go, "Player", 1));
                            go.AddComponent(new Collider(go, true));
                            go.Tag = "Enemy";
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
                        if (type == (byte)PacketType.FireballCreate)
                        {
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();
                            Vector2 target = new Vector2(velX, velY);
                            //target.Normalize();
                            test++;
                            Director director = new Director(new ProjectileBuilder());
                            director.ConstructProjectile(new Vector2(posX, posY), target, "FireballEnemy", new GameObject());
                        }
                        if (type == (byte)PacketType.FireballUpdate)
                        {
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == name + "Enemy")
                                {
                                    go.transform.position = new Vector2(posX, posY);
                                }
                            }
                        }
                        if (type == (byte)PacketType.HomingCreate)
                        {
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();
                            Vector2 target = new Vector2(velX, velY);
                            //target.Normalize();
                            Director director = new Director(new ProjectileBuilder());
                            director.ConstructProjectile(new Vector2(posX, posY), new Vector2(velX, velY), "HomingMissileEnemy", new GameObject());
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
