using System.Collections.Generic;
using System.Linq;

namespace CS5410
{
    class LevelMap
    {
        /* interface for map objects to inherit from */
        public  interface IMapObj
        {
            public (int, int) Coord
            {
                get;
                set;
            }
        }

        /* Immutable Word class -can't add any rules to words */
        private class Word : IMapObj
        {
            private List<string> m_attr;

            Word(string text)
            {
                Text = text;
                m_attr = new List<string>();
                m_attr.Add("push"); // words are only pushable
            }

            public (int, int) Coord
            {
                get;
                set;
            }

            public string Text
            {
                get;
                set;
            }

            /* false is Attribute; true is Object */
            public bool IsNoun
            {
                get;
                set;
            }
        }

        /* Everything else - can have rules applied to it */
        private class MapObject : IMapObj
        {
            public (int, int) Coord
            {
                get;
                set;
            }

            public string Noun
            {
                get;
                set;
            }
        }

        private Dictionary<string, List<IMapObj>> m_objects;
        private Dictionary<string, List<string>> m_attributes;
        private List<IMapObj> m_map;
        private int xCount;
        private int yCount;

        public LevelMap(
                List<List<string>> stringList
                )
        {
            Win = false;
        }

        public bool Win
        {
            get;
            set;
        }

        public IMapObj this[int x, int y]
        {
            get
            {
                return m_map.FirstOrDefault(obj => obj.Coord.Item1 == x && obj.Coord.Item2 == y);
            }
            set
            {
                value.Coord = (x, y);
            }
        }

        public void move(string direction)
        {
            int xMax = xCount - 1; // maximum x value
            int yMax = yCount - 1; // maximum y value

            foreach(MapObject m in m_objects["you"])
            {
                int x = m.Coord.Item1; // current x position
                int y = m.Coord.Item2; // current y position

                // move in a direction
                switch (direction)
                {
                    case "up":
                        if (y > 0)
                        {
                            y--;
                        }
                        break;
                    case "down":
                        if (y < yMax)
                        {
                            y++;
                        }
                        break;
                    case "left":
                        if (x > 0)
                        {
                            x--;
                        }
                        break;
                    case "right":
                        if (x < xMax)
                        {
                            x++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private bool Is(IMapObj obj, string attr)
        {
            return true;
        }

        private void rule(Word node1, Word node2, Word node3)
        {
            if (node1.IsNoun && node2.Text == "is")
            {
                // then make a new rule
                if (node3.IsNoun)
                {
                    // make all objects of node1 into node3
                    m_objects[node3.Text].AddRange(m_objects[node1.Text]);
                    m_objects[node1.Text].Clear();
                }
                else
                {
                    m_attributes[node1.Text].Add(node3.Text);
                }
            }
        }

        /* to be run when two objects land in the same tile (when moving or pushing) */
        private void intersect(MapObject node1, MapObject node2, string direction)
        {
            if (node2 == null)
            {
                return;
            }

            foreach (string attr in m_attributes[node2.Noun])
            {
                switch (attr)
                {
                    case ("push"):
                        // recursively push objects with push attribute
                        // intersect(node2, (MapObject) m_map[node2.Coord.Item1+1][node2.Coord.Item2], direction);
                        break;
                    case ("sink"):
                        // remove anything but a word and the sink tile
                        m_map.Remove(node1);
                        m_map.Remove(node2);

                        break;
                    case ("kill"):
                        // remove an object that is "you"
                        if (m_attributes[node1.Noun].Contains("you"))
                        {
                            // kill
                        }
                        break;
                    case ("stop"):
                        // can't move past
                        break;
                    case ("win"):
                        // win the level
                        if (m_attributes[node1.Noun].Contains("you"))
                        {
                            Win = true;
                        }
                        break;
                }
            }
        }
    }
}
