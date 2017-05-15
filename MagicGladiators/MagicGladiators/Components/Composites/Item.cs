using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Item : Component, ILoadable
    {
        private Animator animator;
        private IStrategy strategy;

        public string Name { get; set; }
        public int Health { get; set; }
        public int Speed { get; set; }
        public int DamageResistance { get; set; }
        public int LavaResistance { get; set; }
        public int Value { get; set; }

        public Item(GameObject gameObject, string[] stats) : base(gameObject)
        {
            this.Name = stats[0];
            this.Health = int.Parse(stats[1]);
            this.Speed = int.Parse(stats[2]);
            this.DamageResistance = int.Parse(stats[3]);
            this.LavaResistance = int.Parse(stats[4]);
            this.Value = int.Parse(stats[5]);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("ItemSheet");

            animator.CreateAnimation("Speed", new Animation(1, 0, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("LavaRes", new Animation(1, 32, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("DmgRes", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Hp", new Animation(1, 32, 0, 32, 32, 10, Vector2.Zero, sprite));


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
