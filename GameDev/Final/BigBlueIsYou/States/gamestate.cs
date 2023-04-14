using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    public class GameState : IState
    {
        private int m_levelIndex;
        private GameStateType m_level;

        public GameState(int levelIndex, GameStateType levelId)
        {
            m_levelIndex = levelIndex;
            m_level = levelId;
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
        }

        public void loadContent(ContentManager content)
        {
            // load level at level index
            var (title, map) = Load.loadLevels(content, m_levelIndex);

            foreach (var row in map)
            {
                // add entities to all systems
            }
        }

        public GameStateType processInput(GameTime gameTime)
        {
            return m_level;
        }

        public void update(GameTime gameTime)
        {
        }

        public void render(SpriteBatch spriteBatch)
        {
        }
    }
}
