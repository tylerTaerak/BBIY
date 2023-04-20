using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CS5410.Systems
{
    public class RenderAnimatedSystem : System, IRender
    {
        /* using maps here allows us to dynamically switch which sprites stand for which objects */
        private Dictionary<Components.Objects, (Sprites.AnimatedSprite, Color)> m_objSpriteMap;
        private Dictionary<string, (Sprites.AnimatedSprite, Color)> m_wordSpriteMap;
        private const int m_timer = 500; /* how long to stay in a frame of animation */
        private const int m_pixelsPerCoord = 24; /* how many pixels per space */

        private int m_windowWidth;
        private int m_windowHeight;

        private int m_mapDim;
        private CollisionSystem m_collision;

        private SpriteFont m_font; // for showing instructions at bottom of screen

        public RenderAnimatedSystem(int dim, CollisionSystem collision)
            :base(
                 )
        {
            m_mapDim = dim; // grid size of level
            m_collision = collision; // needed for checking a win
        }

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_windowWidth = graphics.PreferredBackBufferWidth;
            m_windowHeight = graphics.PreferredBackBufferHeight;
        }

        protected override bool IsInterested(Entities.Entity entity)
        {
            // needs to be textured
            if (!entity.ContainsComponent<Components.Sprite>())
            {
                return false;
            }
            // needs to have a position
            if (!entity.ContainsComponent<Components.Position>())
            {
                return false;
            }
            // needs to be either an object or a word
            if (!entity.ContainsComponent<Components.Noun>() && !entity.ContainsComponent<Components.Text>())
            {
                return false;
            }

            return true;
        }

        public void loadContent(ContentManager content)
        {
            m_objSpriteMap = new Dictionary<Components.Objects, (Sprites.AnimatedSprite, Color)>();
            m_wordSpriteMap = new Dictionary<string, (Sprites.AnimatedSprite, Color)>();

            /* normal sprites */
            var bb = content.Load<Texture2D>("images/BigBlue");
            var flag = content.Load<Texture2D>("images/flag");
            var floor = content.Load<Texture2D>("images/floor");
            var grass = content.Load<Texture2D>("images/grass");
            var hedge = content.Load<Texture2D>("images/hedge");
            var lava = content.Load<Texture2D>("images/lava");
            var rock = content.Load<Texture2D>("images/rock");
            var wall = content.Load<Texture2D>("images/wall");
            var water = content.Load<Texture2D>("images/water");

            m_objSpriteMap.Add(Components.Objects.BigBlue, (new Sprites.AnimatedSprite(bb, new int[] {m_timer}), Color.White)); // color's built in to this one
            m_objSpriteMap.Add(Components.Objects.Flag, (new Sprites.AnimatedSprite(flag, new int[] {m_timer, m_timer, m_timer}), Color.Khaki));
            m_objSpriteMap.Add(Components.Objects.Floor, (new Sprites.AnimatedSprite(floor, new int[] {m_timer, m_timer, m_timer}), Color.SaddleBrown));
            m_objSpriteMap.Add(Components.Objects.Grass, (new Sprites.AnimatedSprite(grass, new int[] {m_timer, m_timer, m_timer}), Color.Green));
            m_objSpriteMap.Add(Components.Objects.Hedge, (new Sprites.AnimatedSprite(hedge, new int[] {m_timer, m_timer, m_timer}), Color.DarkGreen));
            m_objSpriteMap.Add(Components.Objects.Lava, (new Sprites.AnimatedSprite(lava, new int[] {m_timer, m_timer, m_timer}), Color.Tomato));
            m_objSpriteMap.Add(Components.Objects.Rock, (new Sprites.AnimatedSprite(rock, new int[] {m_timer, m_timer, m_timer}), Color.Gray));
            m_objSpriteMap.Add(Components.Objects.Wall, (new Sprites.AnimatedSprite(wall, new int[] {m_timer, m_timer, m_timer}), Color.DarkGray));
            m_objSpriteMap.Add(Components.Objects.Water, (new Sprites.AnimatedSprite(water, new int[] {m_timer, m_timer, m_timer}), Color.DarkSlateBlue));


            /* word sprites */
            var wbaba = content.Load<Texture2D>("images/word-baba");
            var wflag = content.Load<Texture2D>("images/word-flag");
            var wis = content.Load<Texture2D>("images/word-is");
            var wkill = content.Load<Texture2D>("images/word-kill");
            var wlava = content.Load<Texture2D>("images/word-lava");
            var wpush = content.Load<Texture2D>("images/word-push");
            var wrock = content.Load<Texture2D>("images/word-rock");
            var wsink = content.Load<Texture2D>("images/word-sink");
            var wstop = content.Load<Texture2D>("images/word-stop");
            var wwall = content.Load<Texture2D>("images/word-wall");
            var wwater = content.Load<Texture2D>("images/word-water");
            var wwin = content.Load<Texture2D>("images/word-win");
            var wyou = content.Load<Texture2D>("images/word-you");

            m_wordSpriteMap.Add("bigblue", (new Sprites.AnimatedSprite(wbaba, new int[] {m_timer, m_timer, m_timer}), Color.Blue));
            m_wordSpriteMap.Add("flag", (new Sprites.AnimatedSprite(wflag, new int[] {m_timer, m_timer, m_timer}), Color.Wheat));
            m_wordSpriteMap.Add("is", (new Sprites.AnimatedSprite(wis, new int[] {m_timer, m_timer, m_timer}), Color.Beige));
            m_wordSpriteMap.Add("defeat", (new Sprites.AnimatedSprite(wkill, new int[] {m_timer, m_timer, m_timer}), Color.Red));
            m_wordSpriteMap.Add("lava", (new Sprites.AnimatedSprite(wlava, new int[] {m_timer, m_timer, m_timer}), Color.DeepPink));
            m_wordSpriteMap.Add("push", (new Sprites.AnimatedSprite(wpush, new int[] {m_timer, m_timer, m_timer}), Color.Orange));
            m_wordSpriteMap.Add("rock", (new Sprites.AnimatedSprite(wrock, new int[] {m_timer, m_timer, m_timer}), Color.LightSlateGray));
            m_wordSpriteMap.Add("sink", (new Sprites.AnimatedSprite(wsink, new int[] {m_timer, m_timer, m_timer}), Color.Blue));
            m_wordSpriteMap.Add("stop", (new Sprites.AnimatedSprite(wstop, new int[] {m_timer, m_timer, m_timer}), Color.DarkRed));
            m_wordSpriteMap.Add("wall", (new Sprites.AnimatedSprite(wwall, new int[] {m_timer, m_timer, m_timer}), Color.DarkGray));
            m_wordSpriteMap.Add("water", (new Sprites.AnimatedSprite(wwater, new int[] {m_timer, m_timer, m_timer}), Color.LightSteelBlue));
            m_wordSpriteMap.Add("win", (new Sprites.AnimatedSprite(wwin, new int[] {m_timer, m_timer, m_timer}), Color.Gold));
            m_wordSpriteMap.Add("you", (new Sprites.AnimatedSprite(wyou, new int[] {m_timer, m_timer, m_timer}), Color.Lime));

            m_font = content.Load<SpriteFont>("fonts/Main");
        }

        public override void update(GameTime gameTime)
        {
            // update all sprites together, so they stay in sync
            foreach (var sprite in m_objSpriteMap.Values)
            {
                sprite.Item1.update(gameTime);
            }

            foreach (var sprite in m_wordSpriteMap.Values)
            {
                sprite.Item1.update(gameTime);
            }
        }

        public void render(SpriteBatch spriteBatch)
        {
            var xOffset = m_windowWidth / 2 - (m_mapDim * 40) / 2;
            var yOffset = m_windowHeight / 2 - (m_mapDim * 40) / 2;

            List<Entities.Entity> bottomQueue = new List<Entities.Entity>();
            List<Entities.Entity> middleQueue = new List<Entities.Entity>();
            List<Entities.Entity> topQueue = new List<Entities.Entity>();

            foreach (var entity in m_entities.Values)
            {
                var rend = entity.GetComponent<Components.Sprite>();
                switch(rend.Layer)
                {
                    case Components.RenderLayer.Bottom:
                        bottomQueue.Add(entity);
                        break;
                    case Components.RenderLayer.Middle:
                        middleQueue.Add(entity);
                        break;
                    case Components.RenderLayer.Top:
                        topQueue.Add(entity);
                        break;
                }
            }


            foreach(Entities.Entity entity in bottomQueue)
            {
                Sprites.AnimatedSprite sprite;
                Color color;
                if (entity.ContainsComponent<Components.Noun>())
                {
                    var noun = entity.GetComponent<Components.Noun>();
                    sprite = m_objSpriteMap[noun.Object].Item1;
                    color = m_objSpriteMap[noun.Object].Item2;
                }
                else
                {
                    var text = entity.GetComponent<Components.Text>();
                    sprite = m_wordSpriteMap[text.Word].Item1;
                    color = m_wordSpriteMap[text.Word].Item2;
                }


                var pos = entity.GetComponent<Components.Position>();
                var (x, y) = pos.CurrentPosition;

                sprite.render(spriteBatch, xOffset, yOffset, x, y, color);
            }

            foreach(Entities.Entity entity in middleQueue)
            {
                Sprites.AnimatedSprite sprite;
                Color color;
                if (entity.ContainsComponent<Components.Noun>())
                {
                    var noun = entity.GetComponent<Components.Noun>();
                    sprite = m_objSpriteMap[noun.Object].Item1;
                    color = m_objSpriteMap[noun.Object].Item2;
                }
                else
                {
                    var text = entity.GetComponent<Components.Text>();
                    sprite = m_wordSpriteMap[text.Word].Item1;
                    color = m_wordSpriteMap[text.Word].Item2;
                }


                var pos = entity.GetComponent<Components.Position>();
                var (x, y) = pos.CurrentPosition;

                sprite.render(spriteBatch, xOffset, yOffset, x, y, color);
            }

            foreach(Entities.Entity entity in topQueue)
            {
                Sprites.AnimatedSprite sprite;
                Color color;
                if (entity.ContainsComponent<Components.Noun>())
                {
                    var noun = entity.GetComponent<Components.Noun>();
                    sprite = m_objSpriteMap[noun.Object].Item1;
                    color = m_objSpriteMap[noun.Object].Item2;
                }
                else
                {
                    var text = entity.GetComponent<Components.Text>();
                    sprite = m_wordSpriteMap[text.Word].Item1;
                    color = m_wordSpriteMap[text.Word].Item2;
                }


                var pos = entity.GetComponent<Components.Position>();
                var (x, y) = pos.CurrentPosition;

                sprite.render(spriteBatch, xOffset, yOffset, x, y, color);
            }


            if (!m_collision.Win)
            {
                // instruction for undo
                Keys undo = InputSystem.s_keyCommands.FirstOrDefault(x=> x.Value == Commands.Undo).Key; // ugh
                string undoKey = Enum.GetName(typeof(Keys), undo);
                string undoInst = $"{undoKey}: Undo";
                spriteBatch.DrawString(
                        m_font,
                        undoInst,
                        new Vector2(
                            m_windowWidth / 4 - m_font.MeasureString(undoInst).X/2,
                            m_windowHeight - m_windowHeight / 20
                            ),
                        Color.White
                        );
            }

            // instruction to leave to menu
            string menuKey = Enum.GetName(typeof(Keys), Keys.Escape);
            string menuInst = $"{menuKey}: Return to Menu";
            spriteBatch.DrawString(
                    m_font,
                    menuInst,
                    new Vector2(
                        m_windowWidth / 2 - m_font.MeasureString(menuInst).X/2,
                        m_windowHeight - m_windowHeight / 20
                        ),
                    Color.White
                    );


            if (!m_collision.Win)
            {
                // instruction to reset
                Keys reset = InputSystem.s_keyCommands.FirstOrDefault(x=> x.Value == Commands.Reset).Key; // ugh
                string resetKey = Enum.GetName(typeof(Keys), reset);
                string resetInst = $"{resetKey}: Reset";
                spriteBatch.DrawString(
                        m_font,
                        resetInst,
                        new Vector2(
                            3 * m_windowWidth / 4 - m_font.MeasureString(resetInst).X/2,
                            m_windowHeight - m_windowHeight / 20
                            ),
                        Color.White
                        );
            }

        }
    }
}
