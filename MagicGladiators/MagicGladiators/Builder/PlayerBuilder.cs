using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MagicGladiators
{
    class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;


        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject();

            gameObject.Tag = "Player";

            gameObject.AddComponent(new SpriteRenderer(gameObject, "PlayerSheet", 1));
                     
            gameObject.AddComponent(new Animator(gameObject));

            gameObject.AddComponent(new Player(gameObject, gameObject.transform));

            gameObject.AddComponent(new Collider(gameObject, false, true));

            gameObject.AddComponent(new Physics(gameObject));

            gameObject.AddComponent(new RollingMeteor(gameObject, gameObject.transform, gameObject.GetComponent("Animator") as Animator));
            gameObject.AddComponent(new DeathMine(gameObject, gameObject.transform.position));
            gameObject.AddComponent(new Firewave(gameObject));
            gameObject.AddComponent(new Critters(gameObject));
            gameObject.AddComponent(new ShrinkMap(gameObject));
            gameObject.AddComponent(new SlowField(gameObject));

            gameObject.AddComponent(new Fireball(gameObject));

            MouseState mouse = Mouse.GetState();
            gameObject.transform.position = position;

        }

        public void SetCooldownIcon()
        {
            (gameObject.components.Last() as Ability).icon = Player.deathAbilities.Last();
        }

        public void BuildIcon(Vector2 position, string name, string text)
        {
            throw new NotImplementedException();
        }

        public void BuildIcon(Vector2 position, string name, int value, string text)
        {
            throw new NotImplementedException();
        }

        public void BuildItem(Vector2 position, string[] stats)
        {
            throw new NotImplementedException();
        }

        public void BuildMapPart(Vector2 position, string name)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability)
        {
            throw new NotImplementedException();
        }

        public void FireProjectile(Vector2 position, Vector2 targetVector, string ability, GameObject shooter, string id)
        {
            throw new NotImplementedException();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

    }
}
