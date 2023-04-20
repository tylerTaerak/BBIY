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

        private bool m_waitForChanges;
        private bool m_changeComplete;
        private bool m_printSuccess;

        public List<Keys> m_keyPresses;

        public int m_windowWidth;
        public int m_windowHeight;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_windowWidth = graphics.PreferredBackBufferWidth;
            m_windowHeight = graphics.PreferredBackBufferHeight;
            m_keyPresses = new List<Keys>();
            m_keyPresses.Add(Keys.Enter);
        }

        public void loadContent(ContentManager content)
        {
            m_mainFont = content.Load<SpriteFont>("fonts/Main");
            m_largeFont = content.Load<SpriteFont>("fonts/Large");
        }

        public void reset(GameTime gameTime)
        {
            m_changeComplete = false;
            m_waitForChanges = false;
            m_printSuccess = false;

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
                    if (m_waitForChanges)
                    {
                        Keys prevKey = m_choices[m_currChoice];
                        Systems.Commands prevCmd = m_tempMap[prevKey];

                        m_tempMap.Remove(prevKey);

                        // what to do if the player chooses a key that's already been mapped?
                        if (m_tempMap.ContainsKey(k))
                        {
                            var otherCmd = m_tempMap[k];
                            int index = m_choices.IndexOf(k);
                            m_choices[index] = Keys.None;
                            m_tempMap.Remove(k);
                            m_tempMap.Add(Keys.None, otherCmd);
                        }

                        m_choices[m_currChoice] = k;


                        m_tempMap.Add(k, prevCmd);

                        m_changeComplete = true;
                    }
                    else
                    {
                        switch(k)
                        {
                            case Keys.Up:
                                m_currChoice = Math.Max(--m_currChoice, 0);
                                break;
                            case Keys.Down:
                                m_currChoice = Math.Min(++m_currChoice, m_choices.Count);
                                break;
                            case Keys.Enter:
                                if (m_currChoice == m_choices.Count)
                                {
                                    Systems.InputSystem.s_keyCommands = m_tempMap;
                                    m_printSuccess = true;
                                }
                                else
                                {
                                    m_waitForChanges = true;
                                    m_changeComplete = false;
                                }
                                break;
                            case Keys.Escape:
                                return GameStateType.MainMenu;
                        }
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

            string s = String.Format("{0,-8} : {1,-8}", "Key", "Command");
            spriteBatch.DrawString(
                    m_mainFont,
                    s,
                    new Vector2(
                        m_windowWidth / 2 - m_mainFont.MeasureString(s).X / 2,
                        spacing
                        ),
                    Color.White
                    );

            spacing += m_mainFont.MeasureString(s).Y + 20;

            if (m_waitForChanges)
            {
                spriteBatch.DrawString(
                        m_mainFont,
                        "Press a key to change to",
                        new Vector2(
                            m_windowWidth / 2 - m_mainFont.MeasureString("Press a key to change to").X/2,
                            spacing
                            ),
                        Color.Gold
                        );
                if (m_changeComplete)
                {
                    m_waitForChanges = false;
                }

            }

            if (m_printSuccess)
            {
                spriteBatch.DrawString(
                        m_mainFont,
                        "Controls Changed Successfully",
                        new Vector2(
                            m_windowWidth / 2 - m_mainFont.MeasureString("Controls Changed Successfully").X/2,
                            m_windowHeight - 40 - m_mainFont.MeasureString("Controls Changed Successfully").Y
                            ),
                        Color.Gold
                        );
            }

            spacing += m_mainFont.MeasureString("Press a key to change to").Y + 20;


            int count = 0;
            foreach (var kv in m_tempMap)
            {
                s = String.Format("{0,-8} : {1,-8}", Enum.GetName(typeof(Keys), kv.Key), Enum.GetName(typeof(Systems.Commands), kv.Value));
                spriteBatch.DrawString(
                        m_mainFont,
                        s,
                        new Vector2(
                            m_windowWidth / 2 - m_mainFont.MeasureString(s).X / 2,
                            spacing
                            ),
                        m_currChoice == count++ ? Color.Gold : Color.White
                        );

                spacing += m_mainFont.MeasureString(s).Y + 20;
            }

            spriteBatch.DrawString(
                    m_mainFont,
                    "Apply Changes",
                    new Vector2(
                        m_windowWidth / 2 - m_mainFont.MeasureString("Apply Changes").X/2,
                        spacing
                        ),
                    m_currChoice == count ? Color.Gold : Color.White
                    );
        }
    }
}
