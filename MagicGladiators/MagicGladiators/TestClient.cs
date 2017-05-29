﻿using Lidgren.Network;
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
//using TestServer;

namespace MagicGladiators
{
    public enum PacketType { PlayerPos, EnemyPos, CreatePlayer, PlayerVel, EnemyVel, RemoveProjectile, CreateProjectile, UpdateProjectile, Push, Deflect, ProjectileVel, ColorChange, AssignID, UpdateStats, ShrinkMap, Chain, Invisibility, Clone }


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
        public string TestName { get; set; } = "";
        public string TestID { get; set; } = "";
        private string[] directions = new string[4] { "Up", "Down", "Left", "Right" };
        private string hostip;

        //private float TestTimer;

        public TestClient(string ip)
        {
            if (GameWorld.Instance.canClient)
            {
                spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
                font = GameWorld.Instance.Content.Load<SpriteFont>("fontText");
                NetPeerConfiguration config = new NetPeerConfiguration("Server");
                //config.Port = 24049;
                config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                client = new NetClient(config);
                client.Start();
            }
            hostip = ip;
            //client.DiscoverLocalPeers(24049);
        }

        public void ConnectToServer()
        {
            try
            {
                client.Connect(hostip, 51234);
            }
            catch (Exception)
            {
               // client.Shutdown("Meh");
            }
        }

        public void Disconnect()
        {
            client.Disconnect(string.Empty);
            client.Shutdown(string.Empty);
        }

