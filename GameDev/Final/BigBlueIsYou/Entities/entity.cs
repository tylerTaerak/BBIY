using System;
using System.Collections.Generic;

namespace CS5410.Entities
{
    public sealed class Entity
    {
        private readonly Dictionary<Type, Components.Component> components;
        private static uint m_nextId; // id of next entity made

        // ID of this entity
        public uint Id
        {
            get;
            private set;
        }

        public Entity()
        {
            Id = m_nextId++;
        }

        private bool ContainsComponent(Type type)
        {
            return components.ContainsKey(type) && components[type] != null;
        }

        public bool ContainsComponent<TComponent>()
            where TComponent : Components.Component
        {
            return ContainsComponent(typeof(TComponent));
        }

        public void AddComponents(params Components.Component[] comps)
        {
            foreach (Components.Component c in comps)
            {
                Type type = c.GetType();

                this.components.Add(type, c);
            }
        }

        public void Clear()
        {
            components.Clear();
        }

        public void RemoveComponents(params Components.Component[] comps)
        {
            foreach (var c in comps)
            {
                this.components.Remove(c.GetType());
            }
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : Components.Component
        {
            return (TComponent)this.components[typeof(TComponent)];
        }
    }
}
