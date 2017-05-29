using MagicGladiators.Components.Composites;
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

namespace MagicGladiators
{

    public enum ObjectType { }





    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int abilityIndex = 0;
        private List<IAbility> abilityListTest = new List<IAbility>();

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

        public bool MouseOnIcon { get; set; } = false;

        private int buySpellX;
        private int buySpellY;

        private SpriteFont fontText;
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

            CurrentScene = Scene.MainMenu();
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

        }

        public void CreateDummies()
        {
            Director director = new Director(new DummyBuilder());
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 - 280, mapCenter.Y - 16)));
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16 + 280, mapCenter.Y - 16)));
            newObjects.Add(director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 16 + 280)));
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
                if (go.Tag == "Player" || go.Tag == "Dummy")
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
                    go.transform.position = new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16);
                }
                if (go.Tag == "Dummy")
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
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "HomingMissile", 100, "Fires a projectile in the target \n direction, moving towards the \n closest enemy."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Charge", 100, "Sends you in the direction of the \n mouse. Exploding on contact."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Drain", 100, "Fires a slow moving projectile \n towards your mouse."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Deflect", 100, "Creates a shield around you, \n deflecting any spells coming \n your way."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Mine", 100, "Places a static mine at the \n target position. Will explode \n on contact."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "SpeedBoost", 100, "Increases your movement speed"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Chain", 100, "Fires a slow moving projectile, \n that pulls you and the target \n together for a period of time."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Blink", 100, "Instantly moves your character \n towards your mouse's position."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Nova", 100, "Sends out 8 straight flying \n projectiles in different directions."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Spellshield", 100, "Creates a shield around you, \n deleting any spells coming \n your way."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "StoneArmour", 100, "Grants reduced knockback \n effect for a period of time, \n while reducing movement speed."));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Boomerang", 100, "Fires a projectile that return to \n your position"));
            buySpellPosition();
            abilityList.Add(director.ConstructIcon(new Vector2(buySpellX, buySpellY), "Recall", 100, "teleport you to the position, you \n had when casting the spell"));

        }

        public void CreateVendorItems()
        {
            // name, hp, speed, dmgRes, lavaRes, value, knockRes, projectileSpeed, LifeSteal
            Director director = new Director(new ItemBuilder());
            string[] testItem = new string[] { "Speed", "0", "1", "0", "0", "100", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "Hp", "10", "0", "0", "0", "100", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "LavaRes", "0", "0", "0", "-0.01", "100", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "DmgRes", "0", "0", "-0.01", "0", "100", "0", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "KnockRes", "0", "0", "0", "0", "100", "0.01", "0", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "ProjectileSpeed", "0", "0", "0", "0", "100", "0", "0.01", "0", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "LifeSteal", "0", "0", "0", "0", "100", "0", "0", "0.01", "0", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "CDR", "0", "0", "0", "0", "100", "0", "0", "0", "0.05", "0" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
            testItem = new string[] { "AOE", "0", "0", "0", "0", "100", "0", "0", "0", "0", "0.1" };
            itemList.Add(director.ConstructItem(new Vector2(50, 50), testItem));
        }

        public void CreateMap(string map)
        {
            Director director = new Director(new MapBuilder());
            Texture2D sprite = Content.Load<Texture2D>("StandardMap");
            newObjects.Add(director.ConstructMapPart(new Vector2(Window.ClientBounds.Width / 2 - sprite.Width / 2, Window.ClientBounds.Height / 2 - sprite.Height / 2), "Map"));

            foreach (GameObject go in newObjects)
            {
                if (go.Tag == "Map")
                {
                    mapCenter = new Vector2(go.transform.position.X + sprite.Width / 2, go.transform.position.Y + sprite.Height / 2);
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontText = Content.Load<SpriteFont>("fontText");
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

            Rectangle rec = new Rectangle(10, 10, 10, 10);
            Circle circle = new Circle(10, 10, 10);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!pressed)
                {
                    if (CurrentScene.scenetype == "Practice" || CurrentScene.scenetype == "NewGame")
                    {
                        NextScene = Scene.MainMenu();
                    }
                    else if (CurrentScene.scenetype == "PracticeChooseRound" || CurrentScene.scenetype == "Host")
                    {
                        NextScene = Scene.NewGame();
                    }
                    else
                    {
                        if (server != null)
                        {
                            server.Kill();
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
            MouseState mouse = Mouse.GetState();
            Circle mouseCircle = new Circle(mouse.X, mouse.Y, 1);
            if (CurrentScene.scenetype == "Practice")
            {
                foreach (GameObject go in gameObjects)
                {
                    string name = go.Tag;
                    if (go.CurrentHealth < 0)
                    {
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

            UpdateDeathAbilities();

            if (buyPhase)
            {
                UpdateBuyItem(mouse, mouseCircle);

                UpdateBuyAbility(mouse, mouseCircle);

                UpdateAbilityUpgrade(mouse, mouseCircle);

                UpdateItemUpgrade(mouse, mouseCircle);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2) && canServer)
            {
                //server = new TestServer();

                canServer = false;
                showServer = true;
                server = new Process();
                server.StartInfo.FileName = "TestServer.exe";
                server.EnableRaisingEvents = true;
                server.Start();
                //GameWorld.Instance.client = new TestClient("localhost");
                //Thread update = new Thread(ServerUpdate);
                //update.IsBackground = true;
                //update.Start();
                //threads.Add(update);
            }
            if (!canServer)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F6))
                {
                    //server.SendMessage("Server sending text!");
                }

                //server.Update();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3) && canClient)
            {
                client = new TestClient("localhost");
                canClient = false;
                showServer = true;
                client.ConnectToServer();

                //Thread update = new Thread(ClientUpdate);
                //update.IsBackground = true;
                //update.Start();
                //threads.Add(update);

                //Thread draw = new Thread(ClientDraw);
                //draw.IsBackground = true;
                //draw.Start();
                //threads.Add(draw);
            }
            if (!canClient)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F4))
                {
                    client.SendMessage("Client sending text!");
                }
                if (client != null)
                {
                    client.Update();
                }
                //client.Draw();
            }

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
                if (NextScene.scenetype == "Practice")
                {
                    CreateMap(selectedMap);

                    Director director = new Director(new PlayerBuilder());
                    player = director.Construct(new Vector2(mapCenter.X - 16, mapCenter.Y - 280 - 16));
                    Director ability = new Director(new AbilityIconBuilder());
                    Player.abilities.Add(ability.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68, Window.ClientBounds.Height - 42), "Fireball", 0, ""));
                    newObjects.Add(player);

                    CreateDummies();

                    CreateVendorItems();

                    CreateVendorAbilities();

                    foreach (GameObject go in gameObjects)
                    {
                        go.LoadContent(Content);
                    }
                }
                if (NextScene.scenetype == "Host")
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
            if (CurrentScene.scenetype == "Practice")
            {
                if (player.CurrentHealth <= 0)
                {
                    (player.GetComponent("DeathMine") as DeathMine).Update();
                    (player.GetComponent("RollingMeteor") as RollingMeteor).Update();
                    (player.GetComponent("ShrinkMap") as ShrinkMap).Update();
                    (player.GetComponent("SlowField") as SlowField).Update();
                }
            }
        }

        public void ResetItemsAndAbilities()
        {
            abilityList.Clear();
            buyPhase = true;
            currentRound = 1;
            itemList.Clear();
            numberOfRounds = 0;
            playersAlive.Clear();
            readyList.Clear();
            buySpellX = Window.ClientBounds.Width - 144;
            buySpellY = Window.ClientBounds.Height - 144;
            Player.items.Clear();
            Player.abilities.Clear();
            Player.gold = 10000;
        }

        public void UpdateBuyItem(MouseState mouse, Circle mouseCircle)
        {
            //only in buy phase
            if (buyPhase)
            {
                foreach (GameObject go in itemList)
                {
                    Item item = (go.GetComponent("Item") as Item);
                    if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                    {
                        if (canBuy && mouse.RightButton == ButtonState.Pressed && Player.items.Count <= 5)
                        {
                            canBuy = false;
                            if (Player.gold >= item.Value)
                            {
                                Director director = new Director(new ItemBuilder());
                                Player.items.Add(director.ConstructItem(new Vector2(0, 200), new string[] { item.Name, item.Health.ToString(), item.Speed.ToString(), item.DamageResistance.ToString(), item.LavaResistance.ToString(), (item.Value / 2).ToString(), item.KnockBackResistance.ToString(), item.ProjectileSpeed.ToString(), item.LifeSteal.ToString(), item.CDR.ToString(), item.AOEBonus.ToString() }));
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
                        if (canBuy && mouse.RightButton == ButtonState.Pressed && Player.abilities.Count <= 7)
                        {
                            AbilityIcon ability = (go.GetComponent("AbilityIcon") as AbilityIcon);
                            canBuy = false;

                            Director director = new Director(new AbilityIconBuilder());
                            int x = Player.abilities.Count * 34;
                            Player.abilities.Add(director.ConstructIcon(new Vector2(Window.ClientBounds.Width / 2 - 68 + x, Window.ClientBounds.Height - 42), ability.Name, ability.Value, ability.Text));
                            (Player.abilities[Player.abilities.Count - 1].GetComponent("AbilityIcon") as AbilityIcon).index = abilityIndex;
                            abilityIndex++;
                            Player.gold -= ability.Value;

                            CreateAbility ca = new CreateAbility(ability.Name);
                            player.AddComponent(ca.GetComponent(player, player.transform.position));
                            abilityList.Remove(ability.gameObject);
                            break;
                        }
                    }
                }
            }
        }

        public void UpdateAbilityUpgrade(MouseState mouse, Circle mouseCircle)
        {
            foreach (GameObject go in Player.abilities)
            {
                if (mouseCircle.Intersects((go.GetComponent("Collider") as Collider).CircleCollisionBox))
                {
                    if (canBuy && mouse.LeftButton == ButtonState.Pressed)
                    {
                        //rebind ability
                    }
                    if (canBuy && mouse.RightButton == ButtonState.Pressed)
                    {
                        //upgrade ability
                        //giving ability icons ability components. Give each ability component an isBought bool and only allow Update to run if isBought is true. Set isBought to true when buying the ability.
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
                    if (canUpgrade && mouse.LeftButton == ButtonState.Pressed)
                    {
                        //upgrade item
                        canUpgrade = false;
                        Item item = (go.GetComponent("Item") as Item);
                        if (item.upgradeLevel != 3 && Player.gold >= item.UpgradeValue)
                        {
                            Player.gold -= item.UpgradeValue;
                            item.Upgrade();
                            (player.GetComponent("Player") as Player).UpdateStats();
                        }
                        else
                        {
                            //error message (not enough gold)
                        }
                    }
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
            if (!buyPhase)
            {
                if (playersAlive.Count < 2)
                {
                    if (currentRound < numberOfRounds)
                    {
                        //revive all players & reset all stats
                        //CreateDummies();
                        StartRound();
                        buyPhase = true;
                        currentRound++;
                    }
                    else
                    {
                        //show end screen
                        currentRound = 1;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F6) && buyPhase && canBuy)
            {
                canBuy = false;
                readyList.Add(true);
            }
            foreach (bool b in readyList)
            {
                if (!b) break;
                else
                {
                    //reset positions and stats
                    StartRound();
                    buyPhase = false;
                }
            }
        }

        public void UpdateLevel()
        {
            if (objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    if (go.Tag == "Player" || go.Tag == "Dummy")
                    {
                        characters.Add(go);
                        characterColliders.Add((go.GetComponent("Collider") as Collider));
                    }
                    CircleColliders.Remove((go.GetComponent("Collider") as Collider));
                    gameObjects.Remove(go);
                }
                objectsToRemove.Clear();
            }

            if (newObjects.Count > 0)
            {
                foreach (GameObject obj in newObjects) { obj.LoadContent(Content); }
                gameObjects.AddRange(newObjects);
                newObjects.Clear();
            }
            if (!buyPhase)
            {
                readyList.Clear();
                playersAlive.Clear();
                foreach (GameObject go in gameObjects)
                {
                    if (go.Tag == "Player" || go.Tag == "Dummy" || go.Tag == "Enemy")
                    {
                        playersAlive.Add(go);
                    }
                }
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
            }

            DrawTooltipPlayerAbility(mouse, mouseCircle);

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
                    spriteBatch.DrawString(fontText, icon.Name, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(fontText, "Value: " + icon.Value.ToString(), new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 20), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(describtionFont, icon.Text, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5 + 40), Color.Black, 0, Vector2.Zero, 1F, SpriteEffects.None, 1);
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
                    spriteBatch.DrawString(fontText, icon.Name, new Vector2(mouse.Position.X - width + 5, mouse.Position.Y - height + 5), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
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


        public GameObject FindGameObjectWithTag(string tag)
        {
            return gameObjects.Find(x => x.Tag == tag);
        }
    }

}
