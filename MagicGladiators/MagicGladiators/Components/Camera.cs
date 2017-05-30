using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class Camera : Component, IUpdateable
    {
        public Viewport view { get; set; }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }


        public Camera(Viewport newViewport)
        {
            view = newViewport;

            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(newViewport.Width / 2 - 200, newViewport.Height / 2 - 140);

            Position = Vector2.Zero;

        }
        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1);
        }

        public void Update()
        {

        }
    }
}
