using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CS5410.States
{
    public class MenuState : IState
    {
        /**
         *  NOTE: this needs to *dynamically* have level options
         */
        private Dictionary<int, (string, GameStateType)> m_options;
        private int m_choice;
        private List<Keys> m_keyPresses;

        private int m_width;
        private int m_height;

        private SpriteFont m_mainFont;
        private SpriteFont m_largeFont;

        public static List<GameState> s_levels;

        public GameState Level
        {
            get;
            set;
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_options = new Dictionary<int, (string, GameStateType)>();

            int count = 0;
            foreach (var state in s_levels)
            {
                m_options.Add(count++, (state.Title, GameStateType.Level));
            }

            m_options.Add(count++, ("Controls", GameStateType.Controls));
            m_options.Add(count++, ("Credits", GameStateType.Credits));
            m_options.Add(count++, ("Quit", GameStateType.Quit));

            m_choice = 0;

            m_keyPresses = new List<Keys>();

            m_width = graphics.PreferredBackBufferWidth;
            m_height = graphics.PreferredBackBufferHeight;
        }

        public void loadContent(ContentManager content)
        {
            m_mainFont = content.Load<SpriteFont>("fonts/Main");
            m_largeFont = content.Load<SpriteFont>("fonts/Large");
        }

        public void reset(GameTime gameTime)
        {
            m_choice = 0;
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
                            m_choice = Math.Max(--m_choice, 0);
                            break;
                        case Systems.Commands.Down:
                            m_choice = Math.Min(++m_choice, m_options.Count-1);
                            break;
                        case Systems.Commands.Confirm:
                            if (m_options[m_choice].Item2 == GameStateType.Level)
                            {
                                Level = s_levels[m_choice];
                            }
                            return m_options[m_choice].Item2;

                    }
                }
            }

            m_keyPresses.Clear();
            m_keyPresses.AddRange(keysPressed);

            return GameStateType.MainMenu;
        }

        public void update(GameTime gameTime)
        {
        }

        public void render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                    m_largeFont,
                    "Big Blue is You",
                    new Vector2(
                        m_width / 2 - m_largeFont.MeasureString("Big Blue is You").X/2,
                        40
                        ),
                    Color.White
                    );

            float spacing = 40 + m_largeFont.MeasureString("Big Blue is You").Y + 40;

            int count = 0;
            foreach (var level in s_levels)
            {
                string s = level.Title;

                spriteBatch.DrawString(
                        m_mainFont,
                        s,
                        new Vector2(
                            m_width / 2 - m_mainFont.MeasureString(s).X/2,
                            spacing
                            ),
                        m_choice == count++ ? Color.Gold : Color.White
                        );

                spacing += m_mainFont.MeasureString(s).Y + 20;
            }

            spriteBatch.DrawString(
                    m_mainFont,
                    "Controls",
                    new Vector2(
                        m_width / 2 - m_mainFont.MeasureString("Controls").X/2,
                        spacing
                        ),
                    m_choice == count++ ? Color.Gold : Color.White
                    );

            spacing += m_mainFont.MeasureString("Controls").Y + 20;

            spriteBatch.DrawString(
                    m_mainFont,
                    "Credits",
                    new Vector2(
                        m_width / 2 - m_mainFont.MeasureString("Credits").X/2,
                        spacing
                        ),
                    m_choice == count++ ? Color.Gold : Color.White
                    );

            spacing += m_mainFont.MeasureString("Credits").Y + 20;

            spriteBatch.DrawString(
                    m_mainFont,
                    "Quit",
                    new Vector2(
                        m_width / 2 - m_mainFont.MeasureString("Quit").X/2,
                        spacing
                        ),
                    m_choice == count++ ? Color.Gold : Color.White
                    );
        }
    }
}
