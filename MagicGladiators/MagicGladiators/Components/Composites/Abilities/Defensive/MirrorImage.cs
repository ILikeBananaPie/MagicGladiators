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
                for (int i = 0; i < 3; i++)
                {
                    GameObject clone = new GameObject();
                    clone.AddComponent(new SpriteRenderer(clone, "PlayerSheet", 1));
                    clone.AddComponent(new Animator(clone));
                    clone.AddComponent(new Clone(clone));
                    clone.AddComponent(new Collider(gameObject, true, true));
                    clone.AddComponent(new Physics(clone));
                    clone.Tag = "Clone" + directions[i];
                    clone.Id = gameObject.Id;
                    int number = rnd.Next(numbers.Count);
                    clone.cloneNumber = numbers[number];
                    numbers.Remove(number);
                    clone.CurrentHealth = gameObject.CurrentHealth;
                    clone.MaxHealth = gameObject.MaxHealth;
                    clone.LoadContent(GameWorld.Instance.Content);
                    (clone.GetComponent("Animator") as Animator).PlayAnimation((gameObject.GetComponent("Animator") as Animator).AnimationName);
                    clone.transform.position = new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y);
                    GameWorld.newObjects.Add(clone);
                }
                GameWorld.Instance.player.cloneNumber = numbers[0];
                numbers.Clear();
                numbers = new List<int>() { 1, 2, 3, 4 };

                if (GameWorld.Instance.client != null)
                {
                    GameWorld.Instance.client.SendClone(gameObject.Id, gameObject.transform.position);
                }

            }

            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 5)
                {
                    activated = false;
                    activationTime = 0;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag.Contains("Clone"))
                        {
                            GameWorld.objectsToRemove.Add(go);
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
