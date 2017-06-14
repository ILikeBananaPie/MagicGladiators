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
    class DeathMine : Ability, IDeathAbility
    {
        
        private Vector2 originalPos;
        private float timer;

        private Transform transform;
        private Animator animator;

        private Physics physics;

        private IStrategy strategy;
        private bool activated = false;

        public DeathMine(GameObject gameObject, Vector2 position) : base(gameObject)
        {
           
           
            Name = "DeathMine";
            cooldown = 20;
        }



        public void OnCollisionEnter(Collider other)
        {

        }

      

       

        public override void Update()
        {
            if (GameWorld.Instance.player.CurrentHealth > 0) { return; }

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(key) && canShoot)
            {
                canShoot = false;
                Director director = new Director(new ProjectileBuilder());
                director.ConstructProjectile(new Vector2(mouse.Position.X, mouse.Position.Y), Vector2.Zero, "DeathMine", new GameObject(), gameObject.Id);
                if (GameWorld.Instance.client != null)
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Id == gameObject.Id && go.Tag == "DeathMine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                            GameWorld.Instance.client.SendRemoval("DeathMine", gameObject.Id);
                        }
                    }
                    GameWorld.Instance.client.SendProjectile("DeathMine,Create", new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                }
                else
                {
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        if (go.Tag == "DeathMine")
                        {
                            GameWorld.objectsToRemove.Add(go);
                        }
                    }
                }
            }
        }
    }
}
