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
            private int m_initialLifetime;

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
                m_initialLifetime = lifetimeMs;
                Lifetime = TimeSpan.FromMilliseconds(lifetimeMs);
            }

            internal void render(SpriteBatch spriteBatch)
            {
                // render individual particle

                // fade out the particle halfway through lifetime
                if (Lifetime.TotalMilliseconds < m_initialLifetime / 2)
                {
                    float alpha = (float)(Lifetime.TotalMilliseconds/(m_initialLifetime/2));

                    spriteBatch.Draw(
                            m_tex,
                            new Rectangle(
                                (int)Position.X,
                                (int)Position.Y,
                                Dim,
                                Dim
                                ),
                            Color.White * alpha
                            );

                    return;
                }
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
        private int m_nodeWidth;
        private int m_mapDim;

        private int m_windowWidth;
        private int m_windowHeight;

        private GraphicsDevice m_graphics;

        public RenderParticleSystem(int mapDim)
            : base(
                    typeof(Components.Noun),
                    typeof(Components.Position)
                  )
        {
            m_particles = new List<Particle>();

            m_mapDim = mapDim;

            m_nodeWidth = 40; // 40 pixels per space
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_windowWidth = graphics.PreferredBackBufferWidth;
            m_windowHeight = graphics.PreferredBackBufferHeight;

            m_graphics = graphicsDevice;
        }

        public void reset()
        {
            m_particles.Clear();
        }

        private void addParticlesAroundSquare(int x, int y, Color[] colors)
        {
            var xOffset = m_windowWidth / 2 - (m_mapDim * 40) / 2;
            var yOffset = m_windowHeight / 2 - (m_mapDim * 40) / 2;
            // upper left corner of square
            int xPixels = xOffset + m_nodeWidth * x;
            int yPixels = yOffset + m_nodeWidth * y;

            int numPixels = 60;
            int numPixelsPerSide = numPixels / 4; // 4 sides on a square

            var centerX = m_nodeWidth/2 + xPixels;
            var centerY = m_nodeWidth/2 + yPixels;

            float particleSpeed = (float)0.12; // px per ms
            int particleWidth = 4; // px
            int particleLifetime = 400; // ms

            Random rand = new Random();
            List<Texture2D> textures = new List<Texture2D>();

            foreach(Color c in colors)
            {
                Texture2D tex = new Texture2D(m_graphics, 1, 1);
                tex.SetData(new[] {c});
                textures.Add(tex);
            }

            for (var i=0; i<numPixelsPerSide; i++)
            {
                int partX = xPixels + rand.Next(m_nodeWidth);
                int partY = yPixels;

                // speed is based on direction from center
                var diffX = partX - centerX;
                var diffY = partY - centerY;

                Vector2 speed = Vector2.Normalize(new Vector2(diffX, diffY));
                speed = Vector2.Multiply(speed, particleSpeed);

                Texture2D tex = textures[rand.Next()%textures.Count];

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

                Texture2D tex = textures[rand.Next()%textures.Count];

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

                Texture2D tex = textures[rand.Next()%textures.Count];

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

                Texture2D tex = textures[rand.Next()%textures.Count];

                m_particles.Add(new Particle(tex, new Vector2(partX, partY), speed, particleWidth, particleLifetime));
            }
        }

        public void playerDeath(int x, int y)
        {
            // add particles based on player death
            // do this by adding particles around the square that is kill

            Color[] colors = new Color[]{Color.DarkBlue, Color.White, Color.LightBlue};

            addParticlesAroundSquare(x, y, colors);
        }

        public void changeIsYou(Components.Objects newYou)
        {
            // add particles for changed 'IS YOU' rule
            // do this by adding particles around the square that is now YOU

            foreach (var entity in m_entities.Values)
            {
                var noun = entity.GetComponent<Components.Noun>();

                if (noun.Object == newYou)
                {
                    var pos = entity.GetComponent<Components.Position>().CurrentPosition;

                    Color[] colors = new Color[]{Color.White, Color.LightGreen, Color.Cyan, Color.LightBlue, Color.Aqua};

                    addParticlesAroundSquare(pos.Item1, pos.Item2, colors);
                }
            }
        }

        public void changeIsWin(Components.Objects newWin)
        {
            // add particles for changed 'IS WIN' rule
            // do this by adding particles around the square that is now WIN

            foreach (var entity in m_entities.Values)
            {
                var noun = entity.GetComponent<Components.Noun>();

                if (noun.Object == newWin)
                {
                    var pos = entity.GetComponent<Components.Position>().CurrentPosition;

                    Color[] colors = new Color[]{Color.White, Color.Gold, Color.Yellow, Color.Orange, Color.LightPink};

                    addParticlesAroundSquare(pos.Item1, pos.Item2, colors);
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
