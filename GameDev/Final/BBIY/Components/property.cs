namespace CS5410.Components
{
    public enum Properties:int
    {
        You = 1, // this will control input
        Win = 2,
        Defeat = 4,
        Push = 8,
        Sink = 16,
        Stop = 32
    }

    public class Property : Component
    {
        // bitwise-set properties of an entity
        public int GameProperties
        {
            get;
            set;
        }

        public Property()
        {
            GameProperties = 0;
        }

        public bool hasProperty(Properties p)
        {
            return ((GameProperties & (int)p) != 0);
        }

        public void Add(Properties p)
        {
            /* bitwise OR to turn bit on */
            GameProperties |= (int) p;
        }

        public void Remove(Properties p)
        {
            /* bitwise XOR to turn bit off */
            GameProperties ^= (int) p;
        }

        public void Clear()
        {
            GameProperties &= 0;
        }

        public override Property Copy()
        {
            var newProp = new Property();
            newProp.GameProperties = this.GameProperties;
            return newProp;
        }
    }
}
