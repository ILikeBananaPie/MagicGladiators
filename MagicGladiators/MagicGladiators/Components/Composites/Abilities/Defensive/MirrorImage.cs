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
        private List<GameObject> clones = new List<GameObject>();

        public MirrorImage(GameObject gameObject) : base(gameObject)
        {
            cooldown = 5;
            canShoot = true;
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();


            if (activated)
            {
                activationTime += GameWorld.Instance.deltaTime;
                if (activationTime > 2)
                {
                    activated = false;
                    activationTime = 0;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "Clone")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            
                        }
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.U) && canShoot)
            {
                canShoot = false;
                activated = true;
                GameObject clone = new GameObject();

                clone.AddComponent(new SpriteRenderer(gameObject, "Player", 1));

                clone.AddComponent(new Animator(gameObject));

                clone.AddComponent(new Player(gameObject, gameObject.transform));

                //clone.AddComponent(new Collider(gameObject, false));

                clone.AddComponent(new Physics(gameObject));
            }

            if (activated)
            {
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject go in clones)
            {
                go.Draw(spriteBatch);
            }
        }
    }
}
