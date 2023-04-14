using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    public interface IState
    {
        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics);

        public void loadContent(ContentManager content);

        public GameStateType processInput(GameTime gameTime);

        public void update(GameTime gameTime);

        public void render(SpriteBatch spriteBatch);
    }

    public enum GameStateType
    {
        MainMenu,
        Sandbox,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Controls,
        Credits,
        Quit
    }
}
