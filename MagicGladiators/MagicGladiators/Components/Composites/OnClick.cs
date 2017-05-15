using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class OnClick:Component, IUpdateable, ILoadable
    {
        private string destination;
        private Rectangle rectangle;

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
                                GameWorld.Instance.NextScene = Scene.NewGame();
                                break;
                            case "MainMenu":
                                GameWorld.Instance.NextScene = Scene.MainMenu();
                                break;
                            case "Join":
                                GameWorld.Instance.NextScene = Scene.Join();
                                break;
                            case "Host":
                                GameWorld.Instance.NextScene = Scene.Host();
                                break;
                        }
                    }
                }
            }
            lastStates = m;
        }
    }
}
