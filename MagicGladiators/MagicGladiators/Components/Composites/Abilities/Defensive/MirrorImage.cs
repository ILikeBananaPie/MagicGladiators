using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators
{
    class MirrorImage : Ability, IDrawable
    {
        private float timer;
        private bool activated = false;
        private float activationTime;
        private string[] directions = new string[4] { "1", "2", "3", "4" };
        private List<int> numbers = new List<int>() { 1, 2, 3, 4 };
        private Random rnd = new Random();

        public MirrorImage(GameObject gameObject) : base(gameObject)
        {
            cooldown = 20;
            canShoot = true;
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                activated = true;
                Color color = (GameWorld.Instance.player.GetComponent("SpriteRenderer") as SpriteRenderer).Color;
                (GameWorld.Instance.player.GetComponent("SpriteRenderer") as SpriteRenderer).Color = new Color(color, 0.0001F);

                int random = rnd.Next(numbers.Count);
                GameWorld.Instance.player.cloneNumber = numbers[random];
                numbers.Remove(numbers[random]);

                for (int i = 0; i < 3; i++)
                {
                    GameObject clone = new GameObject();
                    clone.AddComponent(new SpriteRenderer(clone, "PlayerSheet", 1));
                    clone.AddComponent(new Animator(clone));
                    clone.AddComponent(new Clone(clone));
                    clone.AddComponent(new Collider(gameObject, true, true));
                    clone.AddComponent(new Physics(clone));
                    clone.Id = gameObject.Id;
                    int number = rnd.Next(numbers.Count);
                    clone.Tag = "Clone" + numbers[number];
                    clone.cloneNumber = numbers[number];
                    numbers.Remove(numbers[number]);
                    clone.CurrentHealth = gameObject.CurrentHealth;
                    clone.MaxHealth = gameObject.MaxHealth;
                    clone.LoadContent(GameWorld.Instance.Content);
                    (clone.GetComponent("Animator") as Animator).PlayAnimation((gameObject.GetComponent("Animator") as Animator).AnimationName);

                    #region clone positions
                    GameObject go = GameWorld.Instance.player;
                    if (go.cloneNumber == 1 && clone.cloneNumber == 2)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    }
                    if (go.cloneNumber == 1 && clone.cloneNumber == 3)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y + 64);
                    }
                    if (go.cloneNumber == 1 && clone.cloneNumber == 4)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y + 64);
                    }

                    if (go.cloneNumber == 2 && clone.cloneNumber == 1)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    }
                    if (go.cloneNumber == 2 && clone.cloneNumber == 3)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y + 64);
                    }
                    if (go.cloneNumber == 2 && clone.cloneNumber == 4)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y + 64);
                    }

                    if (go.cloneNumber == 3 && clone.cloneNumber == 1)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 3 && clone.cloneNumber == 2)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 3 && clone.cloneNumber == 4)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    }

                    if (go.cloneNumber == 4 && clone.cloneNumber == 1)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 4 && clone.cloneNumber == 2)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    }
                    if (go.cloneNumber == 4 && clone.cloneNumber == 3)
                    {
                        clone.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    }
                    #endregion
                    //clone.transform.position = new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y);

                    GameWorld.newObjects.Add(clone);
                }
                numbers.Clear();
                numbers = new List<int>() { 1, 2, 3, 4 };

                int test = rnd.Next(numbers.Count);
                test = numbers[test];
                foreach (GameObject go in GameWorld.newObjects)
                {
                    if (go.cloneNumber == test)
                    {
                        Vector2 tempVector = go.transform.position;
                        int tempNumber = go.cloneNumber;

                        go.transform.position = GameWorld.Instance.player.transform.position;
                        go.cloneNumber = GameWorld.Instance.player.cloneNumber;
                        go.Tag = "Clone" + go.cloneNumber;

                        GameWorld.Instance.player.transform.position = tempVector;
                        GameWorld.Instance.player.cloneNumber = tempNumber;

                        break;
                    }
                }
                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendClone(gameObject.Id, gameObject.transform.position, GameWorld.Instance.player.cloneNumber);
                }
            }

            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 5)
                {
                    activated = false;
                    activationTime = 0;
                    Color color = (GameWorld.Instance.player.GetComponent("SpriteRenderer") as SpriteRenderer).Color;
                    (GameWorld.Instance.player.GetComponent("SpriteRenderer") as SpriteRenderer).Color = new Color(color, 1F);
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag.Contains("Clone") && go.Id == gameObject.Id)
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag.Contains("Clone") && !go.Tag.Contains("Fireball") && go.Id == gameObject.Id)
                        {
                            int test = go.cloneNumber;
                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.SendRemoval(go.Tag, go.Id);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
