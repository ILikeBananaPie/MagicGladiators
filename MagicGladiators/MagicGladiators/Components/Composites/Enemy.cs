using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MagicGladiators
{
    class Enemy:Component, ILoadable, IUpdateable
    {
        private Transform trnsfrm;
        public Vector2 velocity;

        //public static Vector2 accelerationTest;
        //public static Vector2 velocityTest;
        //private float breakTest = 0.050F;

        private readonly Object thisLock = new Object();
        private UpdatePackage _updatePackage;
        public UpdatePackage updatePackage
        {
            get { lock (thisLock) { return _updatePackage; } }
            set { lock (thisLock) { _updatePackage = value; } }
        }

        public Enemy/*Number 1*/(GameObject gameObject) : base(gameObject)
        {
            updatePackage = new UpdatePackage(Vector2.Zero);
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
            trnsfrm.position += velocity;
            updatePackage.InfoUpdate(trnsfrm.position, velocity);
        }

        public void UpdateEnemyInfo(UpdatePackage package)
        {
            this.trnsfrm.position = package.position;
            this.velocity = package.velocity;
        }
        public void UpdateEnemyInfo(Vector2 pos, Vector2 vel)
        {
            this.trnsfrm.position = pos;
            this.velocity = vel;
        }
    }
}
