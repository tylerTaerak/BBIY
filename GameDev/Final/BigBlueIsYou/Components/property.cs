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
    }
}
