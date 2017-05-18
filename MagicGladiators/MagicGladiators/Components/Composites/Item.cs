using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace MagicGladiators
{
    class Item : Component, ILoadable
    {
        private Animator animator;
        private IStrategy strategy;

        public string Name { get; set; }
        public int Health { get; set; }
        public float Speed { get; set; }
        public float DamageResistance { get; set; }
        public float LavaResistance { get; set; }
        public int Value { get; set; }

        public Item(GameObject gameObject, string[] stats) : base(gameObject)
        {
            this.Name = stats[0];
            this.Health = int.Parse(stats[1]);
            //this.Speed = float.Parse(stats[2]);
            /*
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.NumberFormat.NumberGroupSeparator = ",";
            culture.NumberFormat.CurrencyDecimalSeparator = ".";
            culture.NumberFormat.CurrencyGroupSeparator = ",";
            culture.NumberFormat.PercentDecimalSeparator = ".";
            culture.NumberFormat.PercentGroupSeparator = ",";
            */
            this.Speed = float.Parse(stats[2]);
            this.DamageResistance = int.Parse(stats[3]);
            this.LavaResistance = int.Parse(stats[4]);
            this.Value = int.Parse(stats[5]);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("ItemSheet2");

            animator.CreateAnimation("Speed", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("LavaRes", new Animation(1, 0, 2, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("DmgRes", new Animation(1, 0, 3, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Hp", new Animation(1, 0, 1, 32, 32, 10, Vector2.Zero, sprite));


            if (Name == "Speed")
            {
                animator.PlayAnimation("Speed");
            }
            if (Name == "LavaRes")
            {
                animator.PlayAnimation("LavaRes");
            }
            if (Name == "DmgRes")
            {
                animator.PlayAnimation("DmgRes");
            }
            if (Name == "Hp")
            {
                animator.PlayAnimation("Hp");
            }
            strategy = new Idle(animator);
        }
    }
}
