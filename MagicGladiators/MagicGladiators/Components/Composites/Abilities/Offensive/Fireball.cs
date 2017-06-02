using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Fireball : OffensiveAbility
    {
        //public GameObject icon { get; set; }

        public Fireball(GameObject go) : base(go)
        {
            canShoot = true;
            cooldown = 3;
            Name = "Fireball";
        }

        public override void LoadContent(ContentManager content)
        {

        }

        public override void Update()
        {
            if (GameWorld.gameState == GameState.offgame) { return; }
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (mouse.Position.X > 0 && mouse.Position.X < GameWorld.Instance.Window.ClientBounds.Width && mouse.Y > 0 && mouse.Y < GameWorld.Instance.Window.ClientBounds.Height)
            {
                if (mouse.LeftButton == ButtonState.Pressed && canShoot && !GameWorld.Instance.MouseOnIcon)
                {
                    Director director = new Director(new ProjectileBuilder());
                    director.ConstructProjectile(new Vector2(gameObject.transform.position.X, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y), "Fireball", gameObject, gameObject.Id);
                    if (GameWorld.Instance.client != null)
                    {
                        GameWorld.Instance.client.SendProjectile("Fireball,Create", new Vector2(gameObject.transform.position.X + 1, gameObject.transform.position.Y), new Vector2(mouse.Position.X, mouse.Position.Y));
                    }
                    canShoot = false;
                    foreach (GameObject go in GameWorld.gameObjects)
                    {
                        Vector2 mousePos = new Vector2(mouse.Position.X, mouse.Position.Y);
                        if (go.Tag.Contains("Clone") && go.Id == gameObject.Id)
                        {
                            if (go.Tag == "CloneLeft")
                            {
                                mousePos = new Vector2(mouse.Position.X - 64, mouse.Position.Y);
                            }
                            if (go.Tag == "CloneRight")
                            {
                                mousePos = new Vector2(mouse.Position.X + 64, mouse.Position.Y);
                            }
                            if (go.Tag == "CloneUp")
                            {
                                mousePos = new Vector2(mouse.Position.X, mouse.Position.Y - 64);
                            }
                            if (go.Tag == "CloneDown")
                            {
                                mousePos = new Vector2(mouse.Position.X, mouse.Position.Y + 64);
                            }
                            director.ConstructProjectile(new Vector2(go.transform.position.X, go.transform.position.Y), mousePos, "Fireball" + go.Tag, go, gameObject.Id);
                            if (GameWorld.Instance.client != null)
                            {
                                GameWorld.Instance.client.SendProjectile("Fireball" + go.Tag + ",Create", new Vector2(go.transform.position.X, go.transform.position.Y), mousePos);
                            }
                        }
                    }
                }
            }
            //if (!canShoot)
            //{
            //    (icon.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Gray;
            //}
            //else (icon.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
        }
    }
}
