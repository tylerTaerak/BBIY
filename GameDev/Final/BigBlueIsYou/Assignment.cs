using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace CS5410
{
    public class Assignment : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private Dictionary<States.GameStateType, States.IState> m_states;

        private States.GameStateType m_prevState;
        private States.GameStateType m_currState;

        public Assignment()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            /* Set Graphics Settings */
            // something's not working quite right here... Maybe it's just Hyprland though (twas)
            m_graphics.PreferredBackBufferWidth = 1920;
            m_graphics.PreferredBackBufferHeight = 1080;
            m_graphics.ApplyChanges();

            /* initialize game states */
            m_states = new Dictionary<States.GameStateType, States.IState>();

            m_states.Add(States.GameStateType.MainMenu, new States.MenuState());
            var menu = (States.MenuState)m_states[States.GameStateType.MainMenu];

            States.MenuState.s_levels = new List<States.GameState>();

            // create levels
            for (int i=0; i<Load.loadLevels().Count; i++)
            {
                States.MenuState.s_levels.Add(new States.GameState(i));
            }


            m_states.Add(States.GameStateType.Controls, new States.ControlsState());
            m_states.Add(States.GameStateType.Credits, new States.CreditState());

            // define default keymap for input
            Dictionary<Keys, Systems.Commands> keys = new Dictionary<Keys, Systems.Commands>();
            keys.Add(Keys.Up, Systems.Commands.Up);
            keys.Add(Keys.Down, Systems.Commands.Down);
            keys.Add(Keys.Left, Systems.Commands.Left);
            keys.Add(Keys.Right, Systems.Commands.Right);
            keys.Add(Keys.Z, Systems.Commands.Undo);
            keys.Add(Keys.R, Systems.Commands.Reset);
            keys.Add(Keys.Escape, Systems.Commands.Return);
            keys.Add(Keys.Enter, Systems.Commands.Confirm);

            Systems.InputSystem.s_keyCommands = keys;

            LoadInput.LoadInputMap(); // override with user-defined keymap


            foreach (var i in m_states.Values)
            {
                i.initialize(this.GraphicsDevice, m_graphics);
            }

            foreach (var level in States.MenuState.s_levels)
            {
                level.initialize(this.GraphicsDevice, m_graphics);
            }

            m_currState = States.GameStateType.MainMenu;
            m_prevState = States.GameStateType.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent() {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var i in m_states.Values)
            {
                i.loadContent(this.Content);
            }
            foreach (var i in States.MenuState.s_levels)
            {
                i.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (m_currState != States.GameStateType.Quit && m_currState != States.GameStateType.Level)
            {
                m_currState = m_states[m_currState].processInput(gameTime);
            }
            if (m_currState == States.GameStateType.Level)
            {
                States.MenuState menu = (States.MenuState)m_states[States.GameStateType.MainMenu];
                States.GameState level = menu.Level;
                m_currState = level.processInput(gameTime);
            }

            if (m_currState == States.GameStateType.Quit)
            {
                // save any data
                Exit();
            }

            else
            {
                if (m_prevState != m_currState)
                {
                    Console.WriteLine(m_prevState + ", " + m_currState);
                    if (m_currState == States.GameStateType.Level)
                    {
                        States.MenuState menu = (States.MenuState)m_states[States.GameStateType.MainMenu];
                        States.GameState level = menu.Level;
                        level.reset(gameTime);
                    }
                    else
                    {
                        m_states[m_currState].reset(gameTime);
                    }
                    m_prevState = m_currState;
                }

                if (m_currState == States.GameStateType.Level)
                {
                    States.MenuState menu = (States.MenuState)m_states[States.GameStateType.MainMenu];
                    States.GameState level = menu.Level;
                    level.update(gameTime);
                }
                else
                {
                    m_states[m_currState].update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin(SpriteSortMode.Deferred);

            if (m_currState != States.GameStateType.Quit && m_currState != States.GameStateType.Level)
            {
                m_states[m_currState].render(m_spriteBatch);
            }
            if (m_currState == States.GameStateType.Level)
            {
                States.MenuState menu = (States.MenuState)m_states[States.GameStateType.MainMenu];
                States.GameState level = menu.Level;
                level.render(m_spriteBatch);
            }

            m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
