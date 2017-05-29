using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace MagicGladiators
{
    class Item : Component, ILoadable
    {
        private Animator animator;
        private IStrategy strategy;
        private SpriteFont fontText;

        private int tooltipX;
        private int tooltipY;
        private bool showTooltip = false;
        private string[] list = new string[2] { "Stat", "Value" };

        public string Name { get; set; }
        public int Health { get; set; }
        public float Speed { get; set; }
        public float DamageResistance { get; set; }
        public float LavaResistance { get; set; }
        public float KnockBackResistance { get; set; }
        public float ProjectileSpeed { get; set; }
        public float LifeSteal { get; set; }
        public float CDR { get; set; }
        public float AOEBonus { get; set; }
        public int Value { get; set; }
        public int UpgradeValue { get; set; }
        public float GoldBonusPercent { get; set; }

        public int upgradeLevel { get; private set; } = 0;

        public Item(GameObject gameObject, string[] stats) : base(gameObject)
        {
            this.Name = stats[0];
            this.Health = int.Parse(stats[1]);
            this.Speed = float.Parse(stats[2]);
            this.DamageResistance = float.Parse(stats[3]);
            this.LavaResistance = float.Parse(stats[4]);
            this.Value = int.Parse(stats[5]);
            this.KnockBackResistance = float.Parse(stats[6]);
            this.ProjectileSpeed = float.Parse(stats[7]);
            this.LifeSteal = float.Parse(stats[8]);
            this.CDR = float.Parse(stats[9]);
            this.AOEBonus = float.Parse(stats[10]);
            this.GoldBonusPercent = float.Parse(stats[11]);
            this.UpgradeValue = (int)(Value + Value * 0.2F);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("ItemSheet2");
            fontText = content.Load<SpriteFont>("fontText");


            animator.CreateAnimation("Speed", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("LavaRes", new Animation(1, 0, 2, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("DmgRes", new Animation(1, 0, 3, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Hp", new Animation(1, 0, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("KnockRes", new Animation(1, 32, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("ProjectileSpeed", new Animation(1, 32, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("LifeSteal", new Animation(1, 32, 2, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("CDR", new Animation(1, 32, 3, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("AOE", new Animation(1, 64, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Gold", new Animation(1, 64, 1, 32, 32, 10, Vector2.Zero, sprite));


            animator.PlayAnimation(Name);
            strategy = new Idle(animator);
        }

        public void Upgrade()
        {
            Health += (int)(Health * 0.25F);
            Speed += Speed * 0.25F;
            DamageResistance += DamageResistance * 0.25F;
            LavaResistance += LavaResistance * 0.25F;
            Value += (int)(Value * 0.2F);
            KnockBackResistance += KnockBackResistance * 0.25F;
            ProjectileSpeed += ProjectileSpeed * 0.25F;
            LifeSteal += LifeSteal * 0.25F;
            CDR += CDR * 0.25F;
            AOEBonus += AOEBonus * 0.25F;
            UpgradeValue += (int)(UpgradeValue * 0.2F);
            GoldBonusPercent += (UpgradeValue * 0.2f);
            upgradeLevel++;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {

            int plus = 0;
            int index = 0;
            spriteBatch.DrawString(fontText, Name, new Vector2(x + 50, y - 50), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
            plus += 20;
            var fieldValues2 = this.GetType().GetProperties().Select(field => field.GetValue(this)).ToList();
            for (int i = 1; i < fieldValues2.Count - 3; i++)
            {
                object obj = fieldValues2[i];
                float testInt = float.Parse(obj.ToString());
                if (testInt != 0)
                {
                    if (testInt < 10)
                    {
                        spriteBatch.DrawString(fontText, list[index] + ": " + (testInt * 100).ToString(".") + "%", new Vector2(x + 50, y - 50 + plus), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                        plus += 20;
                        index++;
                    }
                    else
                    {
                        spriteBatch.DrawString(fontText, list[index] + " " + "+ " + testInt, new Vector2(x + 50, y - 50 + plus), Color.Black, 0, Vector2.Zero, 0.9F, SpriteEffects.None, 1);
                        plus += 20;
                        index++;

                    }
                }
            }
        }
    }
}
