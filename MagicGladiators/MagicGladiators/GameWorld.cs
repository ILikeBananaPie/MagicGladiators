﻿using MagicGladiators.Components.Composites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Data.SQLite;
using Microsoft.Xna.Framework.Media;

namespace MagicGladiators
{

    public enum ObjectType { }

    public enum GameState { offgame, ingame }



    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int abilityIndex = 0;
        private List<IAbility> abilityListTest = new List<IAbility>();
        private bool aliveCanBind = true;
        private bool deathCanBind = true;
        private Keys[] keys;
        private string bindName;

        public bool canUpdateStatistics = true;

        public bool waitingForServerResponse { get; set; } = false;

        public static List<GameObject> gameObjects;
        public static List<GameObject> newObjects;
        public static List<GameObject> objectsToRemove;

        public static List<GameObject> itemList;
        public static List<GameObject> abilityList = new List<GameObject>();

        public Scene CurrentScene { get; set; }
        public Scene NextScene { get; set; }

        public List<Collider> Colliders { get; private set; }
        public List<Collider> newColliders { get; private set; }

        public List<Collider> CircleColliders { get; set; }
        public List<Collider> newCircleColliders { get; set; }

        public GameObject player { get; private set; }

        public float deltaTime { get; set; }

        private bool canBuy = true;
        private bool canUpgrade = true;
        private bool canReady = true;

        public bool MouseOnIcon { get; set; } = false;

        private int buySpellX;
        private int buySpellY;

        private SpriteFont fontText;
        private SpriteFont keyFont;
        private SpriteFont describtionFont;

        GameObject TooltipBox = new GameObject();

        private List<Collider> testList = new List<Collider>();
        private List<string> offensiveAbilities = new List<string>() { "HomingMissile", "Fireball", "Ricochet" };
        private List<string> defensiveAbilities = new List<string>() { "Deflect", "Invisibility", "Stone Armor" };
        private List<string> movementAbilities = new List<string>() { "Charge", "Blink", "Leap", "Recall" };

        //v.0.2
        private GameObject map;
        public float MapScale { get; set; } = 1;
        public static string selectedMap;

        private Vector2 mapCenter;
        private List<Vector2> playerpositions = new List<Vector2>();


        public static List<GameObject> playersAlive = new List<GameObject>();
        public static bool buyPhase = true;
        public static List<bool> readyList = new List<bool>();

        public static int numberOfRounds = 5;
        public static int currentRound = 1;

        public static List<GameObject> characters = new List<GameObject>();
        public static List<Collider> characterColliders = new List<Collider>();

        public bool showServer { get; set; } = false;
        private bool canServer = true;
        public bool canClient { get; set; } = true;
        public TestClient client;
        public Process server;
        //private TestServer server;
        private List<Thread> threads = new List<Thread>();
        private float clientTimer = 0;
        private bool sendPos = false;

        public static GameState gameState = GameState.offgame;

        public static string playername;

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
            //this.Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, 10);
            dbCon.i.StartDataBaseConnection();
            if (SaMM.i is SaMM) { }
            MediaPlayer.Volume = 0.06F;

            this.Window.Position = new Point(10, 10);
            TooltipBox.AddComponent(new SpriteRenderer(TooltipBox, "ToolTipBox", 1));

            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            buySpellX = Window.ClientBounds.Width - 144;
            buySpellY = Window.ClientBounds.Height - 144;

            gameObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();

            itemList = new List<GameObject>();

            Colliders = new List<Collider>();
            newColliders = new List<Collider>();
            CircleColliders = new List<Collider>();
            newCircleColliders = new List<Collider>();

            CurrentScene = Scene.Login();
            selectedMap = "PillarHoleMap";

