using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    public class IPInput:Component, IUpdateable, ILoadable, IDrawable
    {
        private string ip;
        private Texture2D rect;
        private Keys[] lastPressedKeys;
        private SpriteFont spriteFont;
        private bool clicked;
        private Vector2 pos;
        private MouseState lastStates;

        public IPInput(GameObject go, Vector2 pos) : base(go)
        {
            this.pos = pos;
            ip = string.Empty;
            lastPressedKeys = Keyboard.GetState().GetPressedKeys();
            lastStates = Mouse.GetState();
        }

        public void Update()
        {
            MouseState m = Mouse.GetState();
            if (m.LeftButton != lastStates.LeftButton)
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (
                        m.X > rect.Bounds.X && m.X < rect.Bounds.X + rect.Bounds.Width &&
                        m.Y > rect.Bounds.Y && m.Y < rect.Bounds.Y + rect.Bounds.Height
                        )
                    {
                        clicked = true;
                    } else { clicked = false; }
                }
            }
            lastStates = m;

            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            //foreach (Keys key in lastPressedKeys)
            //{
            //    if (!pressedKeys.Contains(key))
            //        OnKeyUp(key);
            //}

            //check if the currently pressed keys were already pressed
            if (clicked)
            {
                foreach (Keys key in pressedKeys)
                {


                    if (!lastPressedKeys.Contains(key))
                    {
                        if (key.ToString().Contains("D") && key.ToString().Length > 1)
                        {
                            ip += key.ToString()[1];
                        } else if (key == Keys.OemPeriod)
                        {
                            if (pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift))
                            {
                                ip += ":";
                            } else
                            {
                                ip += ".";
                            }
                        } else if (key == Keys.Back)
                        {
                            ip = ip.Truncate(ip.Length - 1);
                        } else if (key == Keys.Enter)
                        {

                        }
                        //else
                        //{
                        //    ip += key;
                        //}
                    }
                }
            }
            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("fontText");
            rect = new Texture2D(GameWorld.Instance.GraphicsDevice, 15 * 21, 14);
            Color[] data = new Color[15*21*14];
            for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            rect.SetData(data);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(rect, pos, Color.White);
            spriteBatch.DrawString(spriteFont, ip, pos, Color.Black);
        }
    }
}
