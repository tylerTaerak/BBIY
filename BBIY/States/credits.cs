using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    public class CreditState : IState
    {
        private string m_creditMsg;

        private SpriteFont m_mainFont;
        private SpriteFont m_largeFont;

        private int m_windowWidth;
        private int m_windowHeight;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_creditMsg = @"
                This game is a tribute to Baba is You, written by Tyler Conley for his Game Dev Final Project.

                He would like to thank Dr. Mathias for teaching the material so well. Sound effects and background

                music were provided by ZapSplat.";

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
            /* no reset logic needed */
        }

        public GameStateType processInput(GameTime gameTime)
        {
            foreach (Keys k in Keyboard.GetState().GetPressedKeys())
            {
                switch(k)
                {
                    case Keys.Escape:
                        return GameStateType.MainMenu;
                }
            }

            return GameStateType.Credits;
        }

        public void update(GameTime gameTime)
        {
            /* no updating needed */
        }

        public void render(SpriteBatch spriteBatch)
        {
            float spacing = 40; // 40 pixels between each menu option - 2x between title and first option

            spriteBatch.DrawString(
                    m_largeFont,
                    "Credits",
                    new Vector2(
                        m_windowWidth / 2 - m_largeFont.MeasureString("Credits").X/2,
                        spacing),
                    Color.White
                    );

            spacing += m_largeFont.MeasureString("Credits").Y + 80;

            spriteBatch.DrawString(
                    m_mainFont,
                    m_creditMsg,
                    new Vector2(
                        m_windowWidth / 2 - m_mainFont.MeasureString(m_creditMsg).X/2,
                        spacing
                        ),
                    Color.White
                    );
        }
    }
}
