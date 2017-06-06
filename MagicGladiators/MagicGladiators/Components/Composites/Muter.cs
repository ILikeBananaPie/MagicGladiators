using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MagicGladiators.DIRECTION;

namespace MagicGladiators
{
    class Muter : Component, ILoadable, IUpdateable
    {
        private Animator animator;

        public Muter(GameObject gameObject) : base(gameObject)
        {
            

        }

        public void LoadContent(ContentManager content)
        {
            

            animator = (Animator)gameObject.GetComponent("Animator");
            Texture2D sprite = content.Load<Texture2D>("Mute");
            animator.CreateAnimation("Mute", new Animation(1, 0, 0, 203, 80, 6, Vector2.Zero, sprite));
            animator.CreateAnimation("Muted", new Animation(1, 80, 0, 203, 80, 6, Vector2.Zero, sprite));
            animator.PlayAnimation("Mute");
        }

        public void Update()
        {
            if (MediaPlayer.IsMuted)
            {
                animator.PlayAnimation("Muted");
            } else
            {
                animator.PlayAnimation("Mute");
            }
        }
    }
}
