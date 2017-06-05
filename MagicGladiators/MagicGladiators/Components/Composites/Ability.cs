using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicGladiators
{
    public enum actions { Fireball, HomingMissile, Drain, Chain }

    public abstract class Ability : Component, ILoadable, IUpdateable, IAbility, IDrawable
    {
        protected float cooldown;
        protected bool canShoot = true;
        protected float cooldownTimer;
        public float damage { get; set; }
        private bool canUse;
        public GameObject icon { get; set; }
        //private SpriteBatch spriteBatch;
        private SpriteFont font;


        public Ability(GameObject gameObject) : base(gameObject)
        {
            //spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);
            font = GameWorld.Instance.Content.Load<SpriteFont>("fontText");

        }

        public abstract void LoadContent(ContentManager content);




        public abstract void Update();

        public void Cooldown()
        {
            //Draw();
            if (!canShoot)
            {
                cooldownTimer += GameWorld.Instance.deltaTime;
                if (icon != null)
                {
                    (icon.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.DarkRed;
                }
            }
            else if (icon != null)
            {
                (icon.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
            }

            if (cooldownTimer > cooldown * gameObject.CooldownReduction)
            {
                cooldownTimer = 0;
                canShoot = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = 0;
            //spriteBatch.Begin();
            if (!canShoot && !(this is IDeathAbility) && GameWorld.gameState == GameState.ingame)
            {
                if (cooldown * GameWorld.Instance.player.CooldownReduction - cooldownTimer > 10)
                {
                    x = -5;
                }
                spriteBatch.DrawString(font, (cooldown * GameWorld.Instance.player.CooldownReduction - cooldownTimer).ToString(".0"), new Vector2(icon.transform.position.X + 5 + x, icon.transform.position.Y), Color.White);
            }
            if (!canShoot && this is IDeathAbility && GameWorld.gameState == GameState.ingame)
            {
                if (cooldown - cooldownTimer > 10)
                {
                    x = -5;
                }
                spriteBatch.DrawString(font, (cooldown - cooldownTimer).ToString(".0"), new Vector2(icon.transform.position.X + 5 + x, icon.transform.position.Y), Color.White);
            }
            //spriteBatch.End();
        }
    }
}
