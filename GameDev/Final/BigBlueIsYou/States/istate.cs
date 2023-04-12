using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    public interface IState
    {
        public void initialize();

        public void loadContent(ContentManager content);

        public void processInput();

        public void update(GameTime gameTime);

        public void render(SpriteBatch spriteBatch);
    }
}
