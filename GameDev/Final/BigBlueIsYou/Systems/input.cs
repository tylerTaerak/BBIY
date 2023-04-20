using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CS5410.Systems
{
    public enum Commands:uint
    {
        Up,
        Down,
        Left,
        Right,
        Undo,
        Reset,
        Return
    }

    // configure to be serializable using the keymap
    [DataContract(Name = "InputSystem")]
    public class InputSystem : System
    {
        // serialize the keymap
        [DataMember()]
        public static Dictionary<Keys, Commands> s_keyCommands;

        private List<Keys> m_keyPresses;

        /* public flag to undo previous move - turned false by GameState */
        public bool Undo
        {
            get;
            set;
        }

        /* public flag to reset level - turned false by GameState */
        public bool Reset
        {
            get;
            set;
        }

        /* public flag indicating new move - turned false by GameState */
        public bool NewMove
        {
            get;
            set;
        }

        /* public flag indicating to return to menu */
        public bool ReturnToMenu
        {
            get;
            set;
        }

        public InputSystem()
            : base(
                    typeof(Components.Property),
                    typeof(Components.Position)
                  )
        {
            m_keyPresses = new List<Keys>();
            Undo = false;
            Reset = false;
        }

        public override void update(GameTime gameTime)
        {
            List<Keys> keysPressed = new List<Keys>();
            keysPressed.AddRange(Keyboard.GetState().GetPressedKeys());

            foreach (Keys k in keysPressed)
            {
                if (!m_keyPresses.Contains(k))
                {
                    if (s_keyCommands.ContainsKey(k))
                    {
                        if (s_keyCommands[k] == Commands.Undo)
                        {
                            Undo = true;
                        }
                        if (s_keyCommands[k] == Commands.Reset)
                        {
                            Reset = true;
                        }
                        if (s_keyCommands[k] == Commands.Return)
                        {
                            ReturnToMenu = true;
                        }

                        moveEntities(k);
                    }
                }
            }

            m_keyPresses.Clear();
            m_keyPresses.AddRange(keysPressed);
        }

        private void moveEntities(Keys keypress)
        {
            foreach (Entities.Entity entity in m_entities.Values)
            {
                var pos = entity.GetComponent<Components.Position>();
                var (x, y) = pos.CurrentPosition;

                // I was going to override IsInterested, but since 'YOU'
                // can change, it would probably be best to just do a check
                // for 'YOU' here
                var prop = entity.GetComponent<Components.Property>();
                if (!prop.hasProperty(Components.Properties.You))
                {
                    continue;
                }

                //
                // NOTE: Keys are user-configurable, so make sure to implement that here
                //
                switch (s_keyCommands[keypress])
                {
                    case Commands.Up:
                        NewMove = true;
                        pos.Facing = Components.Direction.Up;
                        y -= 1;
                        break;
                    case Commands.Down:
                        NewMove = true;
                        pos.Facing = Components.Direction.Down;
                        y += 1;
                        break;
                    case Commands.Left:
                        NewMove = true;
                        pos.Facing = Components.Direction.Left;
                        x -= 1;
                        break;
                    case Commands.Right:
                        NewMove = true;
                        pos.Facing = Components.Direction.Right;
                        x += 1;
                        break;
                }

                entity.GetComponent<Components.Position>().CurrentPosition = (x, y);
            }
        }
    }
}
