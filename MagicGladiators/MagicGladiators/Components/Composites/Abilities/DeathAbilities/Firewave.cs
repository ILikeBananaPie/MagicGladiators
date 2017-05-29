using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MagicGladiators
{
    class Firewave : Ability
    {
        public bool activated { get; set; } = false;
        private string side;
        private Random rnd = new Random();

        public Firewave(GameObject gameObject) : base(gameObject)
        {
            cooldown = 1;
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.H) && canShoot)
            {
                canShoot = false;
                activated = true;
            }

            if (activated)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    activated = false;
                    Vector2 position = new Vector2(mouse.Position.X, mouse.Position.Y);
                    Vector2 target;
                    int closestSide = 0;
                    int width = GameWorld.Instance.Window.ClientBounds.Width;
                    int height = GameWorld.Instance.Window.ClientBounds.Height;

                    int distanceTop = (int)position.Y;
                    int distanceBottom = ((int)position.Y - height) * -1;
                    int distanceLeft = (int)position.X;
                    int distanceRight = ((int)position.X - width) * -1;

                    if (distanceBottom < distanceTop)
                    {
                        if (distanceLeft < distanceRight)
                        {
                            if (distanceLeft < distanceBottom)
                            {
                                //left side
                                int random = rnd.Next(height / 2 - 450, height / 2 + 300);
                                position = new Vector2(0, random);
                                target = new Vector2(width, random);
                                side = "FirewaveLeftRight";
                            }
                            else
                            {
                                //buttom side
                                int random = rnd.Next(width / 2 - 450, width / 2 + 300);
                                position = new Vector2(random, height);
                                target = new Vector2(random, 0);
                                side = "FirewaveTopBottom";
                            }
                        }
                        else
                        {
                            if (distanceRight < distanceBottom)
                            {
                                //right side
                                int random = rnd.Next(height / 2 - 450, height / 2 + 300);
                                position = new Vector2(width, random);
                                target = new Vector2(0, random);
                                side = "FirewaveLeftRight";
                            }
                            else
                            {
                                //buttom side
                                int random = rnd.Next(width / 2 - 450, width / 2 + 300);
                                position = new Vector2(random, height);
                                target = new Vector2(random, 0);
                                side = "FirewaveTopBottom";
                            }
                        }
                    }
                    else
                    {
                        if (distanceLeft < distanceRight)
                        {
                            if (distanceTop < distanceLeft)
                            {
                                //top side
                                int random = rnd.Next(width / 2 - 450, width / 2 + 300);
                                position = new Vector2(random, 0);
                                target = new Vector2(random, height);
                                side = "FirewaveTopBottom";
                            }
                            else
                            {
                                //left side
                                int random = rnd.Next(height / 2 - 450, height / 2 + 300);
                                position = new Vector2(0, random);
                                target = new Vector2(width, random);
                                side = "FirewaveLeftRight";
                            }
                        }
                        else
                        {
                            if (distanceRight < distanceTop)
                            {
                                //right side
                                int random = rnd.Next(height / 2 - 450, height / 2 + 300);
                                position = new Vector2(width, random);
                                target = new Vector2(0, random);
                                side = "FirewaveLeftRight";
                            }
                            else
                            {
                                //top side
                                int random = rnd.Next(width / 2 - 450, width / 2 + 300);
                                position = new Vector2(random, 0);
                                target = new Vector2(random, height);
                                side = "FirewaveTopBottom";
                            }
                        }
                    }
                    target = target - position;
                    target.Normalize();
                    Director director = new Director(new ProjectileBuilder());
                    GameWorld.newObjects.Add(director.ConstructProjectile(position, target, side, new GameObject()));

                    activated = false;
                    canShoot = true;
                }
            }
        }

        public bool intersects(Circle cir, Rectangle rec)
        {
            Vector2 circleDistance;
            float cornorDistance;
            circleDistance.X = Math.Abs(cir.Center.X - rec.X);
            circleDistance.Y = Math.Abs(cir.Center.Y - rec.Y);

            if (circleDistance.X > (rec.Width / 2 + cir.Radius)) return false;
            if (circleDistance.Y > (rec.Height / 2 + cir.Radius)) return false;

            if (circleDistance.X <= (rec.Width / 2)) return true;
            if (circleDistance.Y <= (rec.Height / 2)) return true;

            cornorDistance = (int)(circleDistance.X - rec.Width / 2) ^ 2 + (int)(circleDistance.Y - rec.Height / 2) ^ 2;

            return (cornorDistance <= ((int)cir.Radius ^ 2));
        }
    }
}
