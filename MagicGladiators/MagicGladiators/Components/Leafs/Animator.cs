using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class Animator : Component, IUpdateable
    {
        /// <summary>
        /// The current index of the animation
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// Time elapsed for the current animation
        /// </summary>
        private float timeElapsed;

        /// <summary>
        /// The framerate of the animation
        /// </summary>
        private float fps;

        /// <summary>
        /// The rectangle on the spritesheet
        /// </summary>
        private Rectangle[] rectangles;

        /// <summary>
        /// A reference to the spriteRenderer
        /// </summary>
        private SpriteRenderer spriteRenderer;

        public Dictionary<string, Animation> MyAnimations { get; private set; }

        public string AnimationName { get; private set; }

        public Animator(GameObject gameObject) : base(gameObject)
        {
            this.spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            this.MyAnimations = new Dictionary<string, Animation>();

        }

        public void Update()
        {
            timeElapsed += GameWorld.Instance.deltaTime;

            CurrentIndex = (int)(timeElapsed * fps);

            if (CurrentIndex > rectangles.Length - 1)
            {
                gameObject.OnAnimationDone(AnimationName);
                timeElapsed = 0;
                CurrentIndex = 0;
            }

            spriteRenderer.Rectangle = rectangles[CurrentIndex];
        }

        /// <summary>
        /// Adds a new animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="animation">The animation to add</param>
        public void CreateAnimation(string name, Animation animation)
        {
            //Adds a new animation to the dictionary
            MyAnimations.Add(name, animation);
        }

        /// <summary>
        /// Plays an animation
        /// </summary>
        /// <param name="animationName">Name of animation to play</param>
        public void PlayAnimation(string animationName)
        {
            //Checks if the animation is player
            if (this.AnimationName != animationName)
            {
                //Sets the rectangles
                this.rectangles = MyAnimations[animationName].Rectangles;

                //Sets the size of the rectangle
                this.spriteRenderer.Rectangle = rectangles[0];

                //Sets the offset
                this.spriteRenderer.Offset = MyAnimations[animationName].Offset;

                //Sets the animation name
                this.AnimationName = animationName;

                //Sets the fps
                this.fps = MyAnimations[animationName].FPS;

                //Resets the animation
                timeElapsed = 0;

                CurrentIndex = 0;
            }

        }
    }
}
