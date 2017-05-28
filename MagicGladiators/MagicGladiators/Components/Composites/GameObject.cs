using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicGladiators
{
    public class GameObject : Component
    {
        public Transform transform { get; set; }
        public ContentManager content { get; private set; }
        public NetConnection Connection { get; set; }

        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float Speed { get; set; } = 1;
        public float DamageResistance { get; set; } = 1;
        public float LavaResistance { get; set; } = 1;
        public float HealthRegen { get; set; } = 0.1F;
        public float KnockBackResistance { get; set; } = 1;
        public float ProjectileSpeed { get; set; } = 1;
        public float LifeSteal { get; set; } = 0;
        public float CooldownReduction { get; set; } = 1;
        public float AoeBonus { get; set; } = 1;

        public List<Component> components = new List<Component>();
        private List<Component> componentsToRemove = new List<Component>();

        public string Tag { get; set; } = "Untagged";
        public ObjectType objectType { get; set; }

        private bool isLoaded = false;

        public GameObject()
        {
            this.transform = new Transform(this, Vector2.Zero);

            AddComponent(transform);
        }

        /// <summary>
        /// Loads the GameObject's content, this is where we load sounds, sprites etc.
        /// </summary>
        /// <param name="content">The Content from the GameWorld</param>
        public void LoadContent(ContentManager content)
        {
            this.content = content;
            if (!isLoaded)
            {
                foreach (Component component in components)
                {
                    if (component is ILoadable)
                    {
                        (component as ILoadable).LoadContent(content);
                    }
                }

                isLoaded = true;
            }

        }

        /// <summary>
        /// Updates all the GameObject's components
        /// </summary>
        public void Update()
        {
            //Updates all updatable components
            foreach (Component component in components)
            {
                if (component is IUpdateable)
                {
                    (component as IUpdateable).Update();
                }
                if (component is IAbility)
                {
                    (component as IAbility).Cooldown();
                }
            }
            UpdateComponents();
        }

        public void UpdateComponents()
        {
            if (componentsToRemove.Count > 0)
            {
                foreach (Component comp in componentsToRemove)
                {
                    components.Remove(comp);
                }
                componentsToRemove.Clear();
            }
        }

        /// <summary>
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component component in components)
            {
                if (component is IDrawable)
                {
                    (component as IDrawable).Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Adds a component to the GameObject
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            componentsToRemove.Add(component);
        }

        /// <summary>
        /// Returns the specified component if it exists
        /// </summary>
        /// <param name="component">The component to find</param>
        /// <returns></returns>
        public Component GetComponent(string component)
        {
            return components.Find(x => x.GetType().Name == component);
        }

        public void OnAnimationDone(string animationName)
        {
            foreach (Component component in components)
            {
                if (component is IAnimateable) //Checks if any components are IAnimateable
                {
                    //If a component is IAnimateable we call the local implementation of the method
                    (component as IAnimateable).OnAnimationDone(animationName);
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
            foreach (Component component in components)
            {
                if (component is ICollisionStay) //Checks if any components are IAnimateable
                {
                    //If a component is IAnimateable we call the local implementation of the method
                    (component as ICollisionStay).OnCollisionStay(other);
                }
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            foreach (Component component in components)
            {
                if (component is ICollisionEnter) //Checks if any components are IAnimateable
                {
                    //If a component is IAnimateable we call the local implementation of the method
                    (component as ICollisionEnter).OnCollisionEnter(other);
                }
            }
        }

        public void OnCollisionExit(Collider other)
        {
            foreach (Component component in components)
            {
                if (component is ICollisionExit) //Checks if any components are IAnimateable
                {
                    //If a component is IAnimateable we call the local implementation of the method
                    (component as ICollisionExit).OnCollisionExit(other);
                }
            }
        }
    }
}
