using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    public interface IState
    {
        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics);

        public void loadContent(ContentManager content);

        public void reset(GameTime gameTime);

        public GameStateType processInput(GameTime gameTime);

        public void update(GameTime gameTime);

        public void render(SpriteBatch spriteBatch);
    }

    public enum GameStateType
    {
        MainMenu,
        Level,
        Controls,
        Credits,
        Quit
    }
}
