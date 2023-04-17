using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
            m_graphics.PreferredBackBufferWidth = 1920;
            m_graphics.PreferredBackBufferHeight = 1080;
            m_graphics.ApplyChanges();

            /* initialize game states */
            m_states = new Dictionary<States.GameStateType, States.IState>();

            m_states.Add(States.GameStateType.MainMenu, new States.MenuState());
            m_states.Add(States.GameStateType.Sandbox, new States.GameState(0, States.GameStateType.Sandbox));
            m_states.Add(States.GameStateType.Level1, new States.GameState(1, States.GameStateType.Level1));
            m_states.Add(States.GameStateType.Level2, new States.GameState(2, States.GameStateType.Level2));
            m_states.Add(States.GameStateType.Level3, new States.GameState(3, States.GameStateType.Level3));
            m_states.Add(States.GameStateType.Level4, new States.GameState(4, States.GameStateType.Level4));
            m_states.Add(States.GameStateType.Level5, new States.GameState(5, States.GameStateType.Level5));
            //m_states.Add(States.GameStateType.Controls, new State
            m_states.Add(States.GameStateType.Quit, null);

            foreach (var i in m_states.Values)
            {
                i.initialize(this.GraphicsDevice, m_graphics);
            }

            m_currState = States.GameStateType.MainMenu;
            m_prevState = States.GameStateType.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var i in m_states.Values)
            {
                i.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (m_currState != States.GameStateType.Quit)
            {
                m_currState = m_states[m_currState].processInput(gameTime);
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
                    m_states[m_currState].reset(gameTime);
                    m_prevState = m_currState;
                }

                m_states[m_currState].update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin(SpriteSortMode.Deferred);

            if (m_currState != States.GameStateType.Quit)
            {
                m_states[m_currState].render(m_spriteBatch);
            }

            m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
