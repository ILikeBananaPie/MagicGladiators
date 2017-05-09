using System;
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

        public Enemy/*Number 1*/(GameObject gameObject) : base(gameObject)
        {

        }

        public void LoadContent(ContentManager content)
        {
            trnsfrm = gameObject.GetComponent("Transform") as Transform;
        }

        public void Update()
        {
            //trnsfrm.Translate(direction * GameWorld.Instance.deltaTime);
        }

        public void UpdateEnemyInfo(UpdatePackage package)
        {
            this.trnsfrm.position = package.position;
            this.velocity = package.velocity;
        }
    }
}
