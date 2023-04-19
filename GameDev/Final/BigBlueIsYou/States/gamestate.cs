using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CS5410.States
{
    /**
     *  NOTE: Something cool to add would be some help
     *  items shown on ESC, including pressing ESC again
     *  to return to the main menu
     */
    public class GameState : IState
    {
        private int m_levelIndex;
        private GameStateType m_level;

        /* systems */
        private Systems.RulesSystem m_rules;
        private Systems.RuleApplicatorSystem m_apply;
        private Systems.InputSystem m_input;
        private Systems.CollisionSystem m_collision;
        private Systems.RenderAnimatedSystem m_renderSprites;

        public GameState(int levelIndex, GameStateType levelId)
        {
            m_levelIndex = levelIndex;
            m_level = levelId;
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_rules = new Systems.RulesSystem();
            m_apply = new Systems.RuleApplicatorSystem(m_rules);
            m_input = new Systems.InputSystem();
            m_collision = new Systems.CollisionSystem();
            m_renderSprites = new Systems.RenderAnimatedSystem();

            // load level at level index
            var (title, map) = Load.loadLevels(m_levelIndex);

            for (var j=0; j<map.Length/2; j++)
            {
                for (var i=0; i<map[j].Length; i++)
                {
                    AddEntity(map[j].ToCharArray()[i], i, j);
                    AddEntity(map[j+map.Length/2].ToCharArray()[i], i, j);
                }
            }
        }

        public void loadContent(ContentManager content)
        {

            m_renderSprites.loadContent(content); // load content
        }

        public void reset(GameTime gameTime)
        {
        }

        public GameStateType processInput(GameTime gameTime)
        {
            return m_level;
        }

        public void update(GameTime gameTime)
        {
            m_input.update(gameTime);
            m_collision.update(gameTime);

            m_rules.update(gameTime);
            m_apply.update(gameTime);

            m_renderSprites.update(gameTime);
        }

        public void render(SpriteBatch spriteBatch)
        {
            m_renderSprites.render(spriteBatch);
        }

        private void AddEntity(char c, int x, int y)
        {
            Entities.Entity e = null;
            switch (c)
            {
                case 'w':
                    e = Entities.Wall.create(x, y);
                    break;
                case 'r':
                    e = Entities.Rock.create(x, y);
                    break;
                case 'f':
                    e = Entities.Flag.create(x, y);
                    break;
                case 'b':
                    e = Entities.BigBlue.create(x, y);
                    break;
                case 'l':
                    e = Entities.Floor.create(x, y);
                    break;
                case 'g':
                    e = Entities.Grass.create(x, y);
                    break;
                case 'a':
                    e = Entities.Water.create(x, y);
                    break;
                case 'v':
                    e = Entities.Lava.create(x, y);
                    break;
                case 'h':
                    e = Entities.Hedge.create(x, y);
                    break;
                case 'W':
                    e = Entities.Noun.create("wall", x, y);
                    break;
                case 'R':
                    e = Entities.Noun.create("rock", x, y);
                    break;
                case 'F':
                    e = Entities.Noun.create("flag", x, y);
                    break;
                case 'B':
                    e = Entities.Noun.create("bigblue", x, y);
                    break;
                case 'I':
                    e = Entities.Verb.create("is", x, y);
                    break;
                case 'S':
                    e = Entities.Noun.create("stop", x, y);
                    break;
                case 'P':
                    e = Entities.Noun.create("push", x, y);
                    break;
                case 'V':
                    e = Entities.Noun.create("lava", x, y);
                    break;
                case 'A':
                    e = Entities.Noun.create("water", x, y);
                    break;
                case 'Y':
                    e = Entities.Noun.create("you", x, y);
                    break;
                case 'X':
                    e = Entities.Noun.create("win", x, y);
                    break;
                case 'N':
                    e = Entities.Noun.create("sink", x, y);
                    break;
                case 'K':
                    e = Entities.Noun.create("defeat", x, y);
                    break;
            }

            if (e != null)
            {
                m_rules.Add(e);
                m_apply.Add(e);
                m_input.Add(e);
                m_collision.Add(e);
                m_renderSprites.Add(e);
            }


        }
    }
}
