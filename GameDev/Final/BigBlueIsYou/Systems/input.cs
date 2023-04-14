using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CS5410.Systems
{
    public class InputSystem : System
    {
        private List<Keys> m_keyPresses;

        public InputSystem()
            : base(
                    typeof(Components.Property),
                    typeof(Components.Position)
                  )
        {
            m_keyPresses = new List<Keys>();
        }

        protected override bool IsInterested(Entities.Entity entity)
        {
            if (!base.IsInterested(entity))
            {
                return false;
            }

            var properties = entity.GetComponent<Components.Property>();

            // we only want components with the property "YOU"
            if (!properties.hasProperty(Components.Properties.You))
            {
                return false;
            }

            return true;
        }

        public override void update(GameTime gameTime)
        {
            List<Keys> keysPressed = new List<Keys>();
            keysPressed.AddRange(Keyboard.GetState().GetPressedKeys());

            bool undo = false;

            foreach (Keys k in keysPressed)
            {
                if (!m_keyPresses.Contains(k))
                {
                    // remember---- configurable!!!
                    if (k == Keys.Z)
                    {
                        undo = true;
                    }

                    moveEntities(k);
                }

                m_keyPresses.Clear();
                m_keyPresses.AddRange(keysPressed);

                if (undo)
                {
                    // undo a move
                }
            }
        }

        private void moveEntities(Keys keypress)
        {
            foreach (Entities.Entity entity in m_entities.Values)
            {
                var pos = entity.GetComponent<Components.Position>();
                var (x, y) = pos.CurrentPosition;

                //
                // NOTE: Keys are user-configurable, so make sure to implement that here
                //
                switch (keypress)
                {
                    case Keys.Up:
                        pos.Facing = Components.Direction.Up;
                        y -= 1;
                        break;
                    case Keys.Down:
                        pos.Facing = Components.Direction.Down;
                        y += 1;
                        break;
                    case Keys.Left:
                        pos.Facing = Components.Direction.Left;
                        x -= 1;
                        break;
                    case Keys.Right:
                        pos.Facing = Components.Direction.Right;
                        x += 1;
                        break;
                }

                entity.GetComponent<Components.Position>().CurrentPosition = (x, y);
            }
        }
    }
}
