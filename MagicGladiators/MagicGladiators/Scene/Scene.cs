﻿using MagicGladiators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    public class Scene:ILoadable, IUpdateable, IDrawable
    {
        List<GameObject> gameObjects;
        public string scenetype;

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
            GameObject[] included = new GameObject[4];
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
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 3 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Options"));
                        break;
                    case 2:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaCredits", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 4 - 40);
                        included[i].AddComponent(new OnClick(included[i], "Credits"));
                        break;
                    case 3:
                        included[i].AddComponent(new SpriteRenderer(included[i], "AlphaExitGame", 0));
                        included[i].transform.position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2 - 180, (GameWorld.Instance.GraphicsDevice.Viewport.Height / 6) * 5 - 40);
                        included[i].AddComponent(new OnClick(included[i], "ExitGame"));
                        break;
                }
            }
            Scene send = new Scene(included);
            send.scenetype = "MainMenu";
            return send;
        }
        public static Scene NewGame()
        {
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
            GameObject[] included = new GameObject[1];
            included[0] = new GameObject();
            included[0].AddComponent(new Client(included[0]));
            Scene send = new Scene(included);
            send.scenetype = "Join";
            return send;
        }
        public static Scene Host()
        {
            GameObject[] included = new GameObject[0];
            included[0] = new GameObject();
            included[0].AddComponent(new Server(included[0]));
            Scene send = new Scene(included);
            send.scenetype = "Host";
            return send;
        }
        public static Scene Practice()
        {
            GameObject[] included = new GameObject[0];
            Scene send = new Scene(included);
            send.scenetype = "Practice";
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

        public void ResetGameWorld()
        {
            GameWorld.newObjects.Clear();
            foreach (GameObject obj in GameWorld.gameObjects)
            {
                GameWorld.objectsToRemove.Add(obj);
            }
        }
    }
}

