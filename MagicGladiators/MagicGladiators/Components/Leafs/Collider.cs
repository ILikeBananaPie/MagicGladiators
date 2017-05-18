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

        public float Scale { get; set; } = 1;

        /// <summary>
        /// Inidcates if this collider will check for collisions
        /// </summary>
        public bool CheckCollisions { get; set; }
        public bool CheckCircleCollisions { get; set; }

        private Circle circleCollisionBox;
        private Rectangle collisionBox;

        private HashSet<Collider> otherColliders = new HashSet<Collider>();

        /// <summary>
        /// The colliders collisionbox
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return collisionBox;
            }
        }

        public Circle CircleCollisionBox
        {
            get
            {
                return circleCollisionBox;
            }
            set
            {
                circleCollisionBox = value;
            }
        }

        public Collider(GameObject gameObject, bool newCollider) : base(gameObject)
        {
            CheckCircleCollisions = true;
            LoadContent(GameWorld.Instance.Content);
            circleCollisionBox = new Circle
                    (
                        (int)(gameObject.transform.position.X + spriteRenderer.Rectangle.Width / 2),
                        (int)(gameObject.transform.position.Y + spriteRenderer.Rectangle.Height / 2),
                        spriteRenderer.Rectangle.Width / 2
                    );

            collisionBox = new Rectangle
                (
                    (int)(gameObject.transform.position.X + spriteRenderer.Offset.X),
                    (int)(gameObject.transform.position.Y + spriteRenderer.Offset.Y),
                    spriteRenderer.Rectangle.Width,
                    spriteRenderer.Rectangle.Height
                );

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
            }
        }

        public void Update()
        {
            if (gameObject.Tag == "Map")
            {

            }
            if (gameObject.Tag == "Icon")
            {

            }
            circleCollisionBox = new Circle
        (
            (int)((gameObject.transform.position.X + (spriteRenderer.Rectangle.Width * Scale) / 2)),
            (int)((gameObject.transform.position.Y + (spriteRenderer.Rectangle.Height * Scale) / 2)),
            spriteRenderer.Rectangle.Width * Scale / 2
        );

            collisionBox = new Rectangle
        (
            (int)((gameObject.transform.position.X + spriteRenderer.Offset.X)),
            (int)((gameObject.transform.position.Y + spriteRenderer.Offset.Y)),
            (int)(spriteRenderer.Rectangle.Width * Scale),
            (int)(spriteRenderer.Rectangle.Height * Scale)
        );

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
                        if (circleCollisionBox.Intersects(other.circleCollisionBox))
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
