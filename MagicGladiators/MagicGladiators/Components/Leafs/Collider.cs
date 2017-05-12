using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Collider : Component, IDrawable, ILoadable, IUpdateable
    {
        /// <summary>
        /// A reference to the boxcolliders spriterenderer
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// A reference to the colliders texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Inidcates if this collider will check for collisions
        /// </summary>
        public bool CheckCollisions { get; set; }
        public bool CheckCircleCollisions { get; set; }

        private HashSet<Collider> otherColliders = new HashSet<Collider>();

        /// <summary>
        /// The colliders collisionbox
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                    (
                        (int)(gameObject.transform.position.X + spriteRenderer.Offset.X),
                        (int)(gameObject.transform.position.Y + spriteRenderer.Offset.Y),
                        spriteRenderer.Rectangle.Width,
                        spriteRenderer.Rectangle.Height
                    );
            }
        }

        public Circle CircleCollisionBox
        {
            get
            {
                return new Circle
                    (
                        (int)(gameObject.transform.position.X + spriteRenderer.Rectangle.Width / 2),
                        (int)(gameObject.transform.position.Y + spriteRenderer.Rectangle.Height / 2),
                        spriteRenderer.Sprite.Width / 2
                    );
            }
        }

        public Collider(GameObject gameObject, bool newCollider) : base(gameObject)
        {
            CheckCircleCollisions = true;
            GameWorld.Instance.CircleColliders.Add(this);
            /*
            if (gameObject.Tag == "Dummy" || gameObject.Tag == "Ability")
            {
                CheckCircleCollisions = true;
                GameWorld.Instance.CircleColliders.Add(this);
                
                if (!newCollider)
                {
                    GameWorld.Instance.CircleColliders.Add(this);
                }
                else
                {
                    GameWorld.Instance.newCircleColliders.Add(this);
                }
                
            }
            else
            {
                CheckCollisions = true;

                if (!newCollider)
                {
                    GameWorld.Instance.Colliders.Add(this);
                }
                else
                {
                    GameWorld.Instance.newColliders.Add(this);
                }
            }
            */

        }

        public void LoadContent(ContentManager content)
        {
            spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            texture = content.Load<Texture2D>("CollisionTexture");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameObject.Tag != "Ball")
            {
                Rectangle topLine = new Rectangle(CollisionBox.X, CollisionBox.Y, CollisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(CollisionBox.X, CollisionBox.Y + CollisionBox.Height, CollisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(CollisionBox.X + CollisionBox.Width, CollisionBox.Y, 1, CollisionBox.Height);
                Rectangle leftLine = new Rectangle(CollisionBox.X, CollisionBox.Y, 1, CollisionBox.Height);

#if DEBUG
                spriteBatch.Draw(texture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(texture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(texture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(texture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
#endif
            }


        }

        public void Update()
        {
            CheckCollision();
        }

        private void CheckCollision()
        {
            /*
            if (CheckCollisions)
            {
                foreach (Collider other in GameWorld.Instance.Colliders)
                {
                    if (other != this)
                    {
                        if (CollisionBox.Intersects(other.CollisionBox))
                        {
                            gameObject.OnCollisionStay(other);

                            if (!otherColliders.Contains(other))
                            {
                                otherColliders.Add(other);
                                gameObject.OnCollisionEnter(other);
                            }

                        }
                        else if (otherColliders.Contains(other))
                        {
                            otherColliders.Remove(other);
                            gameObject.OnCollisionExit(other);
                        }
                    }
                }
            }
            */
            if (CheckCircleCollisions)
            {
                foreach (Collider other in GameWorld.Instance.CircleColliders)
                {
                    if (other != this)
                    {
                        if (CircleCollisionBox.Intersects(other.CircleCollisionBox))
                        {
                            gameObject.OnCollisionStay(other);
                            if (!otherColliders.Contains(other))
                            {
                                otherColliders.Add(other);
                                gameObject.OnCollisionEnter(other);
                            }
                        }
                        else if (otherColliders.Contains(other))
                        {
                            otherColliders.Remove(other);
                            gameObject.OnCollisionExit(other);
                        }
                    }
                }
            }
        }

        private void CheckCircleCollision()
        {
            if (true)
            {

            }
        }
    }
}
