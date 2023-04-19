using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace CS5410.Systems
{
    public class RenderParticleSystem : System, IRender
    {
        internal class Particle
        {
            private Texture2D m_tex;

            /// width and height (square) of particle
            internal int Dim
            {
                get;
                set;
            }

            /// X/Y position of particle in pixels(ish)
            internal Vector2 Position
            {
                get;
                set;
            }

            /// X/Y speed of particle in pixels per millisecond
            internal Vector2 Speed
            {
                get;
                set;
            }

            /// lifetime of particle in milliseconds
            internal TimeSpan Lifetime
            {
                get;
                set;
            }

            internal Particle(
                    Texture2D texture,
                    Vector2 position,
                    Vector2 speed,
                    int lifetimeMs
                    )
            {
                m_tex = texture;
                Position = position;
                Speed = speed;
                Lifetime = new TimeSpan(0, 0, 0, lifetimeMs, 0);
            }

            internal void render(SpriteBatch spriteBatch)
            {
                // render individual particle
                spriteBatch.Draw(
                        m_tex,
                        new Rectangle(
                            (int)Position.X,
                            (int)Position.Y,
                            Dim,
                            Dim
                            ),
                        Color.White
                        );
            }
        }

        private List<Particle> m_particles;

        public void playerDeath(int x, int y)
        {
            // add pixels based on player death
        }

        public void changeIsYou(int x, int y)
        {
            // add pixels for changed 'IS YOU' rule
        }

        public void playerWin(int x, int y)
        {
            // add pixels for player win
        }

        public override void update(GameTime gameTime)
        {
            foreach (Particle p in m_particles)
            {
                // move particle using speed
                p.Position += Vector2.Multiply(p.Speed, (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                // reduce lifetime of particle
                p.Lifetime -= gameTime.ElapsedGameTime;

                // remove particle if dead
                if (p.Lifetime <= TimeSpan.Zero)
                {
                    m_particles.Remove(p);
                }
            }
        }

        public void render(SpriteBatch spriteBatch)
        {
            foreach (Particle p in m_particles)
            {
                p.render(spriteBatch);
            }
        }
    }
}
