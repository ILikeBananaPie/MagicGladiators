﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;


        public void BuildGameObject(Vector2 position, object id)
        {
            gameObject = new GameObject(id);
                            
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Player", 1));
                       
            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Player(gameObject, gameObject.transform));

           // gameObject.AddComponent(new Collider(gameObject, false));
          
           // gameObject.LoadContent(GameWorld.Instance.Content);

            gameObject.transform.position = position;

          
        }      

        public GameObject GetResult()
        {
            return gameObject;
        }
       
    }
}