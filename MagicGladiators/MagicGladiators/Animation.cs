using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Animation
    {

        public float FPS { get; private set; }

        public Rectangle[] Rectangles { get; private set; }


        public Vector2 Offset { get; private set; }

        public Color[][] Colors { get; private set; }

        public float MyFrames { get; private set; }


        /// <summary>
        /// Animations constructor divides a sprite sheet into frames there can be used to animations 
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="FPS"></param>
        /// <param name="offset"></param>
        /// <param name="sprite"></param>
        public Animation(int frames, int yPos, int xStartFrame, int width, int height, float FPS, Vector2 offset, Texture2D sprite)
        {
            Rectangles = new Rectangle[frames];

            Offset = offset;

            this.FPS = FPS;

            this.MyFrames = frames;

            for (int i = 0; i < frames; i++) //Creates the rectangles based on the parameters
            {
                Rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
            }



        }

        public Animation(int frames, List<int> xPos, List<int> yPos, List<int> width, List<int> height, float FPS, Vector2 offset, Texture2D sprite)
        {
            Rectangles = new Rectangle[frames];

            Offset = offset;

            this.FPS = FPS;

            this.MyFrames = frames;

            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle(xPos[i], yPos[i], width[i], height[i]);
            }

        }
    }
}
