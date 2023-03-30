using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    /* Nouns: BigBlue, Wall, Rock, Flag Lava, Water */
    /* Verbs: Is */
    /* Attributes: Stop, Push, Win, Sink, Kill/Defeat */

    // instead of this approach, I think I ought to do a composition
    // style approach, where each square in the grid is its own object
    //
    // For this, each object would have a noun and a list of attributes
    // (since an object can't be more than one thing)

    public interface IWord
    {
        Point coord
        {
            get;
            set;
        }
    }

    /** Nouns - do we actually need classes for this? or can it just be strings?*/

    interface INoun : IWord
    {
        static Texture2D bb;
        static Texture2D wall;
        static Texture2D rock;
        static Texture2D flag;
        static Texture2D lava;
        static Texture2D water;

        Texture2D Texture
        {
            get;
            set;
        }
    }

    class BigBlue : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Wall : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Rock : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Flag : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Lava : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Water : INoun
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    /** Attributes */

    interface IAttr : IWord
    {
        Texture2D Texture
        {
            get;
            set;
        }
    }

    class Stop : IAttr
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Push : IAttr
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Win : IAttr
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Sink : IAttr
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    class Kill : IAttr
    {
        public Point coord
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
    }

    /** Verbs (is */

    public class Is : IWord
    {
        public Point coord
        {
            get;
            set;
        }
    }
}
