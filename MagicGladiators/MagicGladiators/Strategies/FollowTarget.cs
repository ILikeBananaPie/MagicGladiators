using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MagicGladiators.DIRECTION;


namespace MagicGladiators
{
    class FollowTarget : IStrategy
    {
        private Transform target;
        private Transform transform;
        private Animator animator;
        private float movementSpeed = 100;

        public FollowTarget(Transform target, Transform transform, Animator animator)
        {
            this.target = target;
            this.transform = transform;
            this.animator = animator;
        }
        public void Execute(ref DIRECTION currentDirection)
        {
            Vector2 translation = Vector2.Zero;

            if( target.position.Y <= transform.position.Y)
            {
                translation += new Vector2(0, -10);
                currentDirection = Back;               
            }
            if (target.position.X <= transform.position.X)
            {
                translation += new Vector2(-10, 0);
                currentDirection = Left;
            }
            if (target.position.Y >= transform.position.Y)
            {
                translation += new Vector2(0, 10);
                currentDirection = Front;
            }
            if (target.position.X >= transform.position.X)
            {
                translation += new Vector2(10, 0);
                currentDirection = Right;
            }
            transform.Translate(translation * movementSpeed * GameWorld.Instance.deltaTime);
            
        }
    }
}