            base.Initialize();
        }

        public void CreatePlayer()
        {
            /*
            Director director = new Director(new PlayerBuilder());
            player = director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16));
            newObjects.Add(player);
            foreach (GameObject go in newObjects)
            {
                if (go.Tag == "Player")
                {
                    go.LoadContent(Content);
                }
            }
            */
        }

        public void StartRound()
        {
            CreateMap(selectedMap);
            ResetCharacters();
            //NextScene = Scene.Play();

        }

        public void CreateDummies()
        {
            Director director = new Director(new DummyBuilder());
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 - 100, mapCenter.Y - 16)));
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 + 100, mapCenter.Y - 16)));
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 16 + 100)));
            foreach (GameObject go in newObjects)
            {
                if (go.Tag == "Dummy")
                {
                    go.LoadContent(Content);
                }
            }
        }

        public void ResetCharacters()
        {
            int index = 0;
            foreach (GameObject go in gameObjects)
            {
                if (go.Tag == "Player" || go.Tag == "Enemy")
                {
                    characters.Add(go);
                    characterColliders.Add((go.GetComponent("Collider") as Collider));
                }
            }
            gameObjects.Clear();
            CircleColliders.Clear();
            foreach (Collider col in characterColliders)
            {
                CircleColliders.Add(col);
            }
            foreach (GameObject go in characters)
            {
                go.CurrentHealth = go.MaxHealth;
                newObjects.Add(go);
                if (go.Tag == "Player")
                {
                    go.transform.position = playerpositions[go.ConnectionNumber];
                }
                if (go.Tag == "Enemy")
                {
                    if (index == 0)
                    {
                        go.transform.position = new Vector2(mapCenter.X - 16 - 280, mapCenter.Y - 16);
                    }
                    if (index == 1)
                    {
                        go.transform.position = new Vector2(mapCenter.X - 16 + 280, mapCenter.Y - 16);
                    }
                    if (index == 2)
                    {
                        go.transform.position = new Vector2(mapCenter.X - 16, mapCenter.Y - 16 + 280);
                    }
                    index++;
                }
            }
            characters.Clear();
            characterColliders.Clear();
            index = 0;
        }

        public void CreateVendorAbilities()
        {
            Director director = new Director(new AbilityIconBuilder());
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "HomingMissile", 110, "Fire a projectile in the target \n direction, moving towards the \n closest enemy."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Charge", 100, "Send you in the target direction."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Drain", 100, "Fire a slow moving projectile \n in the target direction."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Deflect", 100, "Create a shield around you, \n deflecting any spells coming \n your way."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Mine", 50, "Place a static mine at the \n target position. Will explode \n on contact."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "SpeedBoost", 100, "Increase your movement speed \n temporarily"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Chain", 100, "Fires a slow moving projectile, \n that pulls you and the target \n together for a period of time."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Blink", 100, "Instantly moves your character \n towards the target direction."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Nova", 100, "Send out 8 straight flying \n projectiles in different directions."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Spellshield", 110, "Create a shield around you, \n deleting any spells coming \n your way."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "StoneArmour", 100, "Increase your knockback \n resistance and reduces your \n movement speed temporarily."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Boomerang", 100, "Fire a projectile that return to \n your position"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Recall", 100, "Teleport you to the position, you \n had when casting the spell"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "GravityWell", 100, "Fire a projektile that pulls \n people in an area towards it."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "MirrorImage", 100, "Create 3 clones of yourself. \n The clones mimics your fireball"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Invisibility", 100, "Become invisible to other players"));

            int x = Player.deathAbilities.Count * 34;
            Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "RollingMeteor", 0, "Does something"));
            x = Player.deathAbilities.Count * 34;
            Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "DeathMine", 0, "Does something"));
            x = Player.deathAbilities.Count * 34;
            Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "Firewave", 0, "Does something"));
            x = Player.deathAbilities.Count * 34;
            Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "Critters", 0, "Does something"));
            x = Player.deathAbilities.Count * 34;
            Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "ShrinkMap", 0, "Does something"));
            //x = Player.deathAbilities.Count * 34;
            //Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "SlowField", 0, "Does something"));
            //x = Player.deathAbilities.Count * 34;
            //Player.deathAbilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), "IceField", 0, "Does something"));

            int index = 0;
            foreach (Component component in player.components)
            {
                if (component is IDeathAbility)
                {
                    component.key = CreateAbility.keys[index];
                    index++;
                }
            }
            int testIndex = 0;
            for (int i = 0; i < player.components.Count; i++)
            {
                if (player.components[i] is IDeathAbility)
                {
                    (player.components[i] as Ability).icon = Player.deathAbilities[testIndex];
                    testIndex++;
                }
            }
        }

        public void CreateVendorItems()
        {
            // name, hp, speed, dmgRes, lavaRes, value, knockRes, projectileSpeed, LifeSteal
            Director director = new Director(new ItemBuilder());
            string[] testItem = new string[] { "Speed", "0", "0.05", "0", "0", "25", "0", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "Hp", "5", "0", "0", "0", "25", "0", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "LavaRes", "0", "0", "0", "-0.02", "25", "0", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "DmgRes", "0", "0", "-0.02", "0", "25", "0", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "KnockRes", "0", "0", "0", "0", "25", "0.01", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "ProjectileSpeed", "0", "0", "0", "0", "25", "0", "0.05", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "LifeSteal", "0", "0", "0", "0", "25", "0", "0", "0.02", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "CDR", "0", "0", "0", "0", "25", "0", "0", "0", "0.05", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "AOE", "0", "0", "0", "0", "25", "0", "0", "0", "0", "0.1", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "Gold", "0", "0", "0", "0", "25", "0", "0", "0", "0", "0", "0.03" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
        }

        public void CreateMap(string map)
        {
            foreach (GameObject go in gameObjects)
            {
                if (go.Tag == "Map" || go.Tag == "Pillar" || go.Tag == "LavaSpot" || go.Tag == "Lava")
                {
                    objectsToRemove.Add(go);
                }
            }
            //Director director = new Director(new MapBuilder());
            //Texture2D sprite = Content.Load<Texture2D>("LavaBackGround");
            //newObjects.Add(director.ConstructMapPart(new Vector2(0, 0), "Lava"));

            Director director = new Director(new MapBuilder());
            Texture2D sprite = Content.Load<Texture2D>("StandardMap");
            newObjects.Add(director.ConstructMapPart(new Vector2(Window.ClientBounds.Width / 2 - sprite.Width / 2, Window.ClientBounds.Height / 2 - sprite.Height / 2), "Map"));

            foreach (GameObject go in newObjects)
            {
                if (go.Tag == "Map")
                {
                    mapCenter = new Vector2(go.transform.position.X + sprite.Width / 2, go.transform.position.Y + sprite.Height / 2);
                    //add map positions
                    if (playerpositions.Count == 0)
                    {
                        //top
                        playerpositions.Add(new Vector2(mapCenter.X - 16, mapCenter.Y - 300));
                        //top-right
                        playerpositions.Add(new Vector2(mapCenter.X + 200 - 16, mapCenter.Y - 200 - 16));
                        //right
                        playerpositions.Add(new Vector2(mapCenter.X + 300 - 32, mapCenter.Y - 16));
                        //down-right
                        playerpositions.Add(new Vector2(mapCenter.X + 200 - 16, mapCenter.Y + 200 - 16));
                        //down
                        playerpositions.Add(new Vector2(mapCenter.X - 16, mapCenter.Y + 300 - 32));
                        //down-left
                        playerpositions.Add(new Vector2(mapCenter.X - 200 - 16, mapCenter.Y + 200 - 16));
                        //left
                        playerpositions.Add(new Vector2(mapCenter.X - 300, mapCenter.Y - 16));
                        //up-left
                        playerpositions.Add(new Vector2(mapCenter.X - 200 - 16, mapCenter.Y - 200 - 16));

                    }
                }
            }

            #region "Hole Map"
            if (map == "HoleMap" || map == "PillarHoleMap")
            {
                //Lava spot for "Hole map"
                Texture2D lavaSpot = Content.Load<Texture2D>("LavaSpot");
                newObjects.Add(director.ConstructMapPart(new Vector2(Window.ClientBounds.Width / 2 - lavaSpot.Width / 2, Window.ClientBounds.Height / 2 - lavaSpot.Height / 2), "LavaSpot"));
            }
            #endregion

            #region "Pillar map"
            if (map == "PillarMap" || map == "PillarHoleMap")
            {
                //Pillars for Pillars Map
                newObjects.Add(director.ConstructMapPart(new Vector2(mapCenter.X - 16 - sprite.Width / 4, mapCenter.Y - 16 - sprite.Height / 4), "Pillar"));
                newObjects.Add(director.ConstructMapPart(new Vector2(mapCenter.X - 16 + sprite.Width / 4, mapCenter.Y - 16 - sprite.Height / 4), "Pillar"));
                newObjects.Add(director.ConstructMapPart(new Vector2(mapCenter.X - 16 - sprite.Width / 4, mapCenter.Y - 16 + sprite.Height / 4), "Pillar"));
                newObjects.Add(director.ConstructMapPart(new Vector2(mapCenter.X - 16 + sprite.Width / 4, mapCenter.Y - 16 + sprite.Height / 4), "Pillar"));
            }
            #endregion
        }

        public void buySpellPosition()
        {
            if (buySpellX == Window.ClientBounds.Width - 42)
            {
                buySpellX = Window.ClientBounds.Width - 144;
                buySpellY += 34;
            }
            else buySpellX += 34;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SaMM.i.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontText = Content.Load<SpriteFont>("fontText");
            keyFont = Content.Load<SpriteFont>("lunchtime");
            describtionFont = Content.Load<SpriteFont>("lunchtime");
            Content.Load<Texture2D>("ToolTipBox");
            TooltipBox.LoadContent(Content);

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

        bool pressed = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            foreach (GameObject go in gameObjects)
            {
                if (go.Tag == "Player")
                {
                    player = go;
                }
            }
            Rectangle rec = new Rectangle(10, 10, 10, 10);
            Circle circle = new Circle(10, 10, 10);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!pressed)
                {
                    if (CurrentScene.scenetype == "Practice" || CurrentScene.scenetype == "NewGame" || CurrentScene.scenetype == "Play")
                    {
                        if (GameWorld.Instance.server != null)
                        {
                            try
                            {
                                GameWorld.Instance.server.Kill();
                            }
                            catch (Exception) { }
                            try
                            {
                                GameWorld.Instance.server = null;
                            }
                            catch (Exception) { }
                        }
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.Disconnect();
                            GameWorld.Instance.client = null;
                            GameWorld.Instance.canClient = true;
                        }
                        GameWorld.gameState = GameState.offgame;
                        GameWorld.buyPhase = true;
                        NextScene = Scene.MainMenu();
                    }
                    else if (CurrentScene.scenetype == "PracticeChooseRound" || CurrentScene.scenetype == "Host")
                    {
                        if (GameWorld.Instance.server != null)
                        {
                            try
                            {
                                GameWorld.Instance.server.Kill();
                            }
                            catch (Exception) { }
                            try
                            {
                                GameWorld.Instance.server = null;
                            }
                            catch (Exception) { }
                        }
                        if (GameWorld.Instance.client != null)
                        {
                            GameWorld.Instance.client.Disconnect();
                            GameWorld.Instance.client = null;
                            GameWorld.Instance.canClient = true;
                        }
                        GameWorld.gameState = GameState.offgame;
                        GameWorld.buyPhase = true;
                        NextScene = Scene.NewGame();
                    }
                    else
                    {
                        if (server != null)
                        {
                            try
                            {
                                server.Kill();
                            }
                            catch (Exception) { }
                            try
                            {
                                server = null;
                            }
                            catch (Exception) { }
                        }
                        Exit();
                    }
                }
                pressed = true;
            }
            else { pressed = false; }

            // TODO: Add your update logic here
            try
            {
                graphics.ApplyChanges();
            }
            catch (NullReferenceException nre) { }

            // TODO: Add your update logic here
            if (CurrentScene.scenetype == "Practice" || CurrentScene.scenetype == "Play")
            {
                SaMM.i.NewPlaystate(SongState.Battle);
            }
            else
            {
                SaMM.i.NewPlaystate(SongState.Menu);
            }
            MouseState mouse = Mouse.GetState();
            Circle mouseCircle = new Circle(mouse.X, mouse.Y, 1);
            if (CurrentScene.scenetype == "Play")
            {
                foreach (GameObject go in gameObjects)
                {
                    string name = go.Tag;
                    if (go.CurrentHealth < 0 && !buyPhase)
                    {
                        if (go.GetComponent("Enemy") is Enemy)
                        {
                            //(go.GetComponent("Enemy") as Enemy).UponDeath();
                        }
                        objectsToRemove.Add(go);
                        if (client != null)
                        {
                            if (go.Tag == "Player")
                            {
                                name = "Enemy";
                            }
                            client.SendRemoval(name, go.Id);
                        }
                    }
                }
            }
            if (CurrentScene.scenetype == "Practice")
            {
                foreach (GameObject go in gameObjects)
                {
                    if (go.CurrentHealth < 0)
                    {
                        objectsToRemove.Add(go);
                    }
                }
            }

            UpdateDeathAbilities();

            if (buyPhase)
            {
                UpdateBuyItem(mouse, mouseCircle);

                UpdateBuyAbility(mouse, mouseCircle);

                UpdateAbilityRebind(mouse, mouseCircle);

                UpdateItemUpgrade(mouse, mouseCircle);
            }

            if (!canClient)
            {
                if (client != null)
                {
                    client.Update();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F9) && CurrentScene.scenetype == "Practice")
            {
                ResetItemsAndAbilities();
                NextScene = Scene.Practice();
            }
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.F7))
            {
                dbCon.i.AddBattleNotWonToSavedID();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F8))
            {
                dbCon.i.AddBattleWonToSavedID();
            }
