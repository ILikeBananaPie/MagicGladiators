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
    public class Username:Component, IUpdateable, ILoadable, IDrawable
    {
        private string name;
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

        public Username(GameObject go) : base(go)
        {
            name = string.Empty;
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
                                name = name.Truncate(name.Length - 1);
                            }
                        }
                    } else
                    {
                        deleteTimer = 0;
                    }

                    if (!lastPressedKeys.Contains(key))
                    {
                        skip = 0;

                        if (pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift))
                        {
                            if (key == Keys.A || key == Keys.B || key == Keys.C || key == Keys.D || key == Keys.E || key == Keys.F || key == Keys.G || key == Keys.H || key == Keys.I || key == Keys.J || key == Keys.K || key == Keys.L || key == Keys.M || key == Keys.N || key == Keys.O || key == Keys.P || key == Keys.Q || key == Keys.R || key == Keys.S || key == Keys.T || key == Keys.U || key == Keys.V || key == Keys.W || key == Keys.X || key == Keys.Y || key == Keys.Z)
                            {
                                name += key.ToString().ToUpper();
                            }
                        } else
                        {
                            if (key == Keys.A || key == Keys.B || key == Keys.C || key == Keys.D || key == Keys.E || key == Keys.F || key == Keys.G || key == Keys.H || key == Keys.I || key == Keys.J || key == Keys.K || key == Keys.L || key == Keys.M || key == Keys.N || key == Keys.O || key == Keys.P || key == Keys.Q || key == Keys.R || key == Keys.S || key == Keys.T || key == Keys.U || key == Keys.V || key == Keys.W || key == Keys.X || key == Keys.Y || key == Keys.Z)
                            {
                                name += key.ToString().ToLower();
                            }
                            if (key == Keys.D0 || key == Keys.D1 || key == Keys.D2 || key == Keys.D3 || key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 || key == Keys.D8 || key == Keys.D9)
                            {
                                name += key.ToString()[1];
                            }
                            if (key == Keys.NumPad0 || key == Keys.NumPad1 || key == Keys.NumPad2 || key == Keys.NumPad3 || key == Keys.NumPad4 || key == Keys.NumPad5 || key == Keys.NumPad6 || key == Keys.NumPad7 || key == Keys.NumPad8 || key == Keys.NumPad9)
                            {
                                name += key.ToString()[6];
                            }
                            if (key == Keys.Back)
                            {
                                name = name.Truncate(name.Length - 1);
                            }
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
                    output = name + "|";
                } else
                {
                    output = name + " ";
                }
            } else
            {
                skip = 0;
                output = name;
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
            if (output.Length > 0)
            {
                spriteBatch.DrawString(spriteFont, output, pos, Color.Black);
            } else
            {
                spriteBatch.DrawString(spriteFont, "Insert Username", pos, Color.Gray);
            }
        }

        public string GetInput()
        {
            return name;
        }
    }
}
