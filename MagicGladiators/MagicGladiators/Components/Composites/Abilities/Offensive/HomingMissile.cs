using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class HomingMissile : OffensiveAbility, IDrawable, IAbility
    {
        private SpriteFont fontText;
        private bool testCast;

        public HomingMissile(GameObject go) : base(go)
        {
            fontText = GameWorld.Instance.Content.Load<SpriteFont>("fontText");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(fontText, "Ability learned", new Vector2(0, 120), Color.Black);
            if (testCast)
            {
                spriteBatch.DrawString(fontText, "Casting ability!", new Vector2(0, 140), Color.Black);
            }
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space))
            {
                testCast = true;
            }
            if (keyState.IsKeyUp(Keys.Space))
            {
                testCast = false;
            }
        }
    }
}
