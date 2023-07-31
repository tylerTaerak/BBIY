using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

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
        private Stack<Dictionary<uint, Entities.Entity>> m_moves;

        /* systems */
        private Systems.RulesSystem m_rules;
        private Systems.RuleApplicatorSystem m_apply;
        private Systems.InputSystem m_input;
        private Systems.CollisionSystem m_collision;
        private Systems.RenderAnimatedSystem m_renderSprites;
        private Systems.RenderParticleSystem m_particles;

        /* sounds */
        private SoundEffect m_moveSE;
        private SoundEffect m_winSE;
        private SoundEffect m_changeWinSE;
        private Song m_song;

        private List<uint> m_remove;

        public string Title
        {
            get;
            set;
        }

        public GameState(int levelIndex)
        {
            m_levelIndex = levelIndex;

            m_remove = new List<uint>();
            m_moves = new Stack<Dictionary<uint, Entities.Entity>>();
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            // load level at level index
            var (title, map) = Load.loadLevels()[m_levelIndex];
            Title = title;

            m_rules = new Systems.RulesSystem();
            m_input = new Systems.InputSystem();


            m_particles = new Systems.RenderParticleSystem(map[0].Length);
            m_particles.initialize(graphicsDevice, graphics);

            m_apply = new Systems.RuleApplicatorSystem(m_rules, m_particles);
            m_collision = new Systems.CollisionSystem(m_remove, m_particles);

            m_renderSprites = new Systems.RenderAnimatedSystem(map[0].Length, m_collision);
            m_renderSprites.initialize(graphicsDevice, graphics);

            for (var j=0; j<map.Length/2; j++)
            {
                for (var i=0; i<map[j].Length; i++)
                {
                    AddEntity(map[j].ToCharArray()[i], i, j);
                    AddEntity(map[j+map.Length/2].ToCharArray()[i], i, j);
                }
            }

            // add initial state to move list
            m_moves.Push(Systems.System.Copy(m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision));
        }

        public void loadContent(ContentManager content)
        {

            m_renderSprites.loadContent(content); // load content

            m_moveSE = content.Load<SoundEffect>("sounds/move");

            m_winSE = content.Load<SoundEffect>("sounds/win");
            m_changeWinSE = content.Load<SoundEffect>("sounds/changeWin");

            m_collision.WinSound = m_winSE;
            m_apply.ChangeWin = m_changeWinSE;

            m_song = content.Load<Song>("sounds/soundtrack");
        }

        public void reset(GameTime gameTime)
        {
            goToFirst();

            m_particles.reset();

            MediaPlayer.Play(m_song);
        }

        private void goToFirst()
        {
            // clear list, and add just the initial state back into the move list
            while (m_moves.Count > 1)
            {
                m_moves.Pop();
            }
            var first = m_moves.Pop();
            Systems.System.ReadFromCopy(first, m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision);
            m_moves.Clear();
            m_moves.Push(Systems.System.Copy(m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision));

            m_collision.Win = false;
        }

        public GameStateType processInput(GameTime gameTime)
        {
            if (!m_collision.Win)
            {
                m_input.update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                return GameStateType.MainMenu;
            }

            return GameStateType.Level;
        }

        public void update(GameTime gameTime)
        {
            m_collision.update(gameTime);

            m_rules.update(gameTime);
            m_apply.update(gameTime);

            foreach (var entity in m_remove)
            {

                RemoveEntity(entity);
            }
            m_remove.Clear();

            m_renderSprites.update(gameTime);
            m_particles.update(gameTime);

            if (m_input.NewMove)
            {
                m_moves.Push(Systems.System.Copy(m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision));
                m_input.NewMove = false;

                m_moveSE.Play(); // play a sound for each move
            }

            if (m_input.Undo)
            {
                if (m_moves.Count > 1)
                {
                    m_moves.Pop();
                    var last = m_moves.Peek();
                    Systems.System.ReadFromCopy(last, m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision);
                }
                m_input.Undo = false;
            }


            if (m_input.Reset)
            {
                this.goToFirst();
                m_input.Reset = false;
            }

        }

        public void render(SpriteBatch spriteBatch)
        {
            m_renderSprites.render(spriteBatch);
            m_particles.render(spriteBatch);
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
                    e = Entities.Baba.create(x, y);
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
                    e = Entities.Noun.create("baba", x, y);
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
                m_particles.Add(e);
            }
        }

        private void RemoveEntity(uint id)
        {
            m_rules.Remove(id);
            m_apply.Remove(id);
            m_input.Remove(id);
            m_collision.Remove(id);
            m_renderSprites.Remove(id);
            m_particles.Remove(id);
        }
    }
}
