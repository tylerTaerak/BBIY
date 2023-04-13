using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CS5410.Systems
{
    public abstract class System
    {
        protected Dictionary<uint, Entities.Entity> m_entities;

        private Type[] ComponentTypes
        {
            get;
            set;
        }

        /* initialize entity map and define components to use entities */
        public System(params Type[] components)
        {
            m_entities = new Dictionary<uint, Entities.Entity>();
            ComponentTypes = components;
        }

        /*
         * check if entity matches the required components for the system
         * - can override in subclasses if necessary
         */
        protected virtual bool IsInterested(Entities.Entity entity)
        {
            foreach (Type type in ComponentTypes)
            {
                if (!entity.ContainsComponent(type))
                {
                    return false;
                }
            }
            return true;
        }

        /* Add entity to m_entities, if entity matches the system */
        public bool Add(Entities.Entity entity)
        {
            if (IsInterested(entity))
            {
                m_entities.Add(entity.Id, entity);
                return true;
            }
            return false;
        }

        /* remove an element given an id */
        public bool Remove(uint id)
        {
            return m_entities.Remove(id);
        }

        /* abstract method for updating each game loop */
        public abstract void update(GameTime gameTime);
    }
}
