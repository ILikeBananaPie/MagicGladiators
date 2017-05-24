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
    public class SpriteRenderer : Component, ILoadable, IDrawable
    {
        public Rectangle Rectangle { get; set; }

        public float Scale { get; set; } = 1;

        public Texture2D Sprite { get; set; }

        public Vector2 Offset { get; set; }

        public Color Color { get; set; } = Color.White;

        private string spriteName;

        private float layerDepth;

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
        }

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth, Color color) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
            this.Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, gameObject.transform.position + Offset * Scale, Rectangle, Color, 0, Vector2.Zero, Scale, SpriteEffects.None, layerDepth);

        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(spriteName);
            this.Rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
        }
    }
}
