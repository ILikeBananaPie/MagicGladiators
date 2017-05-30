﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Clone : Component, IUpdateable, ILoadable, ICollisionEnter
    {
        private Animator animator;
        private List<string> abilities = new List<string>() { "Fireball", "Chain", "Drain", "HomingMissile", "RollingMeteor", "DeathMeteor" };

        public Clone(GameObject gameObject) : base(gameObject)
        {

        }

        private void CreateAnimations()
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            animator.CreateAnimation("LightGreen", new Animation(1, 64, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Green", new Animation(1, 96, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Blue", new Animation(1, 96, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Red", new Animation(1, 0, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Orange", new Animation(1, 32, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Brown", new Animation(1, 0, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Yellow", new Animation(1, 64, 0, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));
            animator.CreateAnimation("Purple", new Animation(1, 32, 1, 32, 32, 6, Vector2.Zero, spriteRenderer.Sprite));


            animator.PlayAnimation("LightGreen");

        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)gameObject.GetComponent("Animator");
            CreateAnimations();
        }

        public void Update()
        {

            foreach (GameObject go in GameWorld.gameObjects)
            {
                if ((go.Tag == "Enemy" || go.Tag == "Player") && go.Id == gameObject.Id)
                {
                    if (gameObject.Tag.Contains("Left"))
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X - 64, go.transform.position.Y);
                    }
                    if (gameObject.Tag.Contains("Right"))
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X + 64, go.transform.position.Y);
                    }
                    if (gameObject.Tag.Contains("Up"))
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y - 64);
                    }
                    if (gameObject.Tag.Contains("Down"))
                    {
                        gameObject.transform.position = new Vector2(go.transform.position.X, go.transform.position.Y + 64);
                    }
                }
            }
        }

        public void OnCollisionEnter(Collider other)
        {

        }
    }
}