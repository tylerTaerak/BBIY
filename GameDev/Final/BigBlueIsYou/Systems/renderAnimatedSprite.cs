using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CS5410.Systems
{
    public class RenderAnimatedSystem : System, IRender
    {
        /* using maps here allows us to dynamically switch which sprites stand for which objects */
        private Dictionary<Components.Objects, (Sprites.AnimatedSprite, Color)> m_objSpriteMap;
        private Dictionary<string, (Sprites.AnimatedSprite, Color)> m_wordSpriteMap;
        private const int m_timer = 500; /* how long to stay in a frame of animation */
        private const int m_pixelsPerCoord = 24; /* how many pixels per space */

        public RenderAnimatedSystem()
            :base(
                 )
        {
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

            m_objSpriteMap.Add(Components.Objects.BigBlue, (new Sprites.AnimatedSprite(bb, new int[] {m_timer}), Color.White));
            m_objSpriteMap.Add(Components.Objects.Flag, (new Sprites.AnimatedSprite(flag, new int[] {m_timer, m_timer, m_timer}), Color.Beige));
            m_objSpriteMap.Add(Components.Objects.Floor, (new Sprites.AnimatedSprite(floor, new int[] {m_timer, m_timer, m_timer}), Color.DarkGray));
            m_objSpriteMap.Add(Components.Objects.Grass, (new Sprites.AnimatedSprite(grass, new int[] {m_timer, m_timer, m_timer}), Color.Green));
            m_objSpriteMap.Add(Components.Objects.Hedge, (new Sprites.AnimatedSprite(hedge, new int[] {m_timer, m_timer, m_timer}), Color.DarkGreen));
            m_objSpriteMap.Add(Components.Objects.Lava, (new Sprites.AnimatedSprite(lava, new int[] {m_timer, m_timer, m_timer}), Color.Red));
            m_objSpriteMap.Add(Components.Objects.Rock, (new Sprites.AnimatedSprite(rock, new int[] {m_timer, m_timer, m_timer}), Color.Gray));
            m_objSpriteMap.Add(Components.Objects.Wall, (new Sprites.AnimatedSprite(wall, new int[] {m_timer, m_timer, m_timer}), Color.LightGray));
            m_objSpriteMap.Add(Components.Objects.Water, (new Sprites.AnimatedSprite(water, new int[] {m_timer, m_timer, m_timer}), Color.Blue));


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

            m_wordSpriteMap.Add("bigblue", (new Sprites.AnimatedSprite(wbaba, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("flag", (new Sprites.AnimatedSprite(wflag, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("is", (new Sprites.AnimatedSprite(wis, new int[] {m_timer, m_timer, m_timer}), Color.Yellow));
            m_wordSpriteMap.Add("defeat", (new Sprites.AnimatedSprite(wkill, new int[] {m_timer, m_timer, m_timer}), Color.Red));
            m_wordSpriteMap.Add("lava", (new Sprites.AnimatedSprite(wlava, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("push", (new Sprites.AnimatedSprite(wpush, new int[] {m_timer, m_timer, m_timer}), Color.Orange));
            m_wordSpriteMap.Add("rock", (new Sprites.AnimatedSprite(wrock, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("sink", (new Sprites.AnimatedSprite(wsink, new int[] {m_timer, m_timer, m_timer}), Color.Cyan));
            m_wordSpriteMap.Add("stop", (new Sprites.AnimatedSprite(wstop, new int[] {m_timer, m_timer, m_timer}), Color.DarkRed));
            m_wordSpriteMap.Add("wall", (new Sprites.AnimatedSprite(wwall, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("water", (new Sprites.AnimatedSprite(wwater, new int[] {m_timer, m_timer, m_timer}), Color.White));
            m_wordSpriteMap.Add("win", (new Sprites.AnimatedSprite(wwin, new int[] {m_timer, m_timer, m_timer}), Color.Gold));
            m_wordSpriteMap.Add("you", (new Sprites.AnimatedSprite(wyou, new int[] {m_timer, m_timer, m_timer}), Color.Lime));
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
            foreach(Entities.Entity entity in m_entities.Values)
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

                sprite.render(spriteBatch, x, y, color);
            }
        }
    }
}
