using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace CS5410.States
{
    public class ControlsState : IState
    {
        private SpriteFont m_mainFont;
        private SpriteFont m_largeFont;

        private Dictionary<Keys, Systems.Commands> m_tempMap;
        private List<Keys> m_choices;
        private int m_currChoice;

        public List<Keys> m_keyPresses;

        public int m_windowWidth;
        public int m_windowHeight;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_windowWidth = graphics.PreferredBackBufferWidth;
            m_windowHeight = graphics.PreferredBackBufferHeight;
        }

        public void loadContent(ContentManager content)
        {
            m_mainFont = content.Load<SpriteFont>("fonts/Main");
            m_largeFont = content.Load<SpriteFont>("fonts/Large");
        }

        public void reset(GameTime gameTime)
        {
            m_currChoice = 0;
            m_choices = new List<Keys>();

            // allocate temporary mapping for editing
            m_tempMap = new Dictionary<Keys, Systems.Commands>();
            foreach (var kv in Systems.InputSystem.s_keyCommands)
            {
                m_choices.Add(kv.Key);
                m_tempMap.Add(kv.Key, kv.Value);
            }
        }

        public GameStateType processInput(GameTime gameTime)
        {
            List<Keys> keysPressed = new List<Keys>();
            keysPressed.AddRange(Keyboard.GetState().GetPressedKeys());

            foreach (Keys k in keysPressed)
            {
                if (!m_keyPresses.Contains(k))
                {
                    switch(Systems.InputSystem.s_keyCommands[k])
                    {
                        case Systems.Commands.Up:
                            m_currChoice = Math.Max(--m_currChoice, 0);
                            break;
                        case Systems.Commands.Down:
                            m_currChoice = Math.Min(++m_currChoice, m_choices.Count-1);
                            break;
                        case Systems.Commands.Confirm:
                            // apply changes to input map
                            Systems.InputSystem.s_keyCommands.Clear();
                            foreach(var kv in m_tempMap)
                            {
                                Systems.InputSystem.s_keyCommands.Add(kv.Key, kv.Value);
                            }
                            break;
                        case Systems.Commands.Return:
                            return GameStateType.MainMenu;

                    }
                }
            }

            m_keyPresses.Clear();
            m_keyPresses.AddRange(keysPressed);

            return GameStateType.Controls;
        }

        public void update(GameTime gameTime)
        {
        }

        public void render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                    m_largeFont,
                    "Controls",
                    new Vector2(
                        m_windowWidth / 2 - m_largeFont.MeasureString("Controls").X/2,
                        40
                        ),
                    Color.White
                    );

            float spacing = 40 + m_largeFont.MeasureString("Controls").Y + 40;

            string s = String.Format("{0} : {1,-8}", "Key", "Command");
            spriteBatch.DrawString(
                    m_mainFont,
                    s,
                    new Vector2(
                        m_windowWidth / 2 - m_mainFont.MeasureString(s).X / 2,
                        spacing
                        ),
                    Color.White
                    );

            int count = 0;
            foreach (var kv in m_tempMap)
            {
                s = String.Format("{0} : {1,-8}", Enum.GetName(typeof(Keys), kv.Key), Enum.GetName(typeof(Systems.Commands), kv.Value));
                spriteBatch.DrawString(
                        m_mainFont,
                        s,
                        new Vector2(
                            m_windowWidth / 2 - m_mainFont.MeasureString(s).X / 2,
                            spacing
                            ),
                        m_currChoice == count ? Color.Gold : Color.White
                        );

                spacing += m_mainFont.MeasureString(s).Y + 20;
            }
        }
    }
}
