using MagicGladiators.Components.Composites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Collections.Generic;

namespace MagicGladiators
{

    public enum ObjectType { }
    
    



    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld:Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        public static List<GameObject> gameObjects;
        public static List<GameObject> newObjects;
        public static List<GameObject> objectsToRemove;

        public Scene CurrentScene { get; set; }
        public Scene NextScene { get; set; }

        public List<Collider> Colliders { get; private set; }
        public List<Collider> newColliders { get; private set; }

        public List<Collider> CircleColliders { get; set; }
        public List<Collider> newCircleColliders { get; set; }


        public float deltaTime { get; set; }


        private static GameWorld instance;
        public static GameWorld Instance


        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            gameObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();

            Colliders = new List<Collider>();
            newColliders = new List<Collider>();
            CircleColliders = new List<Collider>();
            newCircleColliders = new List<Collider>();

            CurrentScene = Scene.MainMenu();

            //Director director = new Director(new MapBuilder());
            //Texture2D sprite = Content.Load<Texture2D>("StandardMap");
            //gameObjects.Add(director.Construct(new Vector2(Window.ClientBounds.Width / 2 - sprite.Width / 2, Window.ClientBounds.Height / 2 - sprite.Height / 2)));

            //Vector2 mapCenter = new Vector2(gameObjects[0].transform.position.X + sprite.Width / 2, gameObjects[0].transform.position.Y + sprite.Height / 2);
            ////float mapRadius = (gameObjects[0].GetComponent("Collider") as Collider).CircleCollisionBox.Radius;

            //director = new Director(new PlayerBuilder());
            //gameObjects.Add(director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16)));


            //director = new Director(new DummyBuilder());
            //gameObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 - 280, mapCenter.Y - 16)));
            //gameObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 + 280, mapCenter.Y - 16)));
            //gameObjects.Add(director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 16 + 280)));


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            CurrentScene.LoadContent(Content);

            //foreach (GameObject go in gameObjects)
            //{
            //    go.LoadContent(Content);
            //}
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            graphics.ApplyChanges();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            //foreach (GameObject go in gameObjects)
            //{
            //    if (go.CurrentHealth < 0)
            //    {
            //        objectsToRemove.Add(go);
            //    }
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.F2))
            //{
            //    bool testfor = false;
            //    foreach (GameObject obj in gameObjects)
            //    {
            //        if (obj.GetComponent("Server") is Server || obj.GetComponent("Client") is Client)
            //        {
            //            testfor = true;
            //        }
            //    }
            //    if (!testfor)
            //    {
            //        GameObject server = new GameObject();
            //        server.AddComponent(new Server(server));
            //        server.LoadContent(this.Content);
            //        gameObjects.Add(server);
            //    }
            //} else if (Keyboard.GetState().IsKeyDown(Keys.F3))
            //{
            //    bool testfor = false;
            //    foreach (GameObject obj in gameObjects)
            //    {
            //        if (obj.GetComponent("Server") is Server || obj.GetComponent("Client") is Client)
            //        {
            //            testfor = true;
            //        }
            //    }
            //    if (!testfor)
            //    {
            //        GameObject client = new GameObject();
            //        client.AddComponent(new Client(client));
            //        client.LoadContent(this.Content);
            //        gameObjects.Add(client);
            //    }
            //}

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            //foreach (GameObject go in gameObjects)
            //{
            //    go.Update();
                
            //}
            UpdateLevel();
            if (CurrentScene != null)
            {
                CurrentScene.Update();
            }
            base.Update(gameTime);
        }

        public void UpdateLevel()
        {
            //if (objectsToRemove.Count > 0)
            //{
            //    foreach (GameObject go in objectsToRemove)
            //    {
            //        CircleColliders.Remove((go.GetComponent("Collider") as Collider));
            //        gameObjects.Remove(go);
            //    }
            //    objectsToRemove.Clear();
            //}

            //if (newObjects.Count > 0)
            //{
            //    gameObjects.AddRange(newObjects);
            //    newObjects.Clear();
            //}

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.DarkRed);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //foreach (GameObject go in gameObjects)
            //{
            //    go.Draw(spriteBatch);
            //}
            CurrentScene.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public GameObject FindGameObjectWithTag(string tag)
        {
            return gameObjects.Find(x => x.Tag == tag);
        }
    }

}
