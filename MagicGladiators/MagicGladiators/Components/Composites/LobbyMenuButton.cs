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
    class LobbyMenuButton:Component, ILoadable, IUpdateable
    {
        private Animator animator;
        private string button;


        /// <summary>
        /// Button for the lobby
        /// </summary>
        /// <param name="go">Parent object</param>
        /// <param name="button">Case sensitive. Use: Standard, Pillar, Hole, PillarHole, 3, 5, 7</param>
        public LobbyMenuButton(GameObject go, string button) : base(go)
        {
            this.button = button;
        }

        public void LoadContent(ContentManager content)
        {
            this.animator = (Animator)gameObject.GetComponent("Animator");
            lastStates = Mouse.GetState();
            Texture2D sprite = content.Load<Texture2D>("LobbyMenuSheet");

            animator.CreateAnimation("StandardUnSelected", new Animation(1, 0, 0, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("StandardSelected", new Animation(1, 0, 1, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("PillarUnSelected", new Animation(1, 64, 0, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("PillarSelected", new Animation(1, 64, 1, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("HoleUnSelected", new Animation(1, 128, 0, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("HoleSelected", new Animation(1, 128, 1, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("PillarHoleUnSelected", new Animation(1, 192, 0, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("PillarHoleSelected", new Animation(1, 192, 1, 64, 64, 10, Vector2.Zero, sprite));

            animator.CreateAnimation("3UnSelected", new Animation(1, 0, 2, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("3Selected", new Animation(1, 0, 3, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("5UnSelected", new Animation(1, 64, 2, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("5Selected", new Animation(1, 64, 3, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("7UnSelected", new Animation(1, 128, 2, 64, 64, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("7Selected", new Animation(1, 128, 3, 64, 64, 10, Vector2.Zero, sprite));

            animator.PlayAnimation(button + "UnSelected");
        }

        private MouseState lastStates;
        public void Update()
        {
            MouseState m = Mouse.GetState();
            if (m.LeftButton != lastStates.LeftButton)
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (
                        m.X > gameObject.transform.position.X && m.X < gameObject.transform.position.X + 64 &&
                        m.Y > gameObject.transform.position.Y && m.Y < gameObject.transform.position.Y + 64
                        )
                    {
                        switch (button)
                        {
                            case "Standard":
                                GameWorld.selectedMap = "Map";
                                break;
                            case "Pillar":
                                GameWorld.selectedMap = "PillarMap";
                                break;
                            case "Hole":
                                GameWorld.selectedMap = "HoleMap";
                                break;
                            case "PillarHole":
                                GameWorld.selectedMap = "PillarHoleMap";
                                break;
                            case "3":
                                GameWorld.numberOfRounds = 3;
                                break;
                            case "5":
                                GameWorld.numberOfRounds = 5;
                                break;
                            case "7":
                                GameWorld.numberOfRounds = 7;
                                break;
                        }
                    }
                }
            }
            lastStates = m;

            switch (button)
            {
                case "Standard":
                    if (GameWorld.selectedMap == "Map")
                    {
                        animator.PlayAnimation("StandardSelected");
                    } else
                    {
                        animator.PlayAnimation("StandardUnSelected");
                    }
                    break;
                case "Pillar":
                    if (GameWorld.selectedMap == "PillarMap")
                    {
                        animator.PlayAnimation("PillarSelected");
                    } else
                    {
                        animator.PlayAnimation("PillarUnSelected");
                    }
                    break;
                case "Hole":
                    if (GameWorld.selectedMap == "HoleMap")
                    {
                        animator.PlayAnimation("HoleSelected");
                    } else
                    {
                        animator.PlayAnimation("HoleUnSelected");
                    }
                    break;
                case "PillarHole":
                    if (GameWorld.selectedMap == "PillarHoleMap")
                    {
                        animator.PlayAnimation("PillarHoleSelected");
                    } else
                    {
                        animator.PlayAnimation("PillarHoleUnSelected");
                    }
                    break;
                case "3":
                    if (GameWorld.numberOfRounds == 3)
                    {
                        animator.PlayAnimation("3Selected");
                    } else
                    {
                        animator.PlayAnimation("3UnSelected");
                    }
                    break;
                case "5":
                    if (GameWorld.numberOfRounds == 5)
                    {
                        animator.PlayAnimation("5Selected");
                    } else
                    {
                        animator.PlayAnimation("5UnSelected");
                    }
                    break;
                case "7":
                    if (GameWorld.numberOfRounds == 7)
                    {
                        animator.PlayAnimation("7Selected");
                    } else
                    {
                        animator.PlayAnimation("7UnSelected");
                    }
                    break;
            }
        }
    }
}
