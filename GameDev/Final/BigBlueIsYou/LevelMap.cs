using System.Collections.Generic;

namespace CS5410
{
    class LevelMap
    {
        private class MapNode
        {
            INoun m_noun;
            List<IAttr> m_attr;
        }

        public LevelMap(
                int xDim,
                int yDim
                )
        {
        }

        public LevelMap(
                int[,] objects
                )
        {
        }
    }
}
