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
    class AbilityIcon : Component, ILoadable
    {
        private Animator animator;

        public string Name { get; set; }
        public int Value { get; set; }
        public int index { get; set; }

        public AbilityIcon(GameObject gameObject, string name, int value) : base(gameObject)
        {
            this.Name = name;
            this.Value = value;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("SpellSheet2");

            animator.CreateAnimation("HomingMissile", new Animation(1, 64, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Charge", new Animation(1, 32, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Blink", new Animation(1, 0, 2, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Drain", new Animation(1, 0, 3, 32, 32, 10, Vector2.Zero, sprite));

            animator.CreateAnimation("Fireball", new Animation(1, 0, 0, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Deflect", new Animation(1, 0, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Mine", new Animation(1, 32, 1, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("SpeedBoost", new Animation(1, 32, 2, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Chain", new Animation(1, 32, 3, 32, 32, 10, Vector2.Zero, sprite));
            animator.CreateAnimation("Nova", new Animation(1, 64, 1, 32, 32, 10, Vector2.Zero, sprite));

            animator.PlayAnimation(Name);
        }
    }
}
