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
                    int width,
                    int lifetimeMs
                    )
            {
                m_tex = texture;
                Position = position;
                Speed = speed;
                Dim = width;
                Lifetime = TimeSpan.FromMilliseconds(lifetimeMs);
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
        private int m_offsetPixelsX;
        private int m_offsetPixelsY;
        private int m_nodeWidth;

        private int m_windowWidth;
        private int m_windowHeight;

        private GraphicsDevice m_graphics;

        public RenderParticleSystem(int offsetX, int offsetY)
            : base(
                    typeof(Components.Noun),
                    typeof(Components.Position)
                  )
        {
            m_particles = new List<Particle>();

            m_offsetPixelsX = offsetX;
            m_offsetPixelsY = offsetY;

            m_nodeWidth = 20; // 20 pixels per space
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_windowWidth = graphics.PreferredBackBufferWidth;
            m_windowHeight = graphics.PreferredBackBufferHeight;

            m_graphics = graphicsDevice;
        }

        private void addParticlesAroundSquare(int x, int y, Texture2D tex)
        {
            // upper left corner of square
            int xPixels = m_offsetPixelsX + m_nodeWidth * x;
            int yPixels = m_offsetPixelsY + m_nodeWidth * y;

            int numPixels = 100;
            int numPixelsPerSide = numPixels / 4; // 4 sides on a square

            var centerX = m_nodeWidth/2 + xPixels;
            var centerY = m_nodeWidth/2 + yPixels;

            float particleSpeed = (float)0.75; // px per ms
            int particleWidth = 5; // px
            int particleLifetime = 100; // ms

            Random rand = new Random();

            for (var i=0; i<numPixelsPerSide; i++)
            {
                int partX = xPixels + rand.Next(m_nodeWidth);
                int partY = yPixels;

                // speed is based on direction from center
                var diffX = partX - centerX;
                var diffY = partY - centerY;

                Vector2 speed = Vector2.Normalize(new Vector2(diffX, diffY));
                speed = Vector2.Multiply(speed, particleSpeed);

                m_particles.Add(new Particle(tex, new Vector2(partX, partY), speed, particleWidth, particleLifetime));
            }

            for (var i=0; i<numPixelsPerSide; i++)
            {
                int partX = xPixels;
                int partY = yPixels + rand.Next(m_nodeWidth);

                // speed is based on direction from center
                var diffX = partX - centerX;
                var diffY = partY - centerY;

                Vector2 speed = Vector2.Normalize(new Vector2(diffX, diffY));
                speed = Vector2.Multiply(speed, particleSpeed);

                m_particles.Add(new Particle(tex, new Vector2(partX, partY), speed, particleWidth, particleLifetime));
            }

            for (var i=0; i<numPixelsPerSide; i++)
            {
                int partX = xPixels + rand.Next(m_nodeWidth);
                int partY = yPixels + m_nodeWidth;

                // speed is based on direction from center
                var diffX = partX - centerX;
                var diffY = partY - centerY;

                Vector2 speed = Vector2.Normalize(new Vector2(diffX, diffY));
                speed = Vector2.Multiply(speed, particleSpeed);

                m_particles.Add(new Particle(tex, new Vector2(partX, partY), speed, particleWidth, particleLifetime));
            }

            for (var i=0; i<numPixelsPerSide; i++)
            {
                int partX = xPixels + m_nodeWidth;
                int partY = yPixels + rand.Next(m_nodeWidth);

                // speed is based on direction from center
                var diffX = partX - centerX;
                var diffY = partY - centerY;

                Vector2 speed = Vector2.Normalize(new Vector2(diffX, diffY));
                speed = Vector2.Multiply(speed, particleSpeed);

                m_particles.Add(new Particle(tex, new Vector2(partX, partY), speed, particleWidth, particleLifetime));
            }
        }

        public void playerDeath(int x, int y)
        {
            // add particles based on player death
            // do this by adding particles around the square that is kill

            Texture2D tex = new Texture2D(m_graphics, 1, 1);
            tex.SetData(new[] {Color.Red});

            addParticlesAroundSquare(x, y, tex);
        }

        public void changeIsYou(Components.Objects newYou)
        {
            // add particles for changed 'IS YOU' rule
            // do this by adding particles around the square that is now YOU

            Texture2D tex = new Texture2D(m_graphics, 1, 1);
            tex.SetData(new[] {Color.White});

            foreach (var entity in m_entities.Values)
            {
                var noun = entity.GetComponent<Components.Noun>();

                if (noun.Object == newYou)
                {
                    var pos = entity.GetComponent<Components.Position>().CurrentPosition;

                    addParticlesAroundSquare(pos.Item1, pos.Item2, tex);
                }
            }
        }

        public void changeIsWin(Components.Objects newWin)
        {
            // add particles for changed 'IS WIN' rule
            // do this by adding particles around the square that is now WIN

            Texture2D tex = new Texture2D(m_graphics, 1, 1);
            tex.SetData(new[] {Color.Gold});

            foreach (var entity in m_entities.Values)
            {
                var noun = entity.GetComponent<Components.Noun>();

                if (noun.Object == newWin)
                {
                    var pos = entity.GetComponent<Components.Position>().CurrentPosition;

                    addParticlesAroundSquare(pos.Item1, pos.Item2, tex);
                }
            }
        }

        public void playerWin()
        {
            // add particles for player win
            // Do this by adding fast, multicolored particles in the bottom corners
            // of the (game ? screen -> not sure yet)
            // r = rand.Next(256), g = rand.Next(256), b = rand.Next(256)

            int numParticles = 1000;
            int lifetimeMs = 2000;

            Random rand = new Random();

            // generate left-side particles
            for (var i=0; i<numParticles; i++)
            {
                // direction is between pi/6 and pi/3
                var dir = rand.NextDouble() * (Math.PI/3 - Math.PI/6) + Math.PI/6; // angle in radians
                Vector2 dirVector = new Vector2(
                        (float)Math.Cos(dir),
                        -(float)Math.Sin(dir)
                        );

                float speedMagnitude = rand.NextSingle() * 5;

                dirVector = Vector2.Multiply(dirVector, speedMagnitude);

                // size is between 5 and 15 pixels
                var size = rand.Next(10) + 5;

                // texture is a random color
                Texture2D tex = new Texture2D(m_graphics, 1, 1);
                tex.SetData(new[] {new Color(rand.Next(256), rand.Next(256), rand.Next(256))});
                /*
            internal Particle(
                    Texture2D texture,
                    Vector2 position,
                    Vector2 speed,
                    int width,
                    int lifetimeMs
                    )
                 */

                m_particles.Add(new Particle(tex, new Vector2(0, m_windowHeight), dirVector, size, lifetimeMs));
            }


            // generate right-side particles
            for (var i=0; i<numParticles; i++)
            {
                // direction is between 2pi/3 and 5pi/6
                var dir = rand.NextDouble() * (5*Math.PI/6 - 2*Math.PI/3) + 2*Math.PI/3; // angle in radians
                Vector2 dirVector = new Vector2(
                        (float)Math.Cos(dir),
                        -(float)Math.Sin(dir)
                        );

                float speedMagnitude = rand.NextSingle() * 5;

                dirVector = Vector2.Multiply(dirVector, speedMagnitude);

                // size is between 5 and 15 pixels
                var size = rand.Next(10) + 5;

                // texture is a random color
                Texture2D tex = new Texture2D(m_graphics, 1, 1);
                tex.SetData(new[] {new Color(rand.Next(256), rand.Next(256), rand.Next(256))});
                /*
            internal Particle(
                    Texture2D texture,
                    Vector2 position,
                    Vector2 speed,
                    int width,
                    int lifetimeMs
                    )
                 */

                m_particles.Add(new Particle(tex, new Vector2(m_windowWidth, m_windowHeight), dirVector, size, lifetimeMs));
            }
        }

        public override void update(GameTime gameTime)
        {
            List<Particle> toRemove = new List<Particle>();

            foreach (Particle p in m_particles)
            {
                // move particle using speed
                p.Position += Vector2.Multiply(p.Speed, (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                // reduce lifetime of particle
                p.Lifetime -= gameTime.ElapsedGameTime;

                // remove particle if dead
                if (p.Lifetime <= TimeSpan.Zero)
                {
                    toRemove.Add(p);
                }
            }

            foreach (Particle p in toRemove)
            {
                m_particles.Remove(p);
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
