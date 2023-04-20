using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
        private Stack<Dictionary<uint, Entities.Entity>> m_moves;

        /* systems */
        private Systems.RulesSystem m_rules;
        private Systems.RuleApplicatorSystem m_apply;
        private Systems.InputSystem m_input;
        private Systems.CollisionSystem m_collision;
        private Systems.RenderAnimatedSystem m_renderSprites;
        private Systems.RenderParticleSystem m_particles;

        private TimeSpan m_helpMessage;

        private List<uint> m_remove;

        public GameState(int levelIndex, GameStateType levelId)
        {
            m_levelIndex = levelIndex;
            m_level = levelId;

            m_remove = new List<uint>();
            m_moves = new Stack<Dictionary<uint, Entities.Entity>>();
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            // load level at level index
            var (title, map) = Load.loadLevels(m_levelIndex);

            m_rules = new Systems.RulesSystem();
            m_input = new Systems.InputSystem();

            m_renderSprites = new Systems.RenderAnimatedSystem(map[0].Length);
            m_renderSprites.initialize(graphicsDevice, graphics);

            m_particles = new Systems.RenderParticleSystem(map[0].Length);
            m_particles.initialize(graphicsDevice, graphics);

            m_apply = new Systems.RuleApplicatorSystem(m_rules, m_particles);
            m_collision = new Systems.CollisionSystem(m_remove, m_particles);

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
        }

        public void reset(GameTime gameTime)
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
        }

        public GameStateType processInput(GameTime gameTime)
        {
            m_input.update(gameTime);

            if (m_input.ReturnToMenu)
            {
                m_input.ReturnToMenu = false;
                return GameStateType.MainMenu;
            }

            return m_level;
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
            }

            if (m_input.Undo)
            {
                if (m_moves.Count > 1)
                {
                    m_moves.Pop();
                }
                var last = m_moves.Peek();
                Systems.System.ReadFromCopy(last, m_rules, m_input, m_renderSprites, m_particles, m_apply, m_collision);
                m_input.Undo = false;
            }


            if (m_input.Reset)
            {
                this.reset(gameTime);
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
