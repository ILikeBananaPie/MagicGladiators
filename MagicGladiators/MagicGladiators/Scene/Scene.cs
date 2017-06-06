using MagicGladiators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MagicGladiators
{
    public class Scene : ILoadable, IUpdateable, IDrawable
    {
        List<GameObject> gameObjects;
        public string scenetype;
        public static List<GameObject> tempList = new List<GameObject>();


        #region Constructors
        public Scene(GameObject[] go)
        {

            gameObjects = new List<GameObject>();
            foreach (GameObject g in go)
            {
                gameObjects.Add(g);
            }
            ResetGameWorld();
            foreach (GameObject obj in gameObjects)
            {
                GameWorld.newObjects.Add(obj);
            }

        }
        public Scene(GameObject[] go, bool reset)
        {
            if (!reset)
            {
                foreach (GameObject obj in GameWorld.characters)
                {
                    if (!tempList.Exists(x => x.Id == obj.Id))
                    {
                        tempList.Add(obj);
                    }
                }
                foreach (GameObject obj in GameWorld.gameObjects)
                {
                    if (obj.Tag == "Enemy" || obj.Tag == "Player")
                    {
                        if (!tempList.Exists(x => x.Id == obj.Id))
                        {
                            tempList.Add(obj);
                        }
                    }
                }
            }

            gameObjects = new List<GameObject>();
            foreach (GameObject g in go)
            {
                if (g.Tag == "Untagged")
                {
                    gameObjects.Add(g);
                }
            }
            if (!reset)
            {
                ResetGameWorld();
                ClearGameWorld();
            }

            foreach (GameObject obj in gameObjects)
            {
                if (obj.Tag == "Untagged")
                {
                    GameWorld.newObjects.Add(obj);
                }
            }
            foreach (GameObject obj in tempList)
            {
                GameWorld.objectsToRemove.Add(obj);
                if (obj.Tag == "Player" || obj.Tag == "Enemy")
                {
                    GameObject scoreGameObject = new GameObject();
                    scoreGameObject.playerName = obj.playerName;
                    scoreGameObject.Id = obj.Id;
                    scoreGameObject.Tag = "Score";
                    scoreGameObject.kills = obj.kills;
                    scoreGameObject.DamageDone = obj.DamageDone;
                    scoreGameObject.TotalScore = obj.TotalScore;
                    GameWorld.newObjects.Add(scoreGameObject);
                }
            }
            tempList.Clear();
        }
        public Scene(List<GameObject> go)
        {
            gameObjects = new List<GameObject>();
            foreach (GameObject g in go)
            {
                gameObjects.Add(g);
            }
            ResetGameWorld();
            foreach (GameObject obj in gameObjects)
            {
                GameWorld.newObjects.Add(obj);
            }
        }
        #endregion
        #region Specific Standard Scenes
        public static Scene MainMenu()
        {
            GameWorld.Instance.ResetItemsAndAbilities();
            GameObject[] included = new GameObject[5];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaNewGame", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new OnClick(included[i], "NewGame"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaOptions", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2.75f - 40);
                        included[i].AddComponent(new OnClick(included[i], "Options"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaCredits", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4.25f - 40);
                        included[i].AddComponent(new OnClick(included[i], "Credits"));
                        break;
                    case 3:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaExitGame", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "ExitGame"));
                        break;
                    case 4:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaStatistics", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 3.5f - 40);
                        included[i].AddComponent(new OnClick(included[i], "Statistic"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "MainMenu";
            return send;
        }
        public static Scene NewGame()
        {
            GameWorld.Instance.ResetItemsAndAbilities();
            GameObject[] included = new GameObject[4];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaPractice", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeChooseRound"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaJoin", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 3 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Join"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaHost", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Host"));
                        break;
                    case 3:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "MainMenu"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "NewGame";
            return send;
        }
        public static Scene Join()
        {

            GameObject[] included = new GameObject[3];
            included[0] = new GameObject();
            included[0].AddComponent(new SpriteRenderer(included[0], "AlphaTryJoinIP", 0));
            included[0].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
            included[0].AddComponent(new OnClick(included[0], "Joining"));


            included[1] = new GameObject();
            included[1].AddComponent(new SpriteRenderer(included[1], "AlphaBack", 0));
            included[1].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
            included[1].AddComponent(new OnClick(included[1], "NewGame"));

            included[2] = new GameObject();
            included[2].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
            included[2].AddComponent(new IPInput(included[2]));
            (included[0].GetComponent("OnClick") as OnClick).AddIPRelation((included[2].GetComponent("IPInput") as IPInput));


            //included[0].AddComponent(new TestClient());
            Scene send = new Scene(included);
            send.scenetype = "Join";
            return send;
        }
        public static Scene Joined(string ip)
        {
            if (ip != null || ip == string.Empty)
            {
                GameWorld.Instance.client = new TestClient(ip);
                GameWorld.Instance.canClient = false;
                GameWorld.Instance.showServer = true;
            }

            GameObject[] included = new GameObject[1];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "NewGame"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "Joined";
            return send;
        }
        public static Scene Host(string ip)
        {
            //server start
            GameWorld.Instance.server = new Process();
            GameWorld.Instance.server.StartInfo.FileName = "TestServer.exe";
            GameWorld.Instance.server.EnableRaisingEvents = true;
            GameWorld.Instance.server.Start();
            GameWorld.Instance.client = new TestClient(ip);
            GameWorld.Instance.canClient = false;
            GameWorld.Instance.showServer = true;
            GameWorld.Instance.client.isHost = true;

            GameObject[] included = new GameObject[9];
            included[0] = new GameObject();
            included[0].AddComponent(new SpriteRenderer(included[0], "AlphaBack", 0));
            included[0].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
            included[0].AddComponent(new OnClick(included[0], "NewGame"));



            included[1] = new GameObject();
            included[1].AddComponent(new SpriteRenderer(included[1], "AlphaStart", 0));
            included[1].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 1 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
            included[1].AddComponent(new OnClick(included[1], "Play"));



            included[2] = new GameObject();
            included[2].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 2, 64 * 2);
            included[2].AddComponent(new SpriteRenderer(included[2], "LobbyMenuSheet", 0));
            included[2].AddComponent(new Animator(included[2]));
            included[2].AddComponent(new LobbyMenuButton(included[2], "PillarHole"));

            included[3] = new GameObject();
            included[3].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 3, 64 * 2);
            included[3].AddComponent(new SpriteRenderer(included[3], "LobbyMenuSheet", 0));
            included[3].AddComponent(new Animator(included[3]));
            included[3].AddComponent(new LobbyMenuButton(included[3], "Hole"));

            included[4] = new GameObject();
            included[4].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 4, 64 * 2);
            included[4].AddComponent(new SpriteRenderer(included[4], "LobbyMenuSheet", 0));
            included[4].AddComponent(new Animator(included[4]));
            included[4].AddComponent(new LobbyMenuButton(included[4], "Pillar"));

            included[5] = new GameObject();
            included[5].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 5, 64 * 2);
            included[5].AddComponent(new SpriteRenderer(included[5], "LobbyMenuSheet", 0));
            included[5].AddComponent(new Animator(included[5]));
            included[5].AddComponent(new LobbyMenuButton(included[5], "Standard"));

            included[6] = new GameObject();
            included[6].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 3 + 32, 64 * 3);
            included[6].AddComponent(new SpriteRenderer(included[6], "LobbyMenuSheet", 0));
            included[6].AddComponent(new Animator(included[6]));
            included[6].AddComponent(new LobbyMenuButton(included[6], "7"));

            included[7] = new GameObject();
            included[7].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 4 + 32, 64 * 3);
            included[7].AddComponent(new SpriteRenderer(included[7], "LobbyMenuSheet", 0));
            included[7].AddComponent(new Animator(included[7]));
            included[7].AddComponent(new LobbyMenuButton(included[7], "5"));

            included[8] = new GameObject();
            included[8].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width - 64 * 5 + 32, 64 * 3);
            included[8].AddComponent(new SpriteRenderer(included[8], "LobbyMenuSheet", 0));
            included[8].AddComponent(new Animator(included[8]));
            included[8].AddComponent(new LobbyMenuButton(included[8], "3"));


            Scene send = new Scene(included);
            send.scenetype = "Host";
            return send;

        }
        public static Scene Practice()
        {
            GameWorld.gameState = GameState.ingame;
            GameObject[] included = new GameObject[0];
            Scene send = new Scene(included);
            send.scenetype = "Practice";
            Player.gold = 10000;
            return send;
        }
        public static Scene PracticeChooseRound()
        {
            GameObject[] included = new GameObject[4];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaRoundsThree", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeRoundsThree"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaRoundsFive", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 3 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeRoundsFive"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaRoundsSeven", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeRoundsSeven"));
                        break;
                    case 3:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "NewGame"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "PracticeChooseRound";
            return send;
        }
        public static Scene PracticeChooseMap()
        {
            GameObject[] included = new GameObject[5];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaMapMap", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 7) * 2 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeMapMap"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaMapHoleMap", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 7) * 3 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticeHoleMap"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaMapPillarMap", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 7) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PracticePillarMap"));
                        break;
                    case 3:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaMapPillarHoleMap", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 7) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "PillarHoleMap"));
                        break;
                    case 4:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 7) * 6 - 40);
                        included[i].AddComponent(new OnClick(included[i], "NewGame"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "PracticeChooseMap";
            return send;
        }
        public static Scene Play()
        {
            GameWorld.gameState = GameState.ingame;
            GameObject[] included = new GameObject[0];
            Scene send = new Scene(included);
            send.scenetype = "Play";
            return send;
        }
        public static Scene Login()
        {
            GameObject[] included = new GameObject[5];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaLogIn", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 1 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "LoginAttempt"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaCreateAccount", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "CreateAccount"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaExitGame", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "ExitGame"));
                        break;
                    case 3:
                        included[i] = new GameObject();
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new Username(included[i]));
                        break;
                    case 4:
                        included[i] = new GameObject();
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2.5f - 40);
                        included[i].AddComponent(new Password(included[i]));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "Login";
            return send;
        }
        public static Scene CreateAccount()
        {
            GameObject[] included = new GameObject[6];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaCreateAccount", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 1 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "CreateAttempt"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 3) * 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Login"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaExitGame", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "ExitGame"));
                        break;
                    case 3:
                        included[i] = new GameObject();
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new Username(included[i]));
                        break;
                    case 4:
                        included[i] = new GameObject();
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2.5f - 40);
                        included[i].AddComponent(new Password(included[i]));
                        included[i].Name = "Password";
                        break;
                    case 5:
                        included[i] = new GameObject();
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 3 - 40);
                        included[i].AddComponent(new Password(included[i]));
                        included[i].Name = "Recheck Password";
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "CreateAccount";
            return send;
        }
        public static Scene PostScreen()
        {
            GameWorld.gameState = GameState.offgame;
            GameWorld.buyPhase = true;
            CreateAbility.abilityIndex = 0;
            GameWorld.Instance.waitingForServerResponse = false;
            //GameWorld.Instance.DrawScore();
            //ClearGameWorld();
            GameObject[] included = new GameObject[1];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaToMainMenu", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) * 1 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "MainMenu"));
                        break;
                }
            }
            Scene send = new Scene(included, false);
            send.scenetype = "PostScreen";
            return send;
        }
        public static Scene Statistic()
        {
            GameObject[] included = new GameObject[1];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) * 1 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "MainMenu"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "Statistic";
            return send;
        }
        public static Scene Option()
        {
            GameObject[] included = new GameObject[4];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "Mute", 0));
                        included[i].AddComponent(new Animator(included[i]));
                        included[i].AddComponent(new Muter(included[i]));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - (203 / 2), (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 2 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Mute"));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "MainMenu"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "Option";
            return send;
        }
        public static Scene Credits()
        {
            GameObject[] included = new GameObject[4];
            for (int i = 0; i < included.Length; i++)
            {
                included[i] = new GameObject();
                switch (i)
                {
                    case 0:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaCreditsInfo2", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - (1200 / 2), (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - (600 / 2));
                        break;
                    case 1:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaBack", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "MainMenu"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "Credits";
            return send;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (GameObject obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void LoadContent(ContentManager content)
        {


            foreach (GameObject obj in gameObjects)
            {
                obj.LoadContent(content);
            }
        }

        public void Update()
        {


            foreach (GameObject obj in gameObjects)
            {
                obj.Update();
            }
        }

        public static void ResetGameWorld()
        {
            GameWorld.newObjects.Clear();
            foreach (GameObject obj in GameWorld.gameObjects)
            {
                GameWorld.objectsToRemove.Add(obj);
            }
            GameWorld.characters.Clear();
            GameWorld.characterColliders.Clear();
        }

        public static void ClearGameWorld()
        {
            //foreach (GameObject go in GameWorld.gameObjects)
            //{
            //    tempList.Add(go);
            //}
            //foreach (GameObject go in GameWorld.newObjects)
            //{
            //    tempList.Add(go);
            //}
            //foreach (GameObject go in GameWorld.objectsToRemove)
            //{
            //    tempList.Add(go);
            //}

            //tempList.Clear();
            //GameWorld.gameObjects.Clear();
            //GameWorld.Instance.ResetCharacters();
            //GameWorld.characters.Clear();
            //GameWorld.characterColliders.Clear();
            Player.abilities.Clear();
            Player.deathAbilities.Clear();
            Player.items.Clear();
            GameWorld.itemList.Clear();
            GameWorld.abilityList.Clear();
            //ResetGameWorld();
        }
    }
}

