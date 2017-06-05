using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Diagnostics;

namespace MagicGladiators
{
    class OnClick : Component, IUpdateable, ILoadable
    {
        private string destination;
        private Rectangle rectangle;

        private IPInput IPRel;

        public OnClick(GameObject go, string destination) : base(go)
        {
            this.destination = destination;
        }

        public void LoadContent(ContentManager content)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
            rectangle = new Rectangle
                    (
                        (int)(gameObject.transform.position.X + spriteRenderer.Offset.X),
                        (int)(gameObject.transform.position.Y + spriteRenderer.Offset.Y),
                        spriteRenderer.Rectangle.Width,
                        spriteRenderer.Rectangle.Height
                    );
            lastStates = Mouse.GetState();
        }

        MouseState lastStates;
        public void Update()
        {
            MouseState m = Mouse.GetState();
            if (m.LeftButton != lastStates.LeftButton)
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (
                        m.X > rectangle.X && m.X < rectangle.X + rectangle.Width &&
                        m.Y > rectangle.Y && m.Y < rectangle.Y + rectangle.Height
                        )
                    {
                        switch (destination)
                        {
                            case "NewGame":
                                CreateAbility.abilityIndex = 0;
                                if (GameWorld.Instance.server != null)
                                {
                                    try
                                    {
                                        GameWorld.Instance.server.Kill();
                                    } catch (Exception) { }
                                    try
                                    {
                                        GameWorld.Instance.server = null;
                                    } catch (Exception) { }
                                }
                                if (GameWorld.Instance.client != null)
                                {
                                    GameWorld.Instance.client.Disconnect();
                                    GameWorld.Instance.client = null;
                                    GameWorld.Instance.canClient = true;
                                }
                                GameWorld.gameState = GameState.offgame;
                                GameWorld.buyPhase = true;
                                Scene.tempList.Clear();
                                GameWorld.Instance.NextScene = Scene.NewGame();
                                break;
                            case "MainMenu":
                                CreateAbility.abilityIndex = 0;
                                if (GameWorld.Instance.server != null)
                                {
                                    try
                                    {
                                        GameWorld.Instance.server.Kill();
                                    } catch (Exception) { }
                                    try
                                    {
                                        GameWorld.Instance.server = null;
                                    } catch (Exception) { }
                                }
                                if (GameWorld.Instance.client != null)
                                {
                                    GameWorld.Instance.client.Disconnect();
                                    GameWorld.Instance.client = null;
                                    GameWorld.Instance.canClient = true;
                                }
                                GameWorld.gameState = GameState.offgame;
                                GameWorld.buyPhase = true;
                                Scene.tempList.Clear();
                                GameWorld.Instance.NextScene = Scene.MainMenu();
                                break;
                            case "Join":
                                GameWorld.Instance.NextScene = Scene.Join();
                                break;
                            case "Host":
                                GameWorld.Instance.NextScene = Scene.Host("localhost");
                                break;
                            case "Lobby":
                                break;
                            case "Practice":
                                Player.gold = 10000;
                                GameWorld.Instance.NextScene = Scene.Practice();
                                break;
                            case "ExitGame":
                                if (GameWorld.Instance.server != null)
                                {
                                    GameWorld.Instance.server.Kill();
                                }
                                GameWorld.Instance.Exit();
                                break;
                            case "Joining":
                                if (GameWorld.Instance.canClient)
                                {
                                    GameWorld.Instance.NextScene = Scene.Joined(IPRel.GetIPString());
                                }
                                break;
                            case "PracticeChooseRound":
                                GameWorld.Instance.NextScene = Scene.PracticeChooseRound();
                                break;
                            case "PracticeRoundsThree":
                                GameWorld.Instance.NextScene = Scene.PracticeChooseMap();
                                GameWorld.numberOfRounds = 3;
                                break;
                            case "PracticeRoundsFive":
                                GameWorld.Instance.NextScene = Scene.PracticeChooseMap();
                                GameWorld.numberOfRounds = 5;
                                break;
                            case "PracticeRoundsSeven":
                                GameWorld.Instance.NextScene = Scene.PracticeChooseMap();
                                GameWorld.numberOfRounds = 7;
                                break;
                            case "PracticeMapMap":
                                GameWorld.Instance.NextScene = Scene.Practice();
                                GameWorld.selectedMap = "Map";
                                break;
                            case "PracticeHoleMap":
                                GameWorld.Instance.NextScene = Scene.Practice();
                                GameWorld.selectedMap = "HoleMap";
                                break;
                            case "PracticePillarMap":
                                GameWorld.Instance.NextScene = Scene.Practice();
                                GameWorld.selectedMap = "PillarMap";
                                break;
                            case "PillarHoleMap":
                                GameWorld.Instance.NextScene = Scene.Practice();
                                GameWorld.selectedMap = "PillarHoleMap";
                                break;
                            case "Play":
                                if (GameWorld.gameObjects.Exists(x => x.Tag == "Player") && GameWorld.gameObjects.Exists(x => x.Id != ""))
                                {
                                    if (GameWorld.gameObjects.Exists(x => x.Tag == "Enemy"))
                                    {
                                        if (GameWorld.Instance.client != null)
                                        {
                                            GameWorld.Instance.NextScene = Scene.Play();
                                            GameWorld.Instance.client.SendMapSettings(GameWorld.selectedMap, GameWorld.numberOfRounds);
                                            GameWorld.Instance.client.SendStartgame();
                                            foreach (GameObject go in GameWorld.Instance.client.readyList)
                                            {
                                                go.isReady = false;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "LoginAttempt":
                                string a = string.Empty;
                                string b = string.Empty;
                                foreach (GameObject obj in GameWorld.gameObjects)
                                {
                                    if (obj.GetComponent("Username") is Username)
                                    {
                                        a = (obj.GetComponent("Username") as Username).GetInput();
                                    }
                                    if (obj.GetComponent("Password") is Password)
                                    {
                                        b = (obj.GetComponent("Password") as Password).GetInput();
                                    }
                                }
                                if (a.Length > 0 && b.Length > 0)
                                {
                                    if (dbCon.i.IsMatch(dbTables.login, a, b) == dbMatching.Match)
                                    {
                                        GameWorld.playername = a;
                                        dbCon.i.FindNSetID(a);
                                        //GameWorld.Instance.player.playerName = a;
                                        GameWorld.Instance.NextScene = Scene.MainMenu();
                                    }
                                }
                                break;
                            case "CreateAccount":
                                GameWorld.Instance.NextScene = Scene.CreateAccount();
                                break;
                            case "Login":
                                GameWorld.Instance.NextScene = Scene.Login();
                                break;
                            case "CreateAttempt":
                                string c = string.Empty;
                                string d = string.Empty;
                                string e = string.Empty;
                                foreach (GameObject obj in GameWorld.gameObjects)
                                {
                                    if (obj.GetComponent("Username") is Username)
                                    {
                                        c = (obj.GetComponent("Username") as Username).GetInput();
                                    }
                                    if (obj.GetComponent("Password") is Password)
                                    {
                                        if (obj.Name == "Password")
                                            d = (obj.GetComponent("Password") as Password).GetInput();
                                        if (obj.Name == "Recheck Password")
                                            e = (obj.GetComponent("Password") as Password).GetInput();
                                    }
                                }
                                if (c.Length > 0 && d.Length > 0 && e.Length > 0)
                                {
                                    if (d == e)
                                    {
                                        if (dbCon.i.CreateAccount(c, d) == dbCreate.Success)
                                        {
                                            GameWorld.Instance.NextScene = Scene.Login();
                                        }
                                    }
                                }
                                break;
                            case "BackToLobby":
                                GameWorld.Instance.NextScene = Scene.Joined(null);
                                break;
                        }
                    }
                }
            }
            lastStates = m;
        }

        public void AddIPRelation(IPInput IPRel)
        {
            this.IPRel = IPRel;
        }
    }
}
