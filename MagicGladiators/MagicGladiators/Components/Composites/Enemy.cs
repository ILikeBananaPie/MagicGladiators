﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators.Components.Composites
{
    class Enemy:Component, ILoadable, IUpdateable
    {
        private Transform trnsfrm;
        private Vector2 velocity;

        public static Vector2 accelerationTest;
        public static Vector2 velocityTest;
        private float breakTest = 0.050F;

        public Enemy/*Number 1*/(GameObject gameObject) : base(gameObject)
        {

        }

        public void LoadContent(ContentManager content)
        {
            trnsfrm = gameObject.GetComponent("Transform") as Transform;
        }

        public void Update()
        {
            /*
            if (!(Vector2.Distance(velocityTest, Vector2.Zero) > 0.05F && Vector2.Distance(velocityTest, Vector2.Zero) < -0.05F))
            {
                accelerationTest = breakTest * -velocityTest;
                velocityTest += accelerationTest;
                //accelerationTest = Vector2.Zero;

            }
            if (Vector2.Distance(velocityTest, Vector2.Zero) < 0.05F && Vector2.Distance(velocityTest, Vector2.Zero) > -0.05F)
            {
                velocityTest = Vector2.Zero;

            }
            */
            gameObject.transform.position += velocityTest;
        }

        public void UpdateEnemyInfo(UpdatePackage package)
        {
            this.trnsfrm.position = package.position;
            this.velocity = package.velocity;
        }
    }
}
