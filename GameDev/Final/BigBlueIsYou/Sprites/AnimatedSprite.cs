using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace CS5410.Sprites
{
    public class AnimatedSprite
    {
        private Texture2D m_spriteSheet;
        private int[] m_spriteTime;

        private TimeSpan m_animationTime;
        private int m_subImageIndex;
        private int m_subImageWidth;

        private const int idxPerPixel = 20;

        public AnimatedSprite(Texture2D spriteSheet, int[] spriteTime)
        {
            m_spriteSheet = spriteSheet;
            m_spriteTime = spriteTime;

            m_subImageWidth = spriteSheet.Width / spriteTime.Length;
        }

        public void update(GameTime gameTime)
        {
            m_animationTime += gameTime.ElapsedGameTime;
            if (m_animationTime.TotalMilliseconds >= m_spriteTime[m_subImageIndex])
            {
                m_animationTime -= TimeSpan.FromMilliseconds(m_spriteTime[m_subImageIndex++]);
                m_subImageIndex = m_subImageIndex % m_spriteTime.Length;
            }
        }

        public void render(SpriteBatch spritebatch, int x, int y, Color color)
        {
            // I don't think we need the overloaded one for this game
            spritebatch.Draw(
                    m_spriteSheet,
                    new Rectangle(idxPerPixel * x, idxPerPixel * y, idxPerPixel, idxPerPixel), // coordinates
                    new Rectangle(m_subImageIndex * m_subImageWidth, 0, m_subImageWidth, m_spriteSheet.Height), // subtexture
                    color
                    );
        }
    }
}
