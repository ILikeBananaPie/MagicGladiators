using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class ChooseTarget : IStrategy
    {
        private GameObject go;
        private Vector2 target;
        private Transform transform;
        private Animator animator;
        private float distance;
        private Vector2 bestTarget;
        private float points;
        private float currentPoints;

        public ChooseTarget(Vector2 target, Transform transform, Animator animator)
        {
            this.target = target;
            
            this.transform = transform;
            this.animator = animator;
        }

        public void Execute(ref DIRECTION currentDirection)
        {

            //points lave, edge outer ring , middle, holemap = hole, skyder imod dig, deflect = 0; 

            //modstander tættest på kanten

            //modstander tættest på hullet

            //modstander tættest på selv

            //kommer imod en

            //modstander der har ramt dig mest

            //modstander der har vundet flest runder

            //modstander der har lavet mest skade

            //modstander med flest kills

        }

        public void CurrentTarget()
        {

        }

        public void FindTarget()
        {
            foreach (GameObject go in GameWorld.gameObjects)
            {

                if (Vector2.Distance(go.transform.position, go.transform.position) < 10000 && (go.Tag == "Player"))

                {

                    points = Vector2.Distance(go.transform.position, go.transform.position);
                    bestTarget = go.transform.position;
                    foreach (GameObject item in GameWorld.gameObjects)
                    {
                        if (Vector2.Distance(go.transform.position, item.transform.position) < distance && (item.Tag == "Dummy" || item.Tag == "Enemy"))

                        {

                            points = Vector2.Distance(go.transform.position, item.transform.position);
                            bestTarget = item.transform.position;

                        }
                    }

                }
            }

        }


    }

}