#endif 

            UpdateMouseRelease(mouse);

            UpdateMouseOnIcon(mouseCircle);


            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateLists();

            UpdateLevel();

            PhaseCheck();

            if (NextScene != null)
            {
                NextScene.LoadContent(Content);
                CurrentScene = NextScene;
                if (NextScene.scenetype == "Play")
                {
                    CreateMap(selectedMap);
                    ResetCharacters();
                    Director ability = new Director(new AbilityIconBuilder());
                    Player.abilities.Add(ability.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68, Window.ClientBounds.Height - 42), "Fireball", 0, ""));
                    (player.components.Last() as Ability).icon = Player.abilities.Last();
                    player.playerName = playername;
                    /*
                    Director director = new Director(new PlayerBuilder());
                    player = director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16));
                    Director ability = new Director(new AbilityIconBuilder());
                    Player.abilities.Add(ability.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68,Window.ClientBounds.Height - 42), "Fireball", 0, ""));
                    newObjects.Add(player);
                    */
                    //CreateDummies();

                    CreateVendorItems();

                    CreateVendorAbilities();

                    foreach (GameObject go in gameObjects)
                    {
                        go.LoadContent(Content);
                    }
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Fireball")
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                }
                if (NextScene.scenetype == "Practice")
                {
                    CreateMap(selectedMap);
                    ResetCharacters();
                    Director ability = new Director(new AbilityIconBuilder());
                    Player.abilities.Add(ability.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68, Window.ClientBounds.Height - 42), "Fireball", 0, ""));

                    Director director = new Director(new PlayerBuilder());
                    player = director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16));
                    newObjects.Add(player);
                    (player.components.Last() as Ability).icon = Player.abilities.Last();

                    CreateDummies();

                    CreateVendorItems();

                    CreateVendorAbilities();

                    foreach (GameObject go in gameObjects)
                    {
                        go.LoadContent(Content);
                    }
                }
                if (NextScene.scenetype == "Host" || NextScene.scenetype == "Joined")
                {
                    Director director = new Director(new PlayerBuilder());
                    player = director.Construct(new Vector2(50));
                    newObjects.Add(player);

                    UpdateLevel();

                    client.ConnectToServer();
                }

                NextScene = null;
                GC.Collect();
            }
            base.Update(gameTime);
        }

        public void ServerUpdate()
        {
            while (true)
            {
                //server.Update();
            }
        }

        public void ClientUpdate()
        {
            while (true)
            {
                client.Update();
            }
        }

        public void ClientDraw()
        {
            while (true)
            {
                client.Draw();
            }
        }

        public void UpdateDeathAbilities()
        {
            if (CurrentScene.scenetype == "Play")
            {
                if (player != null)
                {
                    if (player.CurrentHealth <= 0 && !buyPhase)
                    {
                        foreach (Component component in player.components)
                        {
                            if (component is IDeathAbility)
                            {
                                (component as Ability).Cooldown();
                                (component as IDeathAbility).Update();
                            }
                        }
                    }
                }
            }
            if (CurrentScene.scenetype == "Practice")
            {
                if (player != null)
                {
                    if (player.CurrentHealth <= 0)
                    {
                        foreach (Component component in player.components)
                        {
                            if (component is IDeathAbility)
                            {
                                (component as Ability).Cooldown();
                                (component as IDeathAbility).Update();
                            }
                        }
                    }
                }
            }
        }

        public void ResetItemsAndAbilities()
        {
            abilityList.Clear();
            buyPhase = true;
            currentRound = 1;
            itemList.Clear();
            //numberOfRounds = 3;
            playersAlive.Clear();
            readyList.Clear();
            buySpellX = Window.ClientBounds.Width - 144;
            buySpellY = Window.ClientBounds.Height - 144;
            Player.items.Clear();
            Player.abilities.Clear();

            if (CurrentScene.scenetype == "Practice")
            {
                Player.gold = 10000;
            }
            else Player.gold = 300;


            Player.deathAbilities.Clear();
            CreateAbility.abilityIndex = 0;
        }

        public void UpdateBuyItem(MouseState mouse, Circle mouseCircle)
        {
            //only in buy phase
            if (buyPhase)
            {
                bool isBuying = true;
                foreach (GameObject go in itemList)
                {
                    Item item = (go.GetComponent("Item") as Item);
                    if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                    {
                        if (canBuy && mouse.RightButton == ButtonState.Pressed && Player.items.Count <= 6 && Player.gold >= item.Value)
                        {
                            canBuy = false;

                            foreach (GameObject go2 in Player.items)
                            {
                                Item item2 = (go2.GetComponent("Item") as Item);
                                if (item2.Name == item.Name && item2.upgradeLevel < 3)
                                {
                                    item2.Upgrade();
                                    Player.gold -= item.Value;
                                    isBuying = false;
                                    break;
                                }
                            }

                            if (isBuying && Player.items.Count < 6)
                            {
                                Director director = new Director(new ItemBuilder());
                                Player.items.Add(director.ConstructItem(new Vector2(0, 200), new string[] { item.Name, item.Health.ToString(), item.Speed.ToString(), item.DamageResistance.ToString(), item.LavaResistance.ToString(), (item.Value / 2).ToString(), item.KnockBackResistance.ToString(), item.ProjectileSpeed.ToString(), item.LifeSteal.ToString(), item.CDR.ToString(), item.AOEBonus.ToString(), item.GoldBonusPercent.ToString() }));
                                Player.gold -= item.Value;
                                (player.GetComponent("Player") as Player).UpdateStats();
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void UpdateBuyAbility(MouseState mouse, Circle mouseCircle)
        {
            if (buyPhase)
            {
                foreach (GameObject go in abilityList)
                {
                    if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                    {
                        MouseOnIcon = true;

                        if (canBuy && mouse.RightButton == ButtonState.Pressed && Player.abilities.Count <= 7)
                        {
                            AbilityIcon ability = (go.GetComponent("AbilityIcon") as AbilityIcon);
                            canBuy = false;

                            if (Player.gold >= ability.Value)
                            {
                                Director director = new Director(new AbilityIconBuilder());
                                int x = Player.abilities.Count * 34;
                                Player.abilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), ability.Name, ability.Value, ability.Text));
                                (Player.abilities[Player.abilities.Count - 1].GetComponent("AbilityIcon") as AbilityIcon).index = abilityIndex;
                                //(Player.abilities[Player.abilities.Count - 1].GetComponent("AbilityIcon") as AbilityIcon).Name = go.Name;
                                abilityIndex++;
                                Player.gold -= ability.Value;

                                CreateAbility ca = new CreateAbility(ability.Name);
                                player.AddComponent(ca.GetComponent(player, player.transform.position));
                                abilityList.Remove(ability.gameObject);
                                CooldownIcon();
                                break;
                            }
                        }
                    }
                    else
                    {
                        MouseOnIcon = false;
                    }
                }
            }
        }

        public void CooldownIcon()
        {
            (player.components.Last() as Ability).icon = Player.abilities.Last();

            //if (player.components.Last().Name == "HomingMissile")
            //{
            //    (player.GetComponent("HomingMissile") as HomingMissile).icon = Player.abilities.Last();
            //}
            //if (player.components.Last().Name == "Boomerang")
            //{
            //    (player.GetComponent("Boomerang") as Boomerang).icon = Player.abilities.Last();
            //}
            //if (player.components.Last().Name == "Chain")
            //{
            //    (player.GetComponent("Chain") as Chain).icon = Player.abilities.Last();
            //}
            //if (player.components.Last().Name == "HomingMissile")
            //{
            //    (player.GetComponent("HomingMissile") as HomingMissile).icon = Player.abilities.Last();
            //}


        }

        public void UpdateAbilityRebind(MouseState mouse, Circle mouseCircle)
        {
            if (aliveCanBind)
            {
                foreach (GameObject go in Player.abilities)
                {
                    if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                    {
                        if (canBuy && mouse.LeftButton == ButtonState.Pressed && player.CurrentHealth > 0 && !(go is IDeathAbility))
                        {
                            //rebind ability
                            KeyboardState keyState = Keyboard.GetState();
                            keys = keyState.GetPressedKeys();
                            if (keys.Length > 0)
                            {
                                aliveCanBind = false;
                                bindName = (go.GetComponent("AbilityIcon") as AbilityIcon).Name;
                            }
                        }
                    }
                }
            }
            if (!aliveCanBind)
            {
                foreach (Component component in player.components)
                {
                    if (component is Ability && component.Name == bindName)
                    {
                        foreach (Component comp in player.components)
                        {
                            if (comp.key == keys.Last())
                            {
                                comp.key = component.key;
                            }
                        }
                        component.key = keys.Last();
                        Array.Clear(keys, 0, keys.Length);
                        bindName = "NoName";
                        aliveCanBind = true;
                        break;
                    }
                }
            }
        }

        public void UpdateItemUpgrade(MouseState mouse, Circle mouseCircle)
        {
            //only in buy phase
            foreach (GameObject go in Player.items)
            {
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    MouseOnIcon = true;

                    if (canBuy && mouse.RightButton == ButtonState.Pressed)
                    {
                        canBuy = false;
                        Player.gold += (go.GetComponent("Item") as Item).Value;
                        Player.items.Remove(go);
                        (player.GetComponent("Player") as Player).UpdateStats();
                        break;
                    }
                }
                else
                {
                    MouseOnIcon = false;
                }
            }
        }

        public void UpdateMouseOnIcon(Circle mouseCircle)
        {
            testList.Clear();
            foreach (GameObject go in abilityList)
            {
                testList.Add((go.GetComponent("Collider") as Collider));
            }
            foreach (GameObject go in itemList)
            {
                testList.Add((go.GetComponent("Collider") as Collider));
            }
            foreach (GameObject go in Player.abilities)
            {
                testList.Add((go.GetComponent("Collider") as Collider));
            }
            foreach (GameObject go in Player.items)
            {
                testList.Add((go.GetComponent("Collider") as Collider));
            }
            foreach (Collider col in testList)
            {
                if (col.gameObject.Tag == "AbilityIcon" && Player.abilities.Exists(x => x.Tag == "AbilityIcon"))
                {

                }
                if (mouseCircle.Intersects(col.CircleCollisionBox))
                {
                    MouseOnIcon = true;
                    break;
                }
                else MouseOnIcon = false;
            }
        }

        public void UpdateMouseRelease(MouseState mouse)
        {
            if (mouse.RightButton == ButtonState.Released)
            {
                canBuy = true;
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                canUpgrade = true;
            }
        }

        public void UpdateLists()
        {
            //map.Update();
            clientTimer += deltaTime;
            foreach (GameObject go in gameObjects)
            {
                go.Update();
                if (go.Tag == "Enemy")
                {
                    sendPos = true;
                }
                if (go.Tag == "Player" && client != null && sendPos)
                {
                    client.SendPositions(go.transform.position);
                    client.SendVelocity((go.GetComponent("Physics") as Physics).Velocity);
                    clientTimer = 0;
                }
            }
            foreach (GameObject go in abilityList)
            {
                go.Update();
            }
            foreach (GameObject go in itemList)
            {
                go.Update();
            }
            foreach (GameObject go in Player.items)
            {
                go.Update();
            }
        }

        public void PhaseCheck()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F6) && buyPhase && canReady)
            {
                canReady = false;
                if (client != null)
                {
                    if (player.isReady)
                    {
                        client.SendReady(player.Id, false);
                        player.isReady = false;

                    }
                    else
                    {
                        client.SendReady(player.Id, true);
                        player.isReady = true;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F6) && buyPhase)
            {
                canReady = true;
            }

            if (client != null && gameState == GameState.ingame && !buyPhase && !waitingForServerResponse)
            {
                if (client.isHost)
                {
                    playersAlive.Clear();
                    foreach (GameObject go in gameObjects)
                    {
                        if (go.Tag == "Player" || go.Tag == "Enemy")
                        {
                            playersAlive.Add(go);
                        }
                    }
                    if (playersAlive.Count < 2)
                    {
                        if (currentRound < numberOfRounds)
                        {
                            foreach (Component component in player.components)
                            {
                                if (component is Ability)
                                {
                                    (component as Ability).CanShoot = true;
                                    (component as Ability).CooldownTimer = 0;
                                }
                            }
                            //revive all players & reset all stats
                            //CreateDummies();
                            client.SendSwitchPhase();
                            //player.TotalScore += player.RoundScore;
                            //player.RoundScore = 0;
                            StartRound();
                            //CreateMap(selectedMap);
                            //ResetCharacters();
                            buyPhase = true;
                            currentRound++;
                        }
                        else
                        {
                            //show end screen
                            currentRound = 1;
                            waitingForServerResponse = true;
                            client.SendEnding();
                            //Thread.Sleep(50);
                            //NextScene = Scene.PostScreen();
                        }
                    }
                }
            }
        }

        public void UpdateLevel()
        {
            if (objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    if (go.Tag == "Player" || go.Tag == "Enemy")
                    {
                        go.transform.position = new Vector2(0, 0);
                        characters.Add(go);
                        characterColliders.Add((go.GetComponent("Collider") as Collider));
                    }
                    CircleColliders.Remove((go.GetComponent("Collider") as Collider));
                    gameObjects.Remove(go);

                    if (go.Tag == "Player")
                    {
                        Player player = (go.GetComponent("Player") as Player);
                        if (client != null && player.lastHitBy != "" && !buyPhase)
                        {
                            client.SendGold(player.lastHitBy, 25);
                            player.lastHitBy = "";
                        }
                    }
                }
                objectsToRemove.Clear();
            }

            if (newObjects.Count > 0)
            {
                foreach (GameObject obj in newObjects) { obj.LoadContent(Content); }
                gameObjects.AddRange(newObjects);
                newObjects.Clear();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.DarkRed);
            MouseState mouse = Mouse.GetState();
            Circle mouseCircle = new Circle(mouse.X, mouse.Y, 1);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (client != null)
            {
                client.Draw();
            }
            spriteBatch.DrawString(fontText, TestClient.text, new Vector2(0, Window.ClientBounds.Height / 2), Color.Black);


            foreach (GameObject go in gameObjects)
            {
                if (!go.IsInvisible)
                {
                    go.Draw(spriteBatch);
                }
                else
                {
                    if (go.Tag == "Player")
                    {
                        go.Draw(spriteBatch);
                    }
                }

            }




            DrawPlayerAbilities();

            if (buyPhase)
            {
                DrawVendorItems();

                DrawPlayerItems();

                DrawVendorAbilities();

                DrawTooltipVenderItem(mouse, mouseCircle);

                DrawTooltipVenderAbility(mouse, mouseCircle);

                DrawTooltipPlayerItem(mouse, mouseCircle);

                DrawTooltipPlayerAbility(mouse, mouseCircle);


            }
            if (CurrentScene.scenetype == "Practice")
            {
                if (player != null)
                {
                    if (player.CurrentHealth < 0)
                    {
                        //UpdateDeathAbilities();
                        DrawPlayerDeathAbilities();
                        DrawTooltipPlayerDeathAbility(mouse, mouseCircle);
                        UpdateAbilityRebind(mouse, mouseCircle);
                    }
                }
            }
            if (CurrentScene.scenetype == "Play" && player.CurrentHealth < 0)
            {
                //UpdateDeathAbilities();
                DrawPlayerDeathAbilities();
                DrawTooltipPlayerDeathAbility(mouse, mouseCircle);
                UpdateAbilityRebind(mouse, mouseCircle);
            }
            if (player != null)
            {
                foreach (Component comp in player.components)
                {
                    if (comp is Ability && !(comp is IDeathAbility) && player.CurrentHealth > 0)
                    {
                        (comp as Ability).Draw(spriteBatch);
                    }
                    if (comp is IDeathAbility && player.CurrentHealth < 0)
                    {
                        (comp as Ability).Draw(spriteBatch);
                    }
                }
            }

            if (CurrentScene.scenetype == "Play")
            {
                DrawScore(new Vector2(Window.ClientBounds.Width, 50));
            }
            else if (CurrentScene.scenetype == "PostScreen")
            {
                DrawScore(new Vector2(Window.ClientBounds.Width / 10 * 6, Window.ClientBounds.Height / 10 * 2));
            }
            else if (CurrentScene.scenetype == "Statistic")
            {
                DrawStatistic(new Vector2(Window.ClientBounds.Width / 10 * 5.7f, Window.ClientBounds.Height / 10 * 2));
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawVendorItems()
        {
            if (buyPhase)
            {
                int x = 10;
                int y = Window.ClientBounds.Height - 138;
                foreach (GameObject go in itemList)
                {
                    go.transform.position = new Vector2(x, y);
                    go.Draw(spriteBatch);
                    if (x == 112)
                    {
                        x = 10;
                        y += 34;
                    }
                    else x += 34;
                }
            }
        }

        public void DrawPlayerItems()
        {
            int x = 180;
            int y = Window.ClientBounds.Height - 138;
            foreach (GameObject go in Player.items)
            {
                go.transform.position = new Vector2(x, y);
                go.Draw(spriteBatch);
                if (x == 214)
                {
                    x = 180;
                    y += 34;
                }
                else x += 34;
            }
        }

        public void DrawVendorAbilities()
        {
            if (buyPhase)
            {
                foreach (GameObject go in abilityList)
                {
                    go.Draw(spriteBatch);
                }
            }
        }

        public void DrawPlayerAbilities()
        {
            foreach (GameObject go in Player.abilities)
            {
                go.Draw(spriteBatch);
                string name = (go.GetComponent("AbilityIcon") as AbilityIcon).Name;
                foreach (Component component in player.components)
                {
                    if (component is Ability && name == component.Name)
                    {
                        int x = 0;
                        string text = component.key.ToString();
                        text = text.Split('.').Last();
                        if (name == "Fireball")
                        {
                            text = "RMB";
                            x = -10;
                        }
                        if (text == "Space")
                        {
                            text = "Spc";
                            x = -10;
                        }
                        if (text == "D")
                        {
                            //do nothing
                        }
                        else if (text.Contains("D"))
                        {
                            text = text.Split('D').Last();
                        }
                        spriteBatch.DrawString(keyFont, text, new Vector2(go.transform.position.X + 10 + x, go.transform.position.Y + 16), Color.White, 0, Vector2.Zero, 1.2F, SpriteEffects.None, 1);
                    }
                }
            }

        }

        public void DrawPlayerDeathAbilities()
        {
            foreach (GameObject go in Player.deathAbilities)
            {
                go.Draw(spriteBatch);
                string name = (go.GetComponent("AbilityIcon") as AbilityIcon).Name;
                foreach (Component component in player.components)
                {
                    if (component is IDeathAbility && component.Name == name)
                    {
                        int x = 0;
                        string text = component.key.ToString();
                        text = text.Split('.').Last();
                        if (text == "Space")
                        {
                            text = "Spc";
                            x = -10;
                        }
                        if (text.Contains("D"))
                        {
                            text = text.Split('D').Last();
                        }
                        spriteBatch.DrawString(keyFont, text, new Vector2(go.transform.position.X + 10 + x, go.transform.position.Y + 16), Color.White, 0, Vector2.Zero, 1.5F, SpriteEffects.None, 1);
                    }
                }
            }
        }

        public void DrawTooltipVenderItem(MouseState mouse, Circle mouseCircle)
        {
            foreach (GameObject go in itemList)
            {
                Item item = (go.GetComponent("Item") as Item);
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    TooltipBox.transform.position = new Vector2(mouse.Position.X + 40, mouse.Position.Y - 60);
                    TooltipBox.Draw(spriteBatch);
                    //spriteBatch.DrawString(fontText, item.Name, new Vector2(mouse.Position.X + 50, mouse.Position.Y - 50), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    item.Draw(spriteBatch, mouse.Position.X, mouse.Position.Y);
                }
            }
        }

        public void DrawTooltipVenderAbility(MouseState mouse, Circle mouseCircle)
        {
            foreach (GameObject go in abilityList)
            {
                AbilityIcon icon = (go.GetComponent("AbilityIcon") as AbilityIcon);
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    int width = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Width;
                    int height = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Height;
                    TooltipBox.transform.position = new Vector2(mouse.Position.X - width, mouse.Position.Y - height);
                    TooltipBox.Draw(spriteBatch);
                    spriteBatch.DrawString(fontText, icon.Name, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(fontText, "Cost: " + icon.Value.ToString(), new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 20), Color.White, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(describtionFont, icon.Text, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 40), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    //icon.Draw(spriteBatch, mouse.Position.X, mouse.Position.Y);
                }
            }
        }

        public void DrawTooltipPlayerAbility(MouseState mouse, Circle mouseCircle)
        {
            //Collision detection currently failing
            foreach (GameObject go in Player.abilities)
            {
                AbilityIcon icon = (go.GetComponent("AbilityIcon") as AbilityIcon);
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    int width = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Width;
                    int height = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Height;
                    TooltipBox.transform.position = new Vector2(mouse.Position.X - width, mouse.Position.Y - height);
                    TooltipBox.Draw(spriteBatch);
                    spriteBatch.DrawString(fontText, icon.Name, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(describtionFont, "To Rebind key: \n Press and hold LMB while \n pressing the desired key", new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 20), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    //icon.Draw(spriteBatch, mouse.Position.X, mouse.Position.Y);
                }
            }
        }

        public void DrawTooltipPlayerDeathAbility(MouseState mouse, Circle mouseCircle)
        {

            foreach (GameObject go in Player.deathAbilities)
            {
                AbilityIcon icon = (go.GetComponent("AbilityIcon") as AbilityIcon);
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    int width = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Width;
                    int height = (TooltipBox.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Height;
                    TooltipBox.transform.position = new Vector2(mouse.Position.X - width, mouse.Position.Y - height);
                    TooltipBox.Draw(spriteBatch);
                    spriteBatch.DrawString(fontText, icon.Name, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5), Color.White, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    //spriteBatch.DrawString(fontText, "Value: " + icon.Value.ToString(), new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 20), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    //icon.Draw(spriteBatch, mouse.Position.X, mouse.Position.Y);
                }
            }
        }

        public void DrawTooltipPlayerItem(MouseState mouse, Circle mouseCircle)
        {
            foreach (GameObject go in Player.items)
            {
                Item item = (go.GetComponent("Item") as Item);
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    TooltipBox.transform.position = new Vector2(mouse.Position.X + 40, mouse.Position.Y - 60);
                    TooltipBox.Draw(spriteBatch);
                    //spriteBatch.DrawString(fontText, item.Name, new Vector2(mouse.Position.X + 50, mouse.Position.Y - 50), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    item.Draw(spriteBatch, mouse.Position.X, mouse.Position.Y);
                }
            }
        }

        public void DrawScore(Vector2 position)
        {
            string phase;
            string text;
            Vector2 textSize;
            int columnOne = 220;
            int columnTwo = 165;
            int columnThree = 120;
            int columnFour = 50;

            int x = 0;
            if (buyPhase)
            {
                phase = "Buy Phase";
            }
            else phase = "Combat Phase";
            if (CurrentScene.scenetype == "Play")
            {
                spriteBatch.DrawString(fontText, phase, new Vector2(position.X - columnTwo, position.Y - 40), Color.Black);
                spriteBatch.DrawString(fontText, "Round: " + currentRound + " / " + GameWorld.numberOfRounds, new Vector2(position.X - columnTwo, position.Y - 20), Color.Black);
            }

            text = "Player | Kills | Damage | Score";
            textSize = fontText.MeasureString(text);
            spriteBatch.DrawString(fontText, "Player | ", new Vector2(position.X - columnOne, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Kills | ", new Vector2(position.X - columnTwo, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Damage | ", new Vector2(position.X - columnThree, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Score", new Vector2(position.X - columnFour, position.Y), Color.Black);

            if (CurrentScene.scenetype == "PostScreen")
            {
                gameObjects = gameObjects.OrderByDescending(o => o.TotalScore).ToList();
                if (canUpdateStatistics)
                {
                    canUpdateStatistics = false;
                    if (player.Id == gameObjects[0].Id)
                    {
                        dbCon.i.AddBattleWonToSavedID();
                    }
                    else
                    {
                        dbCon.i.AddBattleNotWonToSavedID();
                    }
                }
            }
            List<GameObject> tempList = new List<GameObject>();
            foreach (GameObject go in gameObjects)
            {
                if (go.Tag == "Player" || go.Tag == "Enemy" || go.Tag == "Score")
                {
                    if (!tempList.Exists(i => i.Id == go.Id))
                    {
                        tempList.Add(go);
                    }
                }
            }
            foreach (GameObject go in tempList)
            {
                if (go.Tag == "Player" || go.Tag == "Enemy" || go.Tag == "Score")
                {

                    //tempList.Add(go);
                    text = go.playerName;
                    textSize = fontText.MeasureString(text);
                    Vector2 temp = fontText.MeasureString("Player");
                    if (textSize.X > temp.X)
                    {
                        text += "...";
                        textSize = fontText.MeasureString(text);
                        for (int i = text.Length; i >= 0; i--)
                        {
                            //text = text.Remove(text.Length - 2);
                            text = text.Remove(i - 3);
                            text += "...";
                            textSize = fontText.MeasureString(text);
                            if (textSize.X > temp.X)
                            {
                                continue;
                            }
                            else break;
                        }
                    }
                    spriteBatch.DrawString(fontText, text, new Vector2(position.X - columnOne, position.Y + 20 + x), Color.Black);
                    spriteBatch.DrawString(fontText, go.kills.ToString(), new Vector2(position.X - columnTwo, position.Y + 20 + x), Color.Black);
                    spriteBatch.DrawString(fontText, go.DamageDone.ToString(), new Vector2(position.X - columnThree, position.Y + 20 + x), Color.Black);
                    spriteBatch.DrawString(fontText, go.TotalScore.ToString(), new Vector2(position.X - columnFour, position.Y + 20 + x), Color.Black);
                    x += 20;

                }
            }
            tempList.Clear();
        }

        public void DrawStatistic(Vector2 position)
        {
            string text;
            Vector2 textSize;
            int columnOne = 220;
            int columnTwo = 165;
            int columnThree = 105;
            int columnFour = 5;

            Dictionary<string, int> info = dbCon.i.GetStats();
            float percent;
            if (info["battles"] == 0)
            {
                percent = 0;
            }
            else
            {
                percent = ((float)info["battleswon"] / (float)info["battles"]) * 100;
            }
            int tempura = (int)percent;
            string spercent;
            if (tempura >= 10)
            {
                spercent = percent.ToString().Truncate(5);
            }
            else
            {
                spercent = percent.ToString().Truncate(4);
            }
            spercent += "%";


            int x = 0;

            text = "Player | Games | Games Won | Win Percent";
            textSize = fontText.MeasureString(text);
            spriteBatch.DrawString(fontText, "Player | ", new Vector2(position.X - columnOne, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Games | ", new Vector2(position.X - columnTwo, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Games Won | ", new Vector2(position.X - columnThree, position.Y), Color.Black);
            spriteBatch.DrawString(fontText, "Win Percent", new Vector2(position.X - columnFour, position.Y), Color.Black);

            text = dbCon.i.GetName();

            textSize = fontText.MeasureString(text);
            Vector2 temp = fontText.MeasureString("Player");
            if (textSize.X > temp.X)
            {
                text += "...";
                textSize = fontText.MeasureString(text);
                for (int i = text.Length; i >= 0; i--)
                {
                    //text = text.Remove(text.Length - 2);
                    text = text.Remove(i - 3);
                    text += "...";
                    textSize = fontText.MeasureString(text);
                    if (textSize.X > temp.X)
                    {
                        continue;
                    }
                    else break;
                }
            }
            spriteBatch.DrawString(fontText, text, new Vector2(position.X - columnOne, position.Y + 20 + x), Color.Black);
            spriteBatch.DrawString(fontText, info["battles"].ToString(), new Vector2(position.X - columnTwo, position.Y + 20 + x), Color.Black);
            spriteBatch.DrawString(fontText, info["battleswon"].ToString(), new Vector2(position.X - columnThree, position.Y + 20 + x), Color.Black);
            spriteBatch.DrawString(fontText, spercent, new Vector2(position.X - columnFour, position.Y + 20 + x), Color.Black);

            x += 20;

        }

        public GameObject FindGameObjectWithTag(string tag)
        {
            return gameObjects.Find(x => x.Tag == tag);
        }
    }
}