        public void SendClone(string id, Vector2 position)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.Clone);
            msgOut.Write(id);
            msgOut.Write(position.X);
            msgOut.Write(position.Y);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);

        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write(text);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void ShrinkMap()
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.ShrinkMap);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
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
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void SendInvisibility(string id, bool isInvis)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.Invisibility);
            msgOut.Write(id);
            msgOut.Write(isInvis);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
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
                client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
            }
            if (name.Contains("Create"))
            {
                msgOut.Write((byte)PacketType.CreateProjectile);
                msgOut.Write(name);
                msgOut.Write(position.X);
                msgOut.Write(position.Y);
                msgOut.Write(target.X);
                msgOut.Write(target.Y);
                client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
            }
        }

        public void SendRemoval(string name, string id)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.RemoveProjectile);
            msgOut.Write(id);
            msgOut.Write(name);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);

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
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void SendPush(string id, Vector2 vector)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.Push);
            msgOut.Write(id);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void SendColor(string id, string name, byte R, byte G, byte B, byte A)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.ColorChange);
            msgOut.Write(id);
            msgOut.Write(name);
            msgOut.Write(R);
            msgOut.Write(G);
            msgOut.Write(B);
            msgOut.Write(A);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void Deflect(string id, string name, Vector2 position, Vector2 newVel)
        {
            TestName = name;
            TestID = id;
            NetOutgoingMessage msgout;
            msgout = client.CreateMessage();
            msgout.Write((byte)PacketType.Deflect);
            msgout.Write(id);
            msgout.Write(name);
            msgout.Write(position.X);
            msgout.Write(position.Y);
            msgout.Write(newVel.X);
            msgout.Write(newVel.Y);
            client.SendMessage(msgout, NetDeliveryMethod.Unreliable);
        }

        public void UpdateStats(string id, float DamageResistance)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.UpdateStats);
            msgOut.Write(id);
            msgOut.Write(DamageResistance);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
        }

        public void Chain(string id, Vector2 vector)
        {
            NetOutgoingMessage msgOut;
            msgOut = client.CreateMessage();
            msgOut.Write((byte)PacketType.Chain);
            msgOut.Write(id);
            msgOut.Write(vector.X);
            msgOut.Write(vector.Y);
            client.SendMessage(msgOut, NetDeliveryMethod.Unreliable);
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
                                    //player.Id = client.ServerConnection.ToString();
                                    //playerIDTest = client.ServerConnection.ToString();
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
                        break;
                    case NetIncomingMessageType.Data:
                        //text = msgIn.ReadString();
                        byte type = msgIn.ReadByte();
                        if (type == (byte)PacketType.EnemyPos)
                        {
                            string id = msgIn.ReadString();
                            //string corrected = id.Split(' ').Last();
                            //corrected = corrected.Remove(corrected.Length - 1);
                            //EnemyIDTest = id;
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
                            //string corrected = id.Split(' ').Last();
                            //corrected = corrected.Remove(corrected.Length - 1);
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
                            string color = msgIn.ReadString();
                            GameObject go = new GameObject();
                            go.AddComponent(new SpriteRenderer(go, "PlayerSheet", 1));
                            go.AddComponent(new Animator(go));
                            go.AddComponent(new Enemy(go));
                            go.AddComponent(new Collider(go, true, true));
                            go.AddComponent(new Physics(go));
                            go.Tag = "Enemy";
                            go.Id = id;
                            (go.GetComponent("Animator") as Animator).PlayAnimation(color);
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
                            //tring corrected = id.Split(' ').Last();
                            //corrected = corrected.Remove(corrected.Length - 1);
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();
                            if (TestID != id && TestName != name)
                            {
                                foreach (GameObject go in GameWorld.gameObjects)
                                {
                                    if (go.Tag == name && go.Id == id)
                                    {
                                        go.transform.position = new Vector2(posX, posY);

                                        if (name != "Deflect" && name != "Spellshield")
                                        {
                                            (go.GetComponent("Physics") as Physics).Velocity = new Vector2(velX, velY);
                                            (go.GetComponent("Projectile") as Projectile).TestVector = new Vector2(velX, velY);
                                        }

                                    }
                                }
                            }
                        }
                        if (type == (byte)PacketType.CreateProjectile)
                        {
                            string id = msgIn.ReadString();
                            //string corrected = id.Split(' ').Last();
                            //corrected = corrected.Remove(corrected.Length - 1);
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float targetX = msgIn.ReadFloat();
                            float targetY = msgIn.ReadFloat();

                            if (name.Contains("Deflect"))
                            {
                                GameObject effect = new GameObject();
                                effect.AddComponent(new SpriteRenderer(effect, "Deflect", 1));
                                effect.AddComponent(new Collider(effect, true, true));
                                effect.Tag = "Deflect";
                                effect.Id = id;
                                //effect.transform.position = new Vector2(posX, posY);
                                GameWorld.newObjects.Add(effect);
                            }
                            else if (name.Contains("Spellshield"))
                            {
                                GameObject effect = new GameObject();
                                effect.AddComponent(new SpriteRenderer(effect, "Spellshield", 1));
                                effect.AddComponent(new Collider(effect, true, true));
                                effect.Tag = "Spellshield";
                                effect.Id = id;
                                //effect.transform.position = new Vector2(posX, posY);
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
                            //string corrected = sender.Split(' ').Last();
                            //corrected = corrected.Remove(corrected.Length - 1);
                            string name = msgIn.ReadString();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                string test = "";
                                if (id != "")
                                {
                                    //test = id.Split(' ').Last();
                                    //test = test.Remove(test.Length - 1);
                                }
                                //test = id.Split(' ').Last();
                                //test = test.Remove(test.Length - 1);
                                if (go.Tag == "Player" && name == "Chain" && go.Id == id)
                                {
                                    (go.GetComponent("Physics") as Physics).chainDeactivated = true;
                                    (go.GetComponent("Physics") as Physics).chainActivated = false;

                                }
                            }

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
                            string id = msgIn.ReadString();
                            string name = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();
                            float velX = msgIn.ReadFloat();
                            float velY = msgIn.ReadFloat();

                            //TestName = name;
                            //TestID = id;

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

                        if (type == (byte)PacketType.ColorChange)
                        {
                            string id = msgIn.ReadString();
                            string name = msgIn.ReadString();
                            Color color = new Color();
                            byte R = msgIn.ReadByte();
                            byte G = msgIn.ReadByte();
                            byte B = msgIn.ReadByte();
                            byte A = msgIn.ReadByte();
                            color.R = R;
                            color.G = G;
                            color.B = B;
                            color.A = A;
                            GameObject test = GameWorld.Instance.player;

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                string test2 = go.Id;
                                if (test2 != null && go.Tag == name)
                                {
                                    //test2 = go.Id.Split(' ').Last();
                                    //test2 = test2.Remove(test2.Length - 1);
                                }

                                if (go.Id == id)
                                {
                                    (go.GetComponent("SpriteRenderer") as SpriteRenderer).Color = color;
                                }
                            }
                        }
                        if (type == (byte)PacketType.AssignID)
                        {
                            GameWorld.Instance.canClient = false;
                            string id = msgIn.ReadString();
                            string color = msgIn.ReadString();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Player")
                                {
                                    go.Id = id;
                                    (go.GetComponent("Animator") as Animator).PlayAnimation(color);
                                }
                            }
                            //GameWorld.Instance.player.Id = id;
                        }
                        if (type == (byte)PacketType.UpdateStats)
                        {
                            string id = msgIn.ReadString();
                            float damageResist = msgIn.ReadFloat();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                string test2 = go.Id;
                                if (test2 != null && go.Tag == "Enemy")
                                {
                                    //test2 = go.Id.Split(' ').Last();
                                    //test2 = test2.Remove(test2.Length - 1);
                                }

                                if (go.Id == id)
                                {
                                    go.DamageResistance = damageResist;
                                }
                            }
                        }

                        if (type == (byte)PacketType.ShrinkMap)
                        {
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Map")
                                {
                                    (go.GetComponent("SpriteRenderer") as SpriteRenderer).Scale -= 0.1F;
                                    (go.GetComponent("Collider") as Collider).Scale -= 0.1F;
                                    SpriteRenderer sprite = (go.GetComponent("SpriteRenderer") as SpriteRenderer);
                                    go.transform.position = new Vector2(640 - (sprite.Sprite.Width * sprite.Scale) / 2, 360 - (sprite.Sprite.Height * sprite.Scale) / 2);
                                }
                            }
                        }

                        if (type == (byte)PacketType.Chain)
                        {
                            string id = msgIn.ReadString();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                //string test = id.Split(' ').Last();
                                //test = test.Remove(test.Length - 1);
                                if (go.Tag == "Player" && go.Id == id)
                                {
                                    (go.GetComponent("Physics") as Physics).Acceleration += new Vector2(msgIn.ReadFloat(), msgIn.ReadFloat());
                                    (go.GetComponent("Physics") as Physics).chainActivated = true;
                                }
                            }
                        }

                        if (type == (byte)PacketType.Invisibility)
                        {
                            string id = msgIn.ReadString();
                            bool isInvis = msgIn.ReadBoolean();
                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Tag == "Enemy" && go.Id == id)
                                {
                                    go.IsInvisible = isInvis;
                                }
                            }
                        }
                        if (type == (byte)PacketType.Clone)
                        {
                            string id = msgIn.ReadString();
                            float posX = msgIn.ReadFloat();
                            float posY = msgIn.ReadFloat();

                            foreach (GameObject go in GameWorld.gameObjects)
                            {
                                if (go.Id == id)
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        GameObject clone = new GameObject();
                                        clone.AddComponent(new SpriteRenderer(clone, "PlayerSheet", 1));
                                        clone.AddComponent(new Animator(clone));
                                        clone.AddComponent(new Clone(clone));
                                        clone.AddComponent(new Collider(clone, true, true));
                                        clone.AddComponent(new Physics(clone));
                                        clone.Tag = "Clone" + directions[i];
                                        clone.Id = go.Id;
                                        clone.CurrentHealth = go.CurrentHealth;
                                        clone.MaxHealth = go.MaxHealth;
                                        clone.LoadContent(GameWorld.Instance.Content);
                                        (clone.GetComponent("Animator") as Animator).PlayAnimation((go.GetComponent("Animator") as Animator).AnimationName);
                                        clone.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y);
                                        GameWorld.newObjects.Add(clone);
                                    }
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