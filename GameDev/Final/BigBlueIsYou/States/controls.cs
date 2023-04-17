using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CS5410.States
{
    public class ControlsState : IState
    {
        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
        }

        public void loadContent(ContentManager content)
        {
        }

        public void reset(GameTime gameTime)
        {
        }

        public GameStateType processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateType.MainMenu;
            }
            return GameStateType.Controls;
        }

        public void update(GameTime gameTime)
        {
        }

        public void render(SpriteBatch spriteBatch)
        {
        }
    }
}
