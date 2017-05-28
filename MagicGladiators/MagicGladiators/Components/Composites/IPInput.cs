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
        private string output;
        private float skip;
        private Texture2D rect;
        private Keys[] lastPressedKeys;
        private SpriteFont spriteFont;
        private bool clicked;
        private Vector2 pos;
        private MouseState lastStates;

        //
        private bool quickDelete;
        private float deleteTimer;
        //

        public IPInput(GameObject go) : base(go)
        {
            ip = string.Empty;
            output = string.Empty;
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
                        m.X > pos.X && m.X < pos.X + rect.Bounds.Width &&
                        m.Y > pos.Y && m.Y < pos.Y + rect.Bounds.Height
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
                    if (lastPressedKeys.Contains(Keys.Back))
                    {
                        deleteTimer += GameWorld.Instance.deltaTime;
                        if (!quickDelete)
                        {
                            if (deleteTimer > 0.8f)
                            {
                                quickDelete = true;
                                deleteTimer = 0;
                            }
                        } else
                        {
                            if (deleteTimer > 0.15f)
                            {
                                ip = ip.Truncate(ip.Length - 1);
                            }
                        }
                    } else
                    {
                        deleteTimer = 0;
                    }

                    if (!lastPressedKeys.Contains(key))
                    {
                        skip = 0;
                        if ((key.ToString().Contains("D") || (key.ToString().Contains("NumPad"))) && key.ToString().Length > 1)
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
                skip += GameWorld.Instance.deltaTime;
                if (skip >= 2)
                {
                    skip = 0;
                }
                if (skip < 1)
                {
                    output = ip + "|";
                } else
                {
                    output = ip;
                }
            } else
            {
                skip = 0;
                output = ip;
            }
            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;


        }

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("fontText");
            rect = new Texture2D(GameWorld.Instance.GraphicsDevice, 15 * 21, 16);
            Color[] data = new Color[15*21*16];
            for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            rect.SetData(data);
            this.pos = gameObject.transform.position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(rect, pos, Color.White);
            spriteBatch.DrawString(spriteFont, output, pos, Color.Black);
        }

        public string GetIPString()
        {
            return ip;
        }
    }
}
